using System.Threading;
using UnityEngine;
using UnityEngine.Android;
using UnityEngine.UI;

public class DeviceScript : MonoBehaviour
{
	private static DeviceScript instance;

	private Camera mainCamera;
	private Compass compassService;
	private LocationInfo lastLocationInformation;
	private float lastNorth;
	private bool northCalibrated;
	private LocationService locationService;
	private float nextTime = 0.0f;

	public UIScript UiInstance;
	public Text northRotationText;

	void Start()
	{
		#region Inicialização e verificação de permissões
		instance = this;
		northCalibrated = false;

		compassService = UnityEngine.Input.compass;
		compassService.enabled = true;

		mainCamera = GetComponentInChildren<Camera>();

		if (!Permission.HasUserAuthorizedPermission(Permission.FineLocation))
			Permission.RequestUserPermission(Permission.FineLocation);
		#endregion

		locationService = UnityEngine.Input.location;
		locationService.Start(10, 0.01f);

		new Thread(new ThreadStart(() =>
		{
			while (true)
			{
				lastLocationInformation = locationService.lastData;
				Thread.Sleep(1000);
			}
		})).Start();
	}

	void Update()
	{
		lastNorth = compassService.magneticHeading;
				
		if (mainCamera.transform.rotation.eulerAngles.x > 180)
		{
			lastNorth += 180;

			if (lastNorth > 360)
				lastNorth -= 360;
		}

		#region Calibragem
		if (!northCalibrated)
		{
			UiInstance.AddLog($"Calibrating north...");
			UiInstance.AddLog($"Camera global rotation: {mainCamera.transform.rotation.eulerAngles.y}");
			UiInstance.AddLog($"Last north: {lastNorth}");

			transform.rotation = Quaternion.AngleAxis(lastNorth, Vector3.up);
			mainCamera.transform.localRotation = Quaternion.Euler(mainCamera.transform.localRotation.eulerAngles.x, 0f, mainCamera.transform.localRotation.eulerAngles.z);

			UiInstance.AddLog($"New camera local rotation: {mainCamera.transform.localRotation.eulerAngles}");
			northCalibrated = lastNorth > 0;
		}

		northRotationText.text = $"Last north: {lastNorth}º \nCamera rotation: {mainCamera.transform.rotation.eulerAngles}";
		#endregion
	}

	internal Camera Camera()
	{
		return mainCamera;
	}

	internal static DeviceScript Instance()
	{
		return instance;
	}

	internal float LastLatitude()
	{
		return lastLocationInformation.latitude;
	}

	internal float LastLongitude()
	{
		return lastLocationInformation.longitude;
	}

	internal float LastNorth()
	{
		return lastNorth;
	}
}

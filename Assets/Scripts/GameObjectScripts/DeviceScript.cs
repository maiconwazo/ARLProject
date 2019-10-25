using System.Threading;
using UnityEngine;
using UnityEngine.Android;

public class DeviceScript : MonoBehaviour
{
	private static DeviceScript _device;

	private Camera _firstPersonCamera;
	private Compass _compassService;
	private LocationService _locationService;
	private LocationInfo _lastLocationInformation;
	private LineRenderer _northLineRender;
	private float _lastNorth;
	private float _initialNorth;
	private bool _northCalibrated;

	void Start()
	{
		_device = this;
		_northCalibrated = false;

		_compassService = UnityEngine.Input.compass;
		_compassService.enabled = true;

		_northLineRender = GetComponentInChildren<LineRenderer>();
		_northLineRender.enabled = false;

		_firstPersonCamera = GetComponentInChildren<Camera>();

		if (!Permission.HasUserAuthorizedPermission(Permission.FineLocation))
			Permission.RequestUserPermission(Permission.FineLocation);

		_locationService = UnityEngine.Input.location;
		_locationService.Start(10, 0.01f);

		int wait = 20;
		while (_locationService.status == LocationServiceStatus.Initializing && wait > 0)
		{
			new WaitForSeconds(1);
			wait--;
		}

		if (wait < 1)
		{
			MessagesHandler.AddError("Tempo esgotado ao tentar acessar o serviço de localização (GPS).");
		}
		else
		{
			if (_locationService.isEnabledByUser)
			{
				new Thread(new ThreadStart(() =>
				{
					while (true)
					{
						_lastLocationInformation = _locationService.lastData;
						Thread.Sleep(1000);
					}
				})).Start();
			}
			else
			{
				MessagesHandler.AddError("O serviço de localização (GPS) está desabilitado, favor verifique as configurações do dispositivo.");
			}
		}
	}
	void Update()
	{
		_lastNorth = _compassService.magneticHeading;

		if (_firstPersonCamera.transform.rotation.eulerAngles.x > 180)
		{
			_lastNorth += 180;

			if (_lastNorth > 360)
				_lastNorth -= 360;
		}
		
		if (!_northCalibrated)
		{
			_initialNorth = _lastNorth;
			transform.rotation = Quaternion.AngleAxis(_lastNorth, Vector3.up);
			_northCalibrated = _lastNorth > 0;

			_northLineRender.SetPosition(0, new Vector3(_firstPersonCamera.transform.position.x, -1f, _firstPersonCamera.transform.position.z));
			_northLineRender.SetPosition(1, new Vector3(0f, -1f, 15f));
			_northLineRender.enabled = _northCalibrated;
		}
	}

	public static DeviceScript GetDevice()
	{
		return _device;
	}

	public Camera GetFirstPersonCamera()
	{
		return _firstPersonCamera;
	}

	public float GetLastLat()
	{
		return _lastLocationInformation.latitude;
	}

	public float GetLastNorth()
	{
		return _lastNorth;
	}
	
	public float GetInitialNorth()
	{
		return _initialNorth;
	}

	public float GetLastLong()
	{
		return _lastLocationInformation.longitude;
	}

	public void RecalibrateNorth()
	{
		_northCalibrated = false;
	}
}

using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

#if UNITY_EDITOR
#endif

public class UIController : MonoBehaviour
{
	#region Display information
	private InformationToDisplay _northInformation;
	private InformationToDisplay _initialNorthInformation;
	private InformationToDisplay _accelerometerInformation;
	#endregion

	private DeviceScript _device;
	private Camera _firstPersonCamera;
	
	public Camera OverHeadCamera;
	public List<InformationToDisplay> InformationsToDisplay;

	void Start()
	{
		_device = DeviceScript.GetDevice();
		_firstPersonCamera = _device.GetFirstPersonCamera();

		if (OverHeadCamera != null)
			OverHeadCamera.enabled = false;

		_firstPersonCamera.enabled = true;

		_northInformation = InformationsToDisplay.Where(info => info.Name.Equals("NorthText")).FirstOrDefault();
		_initialNorthInformation = InformationsToDisplay.Where(info => info.Name.Equals("InitialNorthText")).FirstOrDefault();
		_accelerometerInformation = InformationsToDisplay.Where(info => info.Name.Equals("AccelerometerText")).FirstOrDefault();
	}

	void Update()
	{
		if (_northInformation != null)
		{
			_northInformation.Text.text = $"Norte: {_device.GetLastNorth().ToString("0")}º";
		}

		if (_initialNorthInformation != null)
		{
			_initialNorthInformation.Text.text = $"Norte inicial: {_device.GetInitialNorth().ToString("0")}º";
		}

		if (_accelerometerInformation != null)
		{
			_accelerometerInformation.Text.text = $"Acelerômetro: {_device.GetAccelerometer().ToString()}";
		}
	}

	public void SwitchCamera(Button button)
	{
		if (OverHeadCamera != null)
		{
			if (OverHeadCamera.enabled)
			{
				button.GetComponentInChildren<Text>().text = "Visão topológica";
				OverHeadCamera.enabled = false;
				_firstPersonCamera.enabled = true;
			}
			else
			{
				button.GetComponentInChildren<Text>().text = "Primeira pessoa";
				_firstPersonCamera.enabled = false;
				OverHeadCamera.enabled = true;
			}
		}
	}
}

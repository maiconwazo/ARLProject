using System.Collections;
using UnityEngine;

public class GyroScript : MonoBehaviour
{
	// STATE
	private float _initialYAngle = 0f;
	private float _appliedGyroYAngle = 0f;
	private float _calibrationYAngle = 0f;
	private Transform _rawGyroRotation;
	private float _tempSmoothing;

	// SETTINGS
	[SerializeField] private float _smoothing = 0.0f;

	private IEnumerator Start()
	{
		Input.gyro.enabled = true;
		Application.targetFrameRate = 60;
		_initialYAngle = transform.parent.eulerAngles.y;

		_rawGyroRotation = new GameObject("GyroRaw").transform;
		_rawGyroRotation.position = transform.position;
		_rawGyroRotation.rotation = transform.rotation;

		yield return new WaitForSeconds(1);
	}

	private void Update()
	{
		ApplyGyroRotation();

		transform.rotation = _rawGyroRotation.rotation;// Quaternion.Slerp(transform.rotation, _rawGyroRotation.rotation, _smoothing);
	}

	private IEnumerator CalibrateYAngle()
	{
		_tempSmoothing = _smoothing;
		_smoothing = 1;
		_calibrationYAngle = _appliedGyroYAngle - _initialYAngle; // Offsets the y angle in case it wasn't 0 at edit time.
		yield return null;
		_smoothing = _tempSmoothing;
	}

	private void ApplyGyroRotation()
	{
		_rawGyroRotation.rotation = Input.gyro.attitude;
		_rawGyroRotation.Rotate(0f, 0f, 180f, Space.Self); // Swap "handedness" of quaternion from gyro.
		_rawGyroRotation.Rotate(90f, 180f, 0f, Space.World); // Rotate to make sense as a camera pointing out the back of your device.
		_appliedGyroYAngle = _rawGyroRotation.eulerAngles.y; // Save the angle around y axis for use in calibration.
	}

	public void SetEnabled(bool value)
	{
		enabled = true;
		StartCoroutine(CalibrateYAngle());
	}
}
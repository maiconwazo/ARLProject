using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectInfo
{
	private float _latitude;
	private float _longitude;
	private GameObject _modelPrefab;
	private bool _positionated;

    public ObjectInfo(float latitude, float longitude, GameObject modelPrefab)
	{
		_latitude = latitude;
		_longitude = longitude;
		_modelPrefab = modelPrefab;
		_positionated = false;
	}

	public bool IsInRangeAndNotPositionated(float latitude, float longitude)
	{
		return ValueInRange(_latitude, latitude) && ValueInRange(_longitude, longitude) && !_positionated;
	}

	private bool ValueInRange(float objectValue, float deviceValue)
	{
		float range = WorldControllerScript.GetWorldControllerScriptInstance().GetRangeValueToTrackObjects();

		float minValue = deviceValue - range;
		float maxValue = deviceValue + range;

		return objectValue > minValue && objectValue < maxValue;
	}

	public void SetPositionated(bool value)
	{
		_positionated = value;
	}

	public float GetLatitude()
	{
		return _latitude;
	}

	public float GetLongitude()
	{
		return _longitude;
	}

	public GameObject GetModel()
	{
		return _modelPrefab;
	}
}

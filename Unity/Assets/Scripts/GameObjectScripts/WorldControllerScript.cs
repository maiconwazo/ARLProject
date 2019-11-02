using GoogleARCore;
using System.Collections.Generic;
using UnityEngine;

public class WorldControllerScript : MonoBehaviour
{
	private static WorldControllerScript _worldControllerInstance;

	public DeviceScript Device;
	public PrefabsController Prefabs;

	private float _rangeValueToTrackObjects;

	void Start()
	{
		_worldControllerInstance = this;
	}

	void Update()
	{
		if (GetARCoreStatus() != SessionStatus.Tracking)
		{
			// talvez precise recalibrar o norte aqui, caso pare de atualizar a rotação do aparelho quando não está rastreando
			return;
		}

		float latitude = Device.GetLastLat();
		float longitude = Device.GetLastLong();

		List<ObjectInfo> trackedObjectsList = ObjectsDataBase.GetTrackedObjects(latitude, longitude);
		foreach (var trackedObject in trackedObjectsList)
		{
			GameObject newObject = Instantiate(Prefabs.DefaultObjectContainerPrefab);
			ObjectScript objectScript = newObject.GetComponent<ObjectScript>();
			if (objectScript != null)
			{
				float relativeAngle = Functions.AngleBetween(Device.GetLastLat(), Device.GetLastLong(), trackedObject.GetLatitude(), trackedObject.GetLongitude());
				float distance = Functions.DistanceBetween(Device.GetLastLat(), Device.GetLastLong(), trackedObject.GetLatitude(), trackedObject.GetLongitude());
				Vector3 position = Quaternion.AngleAxis(relativeAngle, Vector3.up) * Vector3.forward * distance;

				Pose pose = new Pose(position, Quaternion.identity);
				Anchor anchor = Session.CreateAnchor(pose);
				newObject.transform.localScale *= 0.1f;
				newObject.transform.rotation = Quaternion.identity;
				newObject.transform.position = pose.position;
				newObject.transform.parent = anchor.transform;

				objectScript.PlaceObject(trackedObject);
			}
		}
	}

	public SessionStatus GetARCoreStatus()
	{
		return Session.Status;
	}

	public static WorldControllerScript GetWorldControllerScriptInstance()
	{
		return _worldControllerInstance;
	}

	public float GetRangeValueToTrackObjects()
	{
		return _rangeValueToTrackObjects;
	}

	public void SetRangeValueToTrackObjects(float value)
	{
		_rangeValueToTrackObjects = value;
	}
}

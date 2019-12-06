using GoogleARCore;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class WorldScript : MonoBehaviour
{
	private static WorldScript instance;

	private float rangeToTrack;
	private Dictionary<string, ItemInfo> nearItems;
	private bool isRunningItemSerch;
	private object nearItemsLock = new object();

	public DeviceScript DeviceInstance;
	public UIScript UiInstance;
	public GameObject DefaultItemModel;


	void Start()
	{
		instance = this;
		rangeToTrack = 1f;
		isRunningItemSerch = false;
	}

	private IEnumerator FetchNearItems(Dictionary<string, ItemInfo> foundItems)
	{
		isRunningItemSerch = true;
		while (true)
		{
			var items = AndroidMethods.GetNearItems(DeviceInstance.LastLatitude(), DeviceInstance.LastLongitude(), UiInstance);

			foreach (ItemInfo item in items)
			{
				UiInstance.AddLog($"Received:");
				UiInstance.AddLog($"	Id: {item.Id}");
				UiInstance.AddLog($"	Latitude: {item.Latitude}");
				UiInstance.AddLog($"	Longitude: {item.Longitude}");
				UiInstance.AddLog($"	Placed: {item.Placed}");

				lock (nearItemsLock)
				{
					try
					{
						if (!foundItems.ContainsKey(item.Id))
						{
							foundItems.Add(item.Id, item);
						}
					} catch (Exception e)
					{
						UiInstance.AddLog(e.Message);
					}
				}
			}

			yield return new WaitForSeconds(5);
		}
	}

	void Update()
	{
		if (Session.Status != SessionStatus.Tracking)
		{
			return;
		}

		if (!isRunningItemSerch)
		{
			nearItems = new Dictionary<string, ItemInfo>();
			StartCoroutine(FetchNearItems(nearItems));
		}

		float latitude = DeviceInstance.LastLatitude();
		float longitude = DeviceInstance.LastLongitude();

		List<ItemInfo> items;
		lock (nearItemsLock)
		{
			items = nearItems.Select(item => item.Value).Where(item => !item.Placed).ToList();
		}

		foreach (var itemInfo in items)
		{
			UiInstance.AddLog($"Placing item {itemInfo.Id}");
			UiInstance.AddLog($"Data:");
			UiInstance.AddLog($"		Id: {itemInfo.Id}");
			UiInstance.AddLog($"		Latitude: {itemInfo.Latitude}");
			UiInstance.AddLog($"		Longitude: {itemInfo.Longitude}");
			UiInstance.AddLog($"Current Latitude: {latitude} - Current Longitude: {longitude}");

			float relativeAngle = Functions.AngleBetween(latitude, longitude, itemInfo.Latitude, itemInfo.Longitude);
			float distance = Functions.HaversineDistance(latitude, longitude, itemInfo.Latitude, itemInfo.Longitude);
			var position = Quaternion.AngleAxis(relativeAngle, Vector3.up) * Vector3.forward * distance;

			UiInstance.AddLog($"Calculated position {position}");

			Pose pose = new Pose(position, Quaternion.identity);
			Anchor anchor = Session.CreateAnchor(pose);

			var newItem = Instantiate(DefaultItemModel);
			newItem.transform.position = position;
			newItem.transform.rotation = Quaternion.identity;
			newItem.transform.parent = anchor.transform ;

			var itemScript = newItem.AddComponent<ItemScript>();

			itemScript.PlaceItem(itemInfo, position);
			itemInfo.Placed = true;
		}
	}

	public static WorldScript WorldInstance()
	{
		return instance;
	}

	public float RangeToTrack()
	{
		return rangeToTrack;
	}
}

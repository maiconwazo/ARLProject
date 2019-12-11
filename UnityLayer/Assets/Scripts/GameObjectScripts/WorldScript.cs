using GoogleARCore;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class WorldScript : MonoBehaviour
{
	private static WorldScript instance;
	private Dictionary<string, ItemInfo> nearItems;
	private bool isRunningItemSerch;
	private object nearItemsLock = new object();
	private bool debugMode;
	private List<GameObject> listaObjetos;
	private bool listarObjetos;

	public DeviceScript DeviceInstance;
	public UIScript UiInstance;
	public GameObject DefaultItemModel;


	void Start()
	{
		instance = this;
		isRunningItemSerch = false;
		debugMode = false;
		listaObjetos = new List<GameObject>();
		listarObjetos = false;
	}

	public void RemoverItem(string id)
	{
		nearItems.Remove(id);
	}

	private IEnumerator FetchNearItems()
	{
		isRunningItemSerch = true;
		nearItems = new Dictionary<string, ItemInfo>();
		while (true)
		{
			var items = AndroidMethods.GetNearItems(DeviceInstance.LastLatitude(), DeviceInstance.LastLongitude());

			foreach (ItemInfo item in items)
			{
				lock (nearItemsLock)
				{
					try
					{
						if (!nearItems.ContainsKey(item.Id))
						{
							nearItems.Add(item.Id, item);
						}
					}
					catch (Exception e)
					{
						UIScript.AddLog(e.Message);
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
			StartCoroutine(FetchNearItems());
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
			float relativeAngle = Functions.AngleBetween(latitude, longitude, itemInfo.Latitude, itemInfo.Longitude);
			float distance = Functions.HaversineDistance(latitude, longitude, itemInfo.Latitude, itemInfo.Longitude);
			var position = Quaternion.AngleAxis(relativeAngle, Vector3.up) * Vector3.forward * distance;

			UIScript.AddLog($"{itemInfo.Id} - {distance}m");
			UIScript.AddLog($"{itemInfo.Id} - {relativeAngle}º");
			UIScript.AddLog($"{itemInfo.Id} - Coords: ({itemInfo.Latitude}, {itemInfo.Longitude})");

			//Pose pose = new Pose(position, Quaternion.identity);
			//Anchor anchor = Session.CreateAnchor(pose);

			var newItem = Instantiate(DefaultItemModel);
			newItem.transform.position = position;
			newItem.transform.rotation = Quaternion.identity;
			//newItem.transform.parent = anchor.transform;
			listaObjetos.Add(newItem);

			var itemScript = newItem.AddComponent<ItemScript>();

			itemScript.PlaceItem(itemInfo, position);
			itemInfo.Placed = true;
		}

		if (listarObjetos)
		{
			listarObjetos = false;
			UIScript.AddLog($"Quantidade de itens: {listaObjetos.Count}");

			foreach (GameObject o in listaObjetos)
			{
				var script = o.GetComponent<ItemScript>();


				if (o != null)
				{
					UIScript.AddLog($"{script.ItemInfo.Id} - {o.transform.position} - Lat: {script.ItemInfo.Latitude} Long: {script.ItemInfo.Longitude}");
				}
				else
				{
					UIScript.AddLog("nulo");
				}
			}
		}
	}

	public static WorldScript WorldInstance()
	{
		return instance;
	}

	public bool IsDebugMode()
	{
		return debugMode;
	}

	public void AlterDebugMode()
	{
		debugMode = !debugMode;

		if (debugMode)
		{
			listarObjetos = true;
		}
	}

	public void RemoverObjeto(GameObject obj)
	{
		listaObjetos.Remove(obj);
	}
}

using System;
using System.Collections.Generic;
using UnityEngine;

public class AndroidMethods
{
	public static List<ItemInfo> GetNearItems(float latitude, float longitude)
	{
		ItemInfo[] itemInfoArray = null;

		try
		{
			var arlWorld = new AndroidJavaClass("com.FURB.ARLibrary.lib.ARLWorld");
			var arlWorldInstance = arlWorld.CallStatic<AndroidJavaObject>("getARLWorldIntance");
			var json = arlWorldInstance.Call<string>("getNearItems", latitude, longitude);
			itemInfoArray = JsonHelper.FromJson<ItemInfo>(json);
		}
		catch
		{
			itemInfoArray = new ItemInfo[] { new ItemInfo(new Guid().ToString(), DeviceScript.Instance().LastLatitude(), DeviceScript.Instance().LastLongitude(), "") };
		}

		var items = new List<ItemInfo>();
		foreach (ItemInfo itemInfo in itemInfoArray)
		{
			items.Add(itemInfo);
		}

		return items;
	}
}

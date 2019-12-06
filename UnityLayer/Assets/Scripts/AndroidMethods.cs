using System;
using System.Collections.Generic;
using UnityEngine;

public class AndroidMethods
{
	internal static List<ItemInfo> GetNearItems(float latitude, float longitude, UIScript log)
	{
		ItemInfo[] itemInfoArray = null;

		try
		{
			var arlWorld = new AndroidJavaClass("com.FURB.ARLibrary.lib.ARLWorld");
			var arlWorldInstance = arlWorld.CallStatic<AndroidJavaObject>("getARLWorldIntance");
			var json = arlWorldInstance.Call<string>("getNearItems");
			itemInfoArray = JsonHelper.FromJson<ItemInfo>(json);
		}
		catch (Exception e)
		{
			log.AddLog("Erro: " + e.Message);
		}

		var items = new List<ItemInfo>();
		foreach (ItemInfo itemInfo in itemInfoArray)
		{
			items.Add(itemInfo);
		}

		return items;
	}

	internal static void AddMessage(string message)
	{
	}
}

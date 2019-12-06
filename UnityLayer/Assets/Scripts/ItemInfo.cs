using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class ItemInfo
{
	public string Id;
	public float Latitude;
	public float Longitude;
	public bool Placed;
	public string Wavefront;
	
	public ItemInfo(string id, float latitude, float longitude, string wavefront)
	{
		Id = id;
		Latitude = latitude;
		Longitude = longitude;
		Wavefront = wavefront;
		Placed = false;
	}
}

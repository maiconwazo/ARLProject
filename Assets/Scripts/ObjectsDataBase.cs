using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ObjectsDataBase
{	
	private static List<ObjectInfo> Objects;

	public static List<ObjectInfo> GetAllObjects()
	{
		if (Objects == null)
			Objects = new List<ObjectInfo>();

		return Objects;
	}

	public static void AddObject(ObjectInfo objectInfo)
	{		
		GetAllObjects().Add(objectInfo);
	}

	public static List<ObjectInfo> GetTrackedObjects(float latitude, float longitude)
	{
		return GetAllObjects().Where(obj => obj.IsInRangeAndNotPositionated(latitude, longitude)).ToList();
	}
}

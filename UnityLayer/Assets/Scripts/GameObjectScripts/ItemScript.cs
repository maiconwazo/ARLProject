using System;
using UnityEngine;

public class ItemScript : MonoBehaviour
{
	private Mesh mesh;
	private TextMesh info;
	private LineRenderer line;
	private DeviceScript device;
	private ItemInfo itemInfo;
	private Vector4 lastCalculatedValues;

	void Start()
	{
		info = GetComponentInChildren<TextMesh>();
		info.gameObject.SetActive(false);

		line = GetComponentInChildren<LineRenderer>();
		line.gameObject.SetActive(false);

		gameObject.AddComponent<MeshFilter>().mesh = mesh;

		device = DeviceScript.Instance();
	}

	void Update()
	{
		if (line != null)
		{
			var pos0 = new Vector3(this.device.Camera().transform.position.x, 0, this.device.Camera().transform.position.z);
			var pos1 = new Vector3(this.transform.position.x, 0, this.transform.position.z);

			line.SetPosition(0, pos0);
			line.SetPosition(1, pos1);
			line.gameObject.SetActive(true);
		}

		if (info != null)
		{
			info.transform.position = transform.position + new Vector3(0, (mesh.bounds.extents.y / 2) + 1f, 0);
			info.gameObject.SetActive(true);

			Vector4 valuesToCalculate = new Vector4(itemInfo.Latitude, itemInfo.Longitude, device.LastLatitude(), device.LastLongitude());

			if (lastCalculatedValues != valuesToCalculate)
			{
				info.text = $"{Functions.HaversineDistance(itemInfo.Latitude, itemInfo.Longitude, device.LastLatitude(), device.LastLongitude()).ToString()}m";
				lastCalculatedValues = new Vector4(itemInfo.Latitude, itemInfo.Longitude, device.LastLatitude(), device.LastLongitude());
			}
		}
	}
	
	public void PlaceItem(ItemInfo itemInfo, Vector3 position)
	{
		this.itemInfo = itemInfo;
		mesh = MeshBuilder.Instance.BuildMesh(itemInfo.Wavefront);
	}
}

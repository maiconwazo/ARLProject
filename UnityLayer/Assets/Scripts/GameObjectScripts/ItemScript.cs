using UnityEngine;

public class ItemScript : MonoBehaviour
{
	private Mesh mesh;
	private TextMesh info;
	private LineRenderer line;
	private DeviceScript device;
	public ItemInfo ItemInfo;

	void Start()
	{
		info = GetComponentInChildren<TextMesh>();
		info.gameObject.SetActive(false);

		line = GetComponentInChildren<LineRenderer>();
		line.gameObject.SetActive(false);

		if (mesh != null)
		{
			gameObject.AddComponent<MeshFilter>().mesh = mesh;
		}

		device = DeviceScript.Instance();
	}

	void Update()
	{
		var isDebug = WorldScript.WorldInstance().IsDebugMode();
		var distancia = Vector3.Distance(transform.position, this.device.Camera().transform.position);

		if (distancia > 500)
		{
			ItemInfo.Placed = false;
			WorldScript.WorldInstance().RemoverItem(ItemInfo.Id);
			WorldScript.WorldInstance().RemoverObjeto(this.gameObject);
			Destroy(this.gameObject);
		}

		if (isDebug)
		{
			var pos0 = new Vector3(this.device.Camera().transform.position.x, 0, this.device.Camera().transform.position.z);
			var pos1 = new Vector3(this.transform.position.x, 0, this.transform.position.z);

			line.SetPosition(0, pos0);
			line.SetPosition(1, pos1);

			if (mesh != null)
				info.transform.position = transform.position + new Vector3(0, (mesh.bounds.extents.y / 2) + 1f, 0);

			info.text = $"{distancia}m";
			info.transform.rotation = Quaternion.LookRotation(info.transform.position - this.device.Camera().transform.position);
			
			line.gameObject.SetActive(true);
			info.gameObject.SetActive(true);
		}
		else
		{
			line.gameObject.SetActive(false);
			info.gameObject.SetActive(false);
		}
	}

	public void PlaceItem(ItemInfo itemInfo, Vector3 position)
	{
		UIScript.AddLog($"{itemInfo.Id} PlaceItem");
		this.ItemInfo = itemInfo;

		if (!string.IsNullOrWhiteSpace(itemInfo.Wavefront))
			mesh = MeshBuilder.Instance.BuildMesh(itemInfo.Wavefront);
	}
}

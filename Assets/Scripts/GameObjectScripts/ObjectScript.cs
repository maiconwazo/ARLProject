using System;
using UnityEngine;

public class ObjectScript : MonoBehaviour
{
	private TextMesh _info;
	private LineRenderer _line;
	private DeviceScript _device;
	private GameObject _model;
	private GameObject _modelInstance;

	private float _lat;
	private float _long;
	private bool _ready;

	private Vector4 _lastCalculatedValues;


	void Start()
	{
		_info = GetComponentInChildren<TextMesh>();
		_info.gameObject.SetActive(false);

		_line = GetComponentInChildren<LineRenderer>();
		_line.gameObject.SetActive(false);

		_device = DeviceScript.GetDevice();
		_ready = false;
	}

	void Update()
	{
		if (!_ready)
			return;

		if (_line != null)
		{
			_line.SetPosition(0, this._device.transform.position);
			_line.SetPosition(1, this.transform.position);
		}

		if (_info != null)
		{
			_info.transform.position = _modelInstance.transform.position + new Vector3(0, (_modelInstance.GetComponent<MeshFilter>().mesh.bounds.extents.y / 2) + .5f, 0);
			Vector4 valuesToCalculate = new Vector4(_lat, _long, _device.GetLastLat(), _device.GetLastLong());

			if (_lastCalculatedValues != valuesToCalculate)
			{
				_info.text = $"{Functions.DistanceBetween(_lat, _long, _device.GetLastLat(), _device.GetLastLong()).ToString()}m";
				_lastCalculatedValues = new Vector4(_lat, _long, _device.GetLastLat(), _device.GetLastLong());
			}
		}
	}

	public void SetCoordinates(float latitude, float longitude)
	{
		_lat = latitude;
		_long = longitude;
	}

	public void SetModel(GameObject model)
	{
		_model = model;
	}

	public void PlaceObject(ObjectInfo objectInfo)
	{
		SetCoordinates(objectInfo.GetLatitude(), objectInfo.GetLongitude());
		SetModel(objectInfo.GetModel());

		if (_lat == 0 && _long == 0)
			throw new Exception("Erro ao posicionar objeto. Coordenadas não foram definidas.");

		if (_model == null)
			throw new Exception("Erro ao posicionar objeto. Modelo não foi definido.");

		_modelInstance = Instantiate(_model, new Vector3(0, 0, 0), Quaternion.identity, transform) as GameObject;
		_info.gameObject.SetActive(true);
		_line.gameObject.SetActive(true);

		_ready = true;
		objectInfo.SetPositionated(true);
	}
}

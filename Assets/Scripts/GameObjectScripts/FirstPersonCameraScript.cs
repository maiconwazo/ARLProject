using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstPersonCameraScript : MonoBehaviour
{
	private static GameObject _firstPersonCameraGameObject;
	private static Camera _firstPersonCamera;

	void Start()
    {
		_firstPersonCameraGameObject = this.gameObject;
		_firstPersonCamera = GetComponent<Camera>();
	}

	public static GameObject GetFirstPersonCameraGameObject()
	{
		return _firstPersonCameraGameObject;
	}

	void Update()
    {
        
    }
}

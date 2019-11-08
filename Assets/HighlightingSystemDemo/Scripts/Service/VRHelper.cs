using UnityEngine;
using UnityEngine.XR;
using System.Collections;
using System.Collections.Generic;

[DisallowMultipleComponent]
public class VRHelper : MonoBehaviour
{
	static public bool isVRScene = false;
	static private VRDeviceType1 _vrDeviceType = VRDeviceType1.None;

	// 
	void Start()
	{
		SetVRDeviceType(VRDeviceType1.Split);
		isVRScene = true;
	}

	// 
	void OnDisable()
	{
		SetVRDeviceType(VRDeviceType1.None);
		isVRScene = false;
	}

	// 
	static public void SetVRDeviceType(VRDeviceType1 vrDeviceType)
	{
		if (_vrDeviceType == vrDeviceType) { return; }

		_vrDeviceType = vrDeviceType;
		//UnityEngine.XR.XRSettings.loadedDevice = _vrDeviceType;
		bool vr = (_vrDeviceType != VRDeviceType1.None);
		UnityEngine.XR.XRSettings.showDeviceView = vr;
		UnityEngine.XR.XRSettings.enabled = vr;
	}
}
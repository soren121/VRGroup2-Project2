using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VR;
using UnityEngine.Networking;

public class GameManager: NetworkBehaviour {

	public GameObject vrCameraRig;
    public GameObject vrCameraRigControlCenter;
	public GameObject nonVRCameraRig;
	public GameObject rover;
	[HideInInspector]
    public bool roverCameraSet;
	//public Blinker hmdBlinker;
	public SteamVR_TrackedObject hmd;
	public SteamVR_TrackedObject controllerLeft;
	public SteamVR_TrackedObject controllerRight;
	// Use this for initialization
	void Start () {
		
	}
	public void enableRover()
    {
        roverCameraSet = true;
    }
	// Update is called once per frame
	void Update () {
		
	}

	public void enableVR()
	{
		StartCoroutine(doEnableVR());
	}

	IEnumerator doEnableVR()
	{
        vrCameraRigControlCenter.SetActive(false);
        vrCameraRig.SetActive(false);
        while (UnityEngine.XR.XRSettings.loadedDeviceName != "OpenVR")
		{
			UnityEngine.XR.XRSettings.LoadDeviceByName("OpenVR");
			yield return null;
		}
		UnityEngine.XR.XRSettings.enabled = true;

        if (roverCameraSet)
        {
           
            vrCameraRig.SetActive(true);
        }
        else
        {
            vrCameraRigControlCenter.SetActive(true);
            
        }
        nonVRCameraRig.SetActive(false);
	}
}

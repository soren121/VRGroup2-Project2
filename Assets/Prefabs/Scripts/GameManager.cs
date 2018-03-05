using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VR;
public class GameManager: MonoBehaviour {

	public GameObject vrCameraRig;
    public GameObject vrCameraRigControlCenter;
	public GameObject nonVRCameraRig;
    public bool rovercameraset;
	//public Blinker hmdBlinker;
	public SteamVR_TrackedObject hmd;
	public SteamVR_TrackedObject controllerLeft;
	public SteamVR_TrackedObject controllerRight;
	// Use this for initialization
	void Start () {
		
	}
	public void enableRover()
    {
        rovercameraset = true;
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
		while(UnityEngine.XR.XRSettings.loadedDeviceName != "OpenVR")
		{
			UnityEngine.XR.XRSettings.LoadDeviceByName("OpenVR");
			yield return null;
		}
		UnityEngine.XR.XRSettings.enabled = true;

        if (rovercameraset)
        {
            vrCameraRigControlCenter.SetActive(false);
            vrCameraRig.SetActive(true);
        }
        else
        {
            vrCameraRigControlCenter.SetActive(true);
            vrCameraRig.SetActive(false);
        }
        nonVRCameraRig.SetActive(false);
	}
}

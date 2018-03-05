using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class LittleRoverScript : NetworkBehaviour {

    public GameObject BigRover;
    public GameObject BigCenter;
    public GameObject LittleCenter;

	// Use this for initialization
	void Start () {
		
	}

	[ClientRpc]
	public void UpdateFromBigRover(Transform roverTransform)
	{
		Vector3 differencePos = roverTransform.position - BigCenter.transform.position;
		differencePos = differencePos * .1f;
		gameObject.transform.rotation = roverTransform.rotation;
		differencePos = differencePos * .1f;
		gameObject.transform.position = LittleCenter.transform.position + differencePos;
	}
}

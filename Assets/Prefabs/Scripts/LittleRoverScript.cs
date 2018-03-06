using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class LittleRoverScript : NetworkBehaviour {

	public void UpdateFromBigRover(GameObject bigCenter, GameObject littleCenter, Vector3 roverPos, Quaternion roverRot)
	{
		Vector3 differencePos = roverPos - bigCenter.transform.position;
		differencePos = differencePos * .1f;
		gameObject.transform.rotation = roverRot;
		differencePos = differencePos * .1f;
		gameObject.transform.position = littleCenter.transform.position + differencePos;
	}
}

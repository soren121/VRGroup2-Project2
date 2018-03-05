using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class LittleRoverScript : MonoBehaviour {

    public GameObject BigRover;
    public GameObject BigCenter;
    public GameObject LittleRover;
    public GameObject LittleCenter;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        moveLittleRover();
    }

    [Command]
    public void moveLittleRover()
    {
        Vector3 differencePos = BigRover.transform.position - BigCenter.transform.position;
        differencePos = differencePos * .1f;
        LittleRover.transform.rotation = BigRover.transform.rotation;
        differencePos = differencePos * .1f;
        LittleRover.transform.position = LittleCenter.transform.position + differencePos;
    }
}

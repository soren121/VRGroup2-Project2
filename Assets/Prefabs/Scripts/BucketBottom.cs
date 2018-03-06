using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BucketBottom : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnCollisionEnter(Collision other) {
		if (other.gameObject.GetComponent<GoalObject> () != null) {
			Destroy (other.gameObject);
		}
	}
}

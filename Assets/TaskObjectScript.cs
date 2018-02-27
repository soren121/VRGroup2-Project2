using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TaskObjectScript : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnCollisionEnter(Collision collision)
    {
        StartCoroutine(handleCollision(collision));
    }


    // Determines what hit the floor, and then performs the appropriate action
    private IEnumerator handleCollision(Collision collision)
    {
        if (collision.transform.name == "BigRover")
        {
            collision.rigidbody.isKinematic = false;
            collision.rigidbody.useGravity = true;

        }
        yield return null;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BigTerrainScript : MonoBehaviour {

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
        if (collision.transform.name == "TaskObject(Clone)" && collision.rigidbody.isKinematic == false)
        {
            collision.rigidbody.isKinematic = true;
            collision.rigidbody.useGravity = true;
			Destroy(collision.gameObject.GetComponent<Rigidbody>());
        }
        yield return null;
    }
}

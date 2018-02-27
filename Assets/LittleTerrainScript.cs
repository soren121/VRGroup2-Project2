using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LittleTerrainScript : MonoBehaviour
{
    public GameObject TaskObject;
    public GameObject BigTerrain;
    public GameObject BigCenter;
    public GameObject LittleTerrain;
    public GameObject LittleCenter;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnCollisionEnter(Collision collision)
    {
        StartCoroutine(handleCollision(collision));   
    }


    // Determines what hit the floor, and then performs the appropriate action
    private IEnumerator handleCollision(Collision collision)
    {
        if (collision.transform.name == "TaskObject" && collision.rigidbody.isKinematic == false)
        {
            //Make the TaskObject so that it won't bounce everywhere if you drop it on the terrain
            collision.rigidbody.isKinematic = true;
            yield return new WaitForSeconds(1);

            //get the TaskObject position
            Vector3 otherPos = collision.transform.position;
            Debug.Log(collision.transform.name + " has position position " + collision.transform.name);

            //get the difference from the position of the taskObject and the little terrain center
            Vector3 differencePos = collision.transform.position - LittleCenter.transform.position;

            //scale the coordinates so they will be in the correct position on the big terrain
            differencePos.x += differencePos.x * 100;
            differencePos.z += differencePos.z * 100;
            Vector3 newPos = BigCenter.transform.position + differencePos;

            //create the taskObject in the correct location on the big terrain
            GameObject newTask = GameObject.Instantiate(TaskObject);
            newTask.transform.localScale += new Vector3(10, 10, 10);
            newTask.transform.position += newPos;

            //disable renderer on the taskObject container so that it can be used as a task boundary
            newTask.transform.GetComponent<Renderer>().enabled = false;
            newTask.GetComponent<Rigidbody>().useGravity = true;
            collision.rigidbody.useGravity = false;
                
        }
        yield return null;
    }
}

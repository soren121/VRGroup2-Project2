using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class TaskObjectManager : MonoBehaviour {

    // public int numTasksToSpawn;
    // GameObject[] tasks;
    // TaskObject taskPrefab;

	// Use this for initialization
	void Start () {
        // tasks = new GameObject[numTasksToSpawn];
        // for(int i = 0; i < numTasksToSpawn; i++) { 
        //     // instantiate each TaskObject in same position as parent, but 2 units up so it falls
        //     TaskObject task = GameObject.Instantiate(
        //                                 taskPrefab, // prefab
        //                                 this.transform.localPosition+Vector3.up*2, // pos
        //                                 Quaternion.AngleAxis(0, Vector3.up),  // rot
        //                                 this.transform // parent
        //                             ) as TaskObject;
        // }
	}

    // called by TaskObjects when they are completed
    public void logTaskAndDestroy(int id) {
        TaskObject[] taskObjects = FindObjectsOfType<TaskObject>();
        if(taskObjects.Length >= 0) {
            // loop through TaskObjects in scene and find the TaskObject with the given instanceID. 
            // this will be the TaskObject that called this method upon being completed.
            for(int i = 0; i < taskObjects.Length; i++) {
                if(taskObjects[i].gameObject.GetInstanceID() == id) {
                    // found TaskObject that called this method
                    TaskObject t = taskObjects[i];
                    // log task metadata
                    Debug.Log("spawnMoment: "+t.getSpawnMoment());
                    Debug.Log("roverEnterMoment: "+t.getRoverEnterMoment());
                    Debug.Log("completionMoment: "+t.getCompletionMoment());
                    Debug.Log("numGoalObjects: "+t.getNumGoalObjects());
                    // destroy TaskObject
                    GameObject.Destroy(t.gameObject);
                    break;
                }
            }
        }
    }
	
	// Update is called once per frame
	void Update () {
		
	}

}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Diagnostics;

public class TaskManagerScript : MonoBehaviour {

    public int numTasksToSpawn;
    GameObject[] taskObjects;
    TaskObjectScript taskPrefab;

	// Use this for initialization
	void Start () {
        // set to false so Start() method not called on instantiated task objects
        // before setting the inCommandCenter property
        taskPrefab.gameObject.SetActive(false);
        taskObjects = new GameObject[numTasksToSpawn];
        // instantiate task objects and specify them as inCommandCenter
        // this will disable the stat keeping for these, as they are just meant to model
        // the real tasks, which will be cloned on the Moon surface when dropped onto the WIM
        for(int i = 0; i < numTasksToSpawn; i++) {
            TaskObjectScript task = GameObject.Instantiate(taskPrefab, this.transform.localPosition, Quaternion.AngleAxis(0, Vector3.up), this.transform);
            task.setInCommandCenter(true);
            task.OnTaskCompleted += logTaskAndDestroy;
        }
        // activate prefab so start method is called
        taskPrefab.gameObject.SetActive(true);
	}

    private void logTaskAndDestroy() {
        // loop through TaskObjects in scene and find the TaskObject
        // with the given ID
        TaskObjectScript[] taskObjects = FindObjectsOfType<TaskObjectScript>();
    }
	
	// Update is called once per frame
	void Update () {
		
	}

}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Diagnostics;

public class TaskObject : MonoBehaviour {

    private int numGoalObjects;
    private DateTime spawnMoment;
    private DateTime roverEnterMoment;
    private DateTime completionMoment;
    private TaskObjectManager taskManager;

    public bool isCompleted;

	// Use this for initialization
	void Start () {
        numGoalObjects = transform.childCount;
        spawnMoment = DateTime.UtcNow;
        taskManager = GameObject.FindObjectOfType<TaskObjectManager>();
        StartCoroutine(checkTaskCompleted());
	}

    // something entered the task space
    void OnTriggerEnter(Collider other) {   
        // check if rover entered for the first time
        if(other.gameObject.transform.name == "BigRover" && roverEnterMoment == DateTime.MinValue) {
            roverEnterMoment = DateTime.UtcNow;
            Destroy(GameObject.Find("CheckPoint 1(Clone)"));
        }
    }

    // constantly checks if this task has met completion condition
    private IEnumerator checkTaskCompleted() {
        // busy wait
        while(!isCompleted) yield return null;
        // task has been completed
        onTaskCompleted();
    }

    // called when TaskObject has met completed condition
    private void onTaskCompleted() {
        completionMoment = DateTime.UtcNow;
        taskManager.logTaskAndDestroy(gameObject.GetInstanceID());
    }

    // --------------------------------

    public DateTime getSpawnMoment() {
        return this.spawnMoment;
    }

    public DateTime getRoverEnterMoment() {
        return this.roverEnterMoment;
    }

    public DateTime getCompletionMoment() {
        return this.completionMoment;
    }

    public int getNumGoalObjects() {
        return this.numGoalObjects;
    }

}

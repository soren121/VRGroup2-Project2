using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Diagnostics;

public class TaskObject : MonoBehaviour {

    private GoalObject[] goalObjects;
    private DateTime spawnMoment;
    private DateTime roverEnterMoment;
    private DateTime completionMoment;
    private TaskObjectManager taskManager;

	// Use this for initialization
	void Start () {
        goalObjects = transform.GetComponentsInChildren<GoalObject>();
        spawnMoment = DateTime.UtcNow;
        taskManager = GameObject.FindObjectOfType<TaskObjectManager>();
	}

    // something entered the task space
    void OnTriggerEnter(Collider other) {   
        // check if rover entered for the first time
        if(other.gameObject.transform.name == "BigRover" && roverEnterMoment == DateTime.MinValue) {
            roverEnterMoment = DateTime.UtcNow;
            // let goal objects all roll away
            for(int i = 0; i < goalObjects.Length; i++) {
                goalObjects[i].gameObject.AddComponent<Rigidbody>().isKinematic = false;
            }
            // now start checking (after 5sec delay) if task completed
            StartCoroutine(checkTaskCompleted());
        }
    }

    // constantly checks if this task has met completion condition
    private IEnumerator checkTaskCompleted() {
        // constantly check if all gaol objects are not eaten
		UnityEngine.Debug.Log(transform.GetComponentsInChildren<GoalObject>().Length);
		while(transform.GetComponentsInChildren<GoalObject>().Length > 0) yield return null;
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
        return this.goalObjects.Length;
    }

}

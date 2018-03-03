using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Diagnostics;

public class TaskObject : MonoBehaviour {

    private DateTime spawnMoment;
    private DateTime roverEnterMoment;
    private DateTime completionMoment;
    private TimeSpan travelingTime;
    private TimeSpan workingTime;
    private TimeSpan completionTime;
    private Stopwatch sw;
    private int numGoalObjects;
    private TaskObjectManager taskManager;
    public bool isCompleted;

	// Use this for initialization
	void Start () {
        // initialize times/timers
        spawnMoment = DateTime.UtcNow;
        numGoalObjects = transform.childCount-1;
        sw = new Stopwatch();
        sw.Start();
        // grab TaskManager in scene to handle task completion
        taskManager = GameObject.FindObjectOfType<TaskObjectManager>();
        // constantly check completed condition for this task
        StartCoroutine(checkTaskCompleted());
	}

    // Something hit the task boundary
    void OnTriggerEnter(Collider other)
    {   
        // check if what hit the trigger was the BigRover
        if (other.gameObject.transform.name == "BigRover") {
            // check if this is the first time the rover has entered task space
            if(!sw.IsRunning) {
                roverEnterMoment = DateTime.UtcNow;
                // set the time it took for rover to reach task
                sw.Stop();
                travelingTime = sw.Elapsed;
                sw.Reset();
                sw.Start();
                // unbox the task to 
                other.attachedRigidbody.isKinematic = false;
                other.attachedRigidbody.useGravity = true;
            }
        }
    }

    // constantly checks if this TaskObject has met completed condition
    private IEnumerator checkTaskCompleted() {
        while(true) {
            // check if task completed (right now just a public variable, 
            // but will be some conditional state of inner goal objects)
            if(this.isCompleted) onTaskCompleted();
            yield return null;
        }
    }

    // called when TaskObject has met completed condition.
    // calls method in TaskManager that logs the this task's
    // metadata and destroys it
    private void onTaskCompleted() {
        completionMoment = DateTime.UtcNow;
        // set the time it took for rover to start/finish task
        sw.Stop();
        workingTime = sw.Elapsed;
        completionTime = travelingTime.Add(workingTime);
        // call task manager
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

    public TimeSpan getTravelingTime() {
        return this.travelingTime;
    }

    public TimeSpan getWorkingTime() {
        return this.workingTime;
    }

    public TimeSpan getCompletionTime() {
        return this.completionTime;
    }

    public int getNumGoalObjects() {
        return this.numGoalObjects;
    }

}

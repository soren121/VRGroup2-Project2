using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Diagnostics;

public class TaskObjectScript : MonoBehaviour {

    private DateTime spawnMoment;
    private DateTime roverEnterMoment;
    private DateTime completionMoment;
    private TimeSpan travelingTime;
    private TimeSpan workingTime;
    private TimeSpan completionTime;
    private Stopwatch sw;
    private int numGoalObjects;

	// Use this for initialization
	void Start () {
        spawnMoment = DateTime.UtcNow;
        numGoalObjects = transform.childCount-1;
        sw = new Stopwatch();
        sw.Start();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    // Something hit the task boundary
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.transform.name == "BigRover")
        {
            // check if this is the first time the rover has entered
            if(!sw.IsRunning) {
                roverEnterMoment = DateTime.UtcNow;
                // set the time it took for rover to reach task
                sw.Stop();
                travelingTime = sw.Elapsed;
                sw.Reset();
                sw.Start();
                // unbox the task
                other.attachedRigidbody.isKinematic = false;
                other.attachedRigidbody.useGravity = true;
            }
        }
    }

    void OnDestroy() {
        completionMoment = DateTime.UtcNow;
        // set the time it took for rover to start/finish task
        sw.Stop();
        workingTime = sw.Elapsed;
        completionTime = travelingTime.Add(workingTime);
        // let task manager know this task has been completed
        // TODO: send metadata to task manager
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

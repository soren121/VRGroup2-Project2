using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using SimpleJSON;

public class TaskObjectManager : MonoBehaviour {

    private JSONNode data;

	// Use this for initialization
	void Start () {
        // add root session object
        data = new JSONObject();
        data.Add("session", new JSONObject());
        // initialize session object
        data["session"].AsObject.Add("start", new JSONString(DateTime.UtcNow.ToString()));
        data["session"].AsObject.Add("tasks", new JSONArray());
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
                    // create json representation of task data for logging
                    JSONObject task = new JSONObject();
                    task.Add("spawnMoment", new JSONString(t.getSpawnMoment().ToString()));
                    task.Add("roverEnterMoment", new JSONString(t.getRoverEnterMoment().ToString()));
                    task.Add("completionMoment", new JSONString(t.getCompletionMoment().ToString()));
                    task.Add("numGoalObjects", new JSONNumber(t.getNumGoalObjects()));
                    // add task to session["tasks"] array
                    data["session"]["tasks"].AsArray.Add(task);
                    // debug log the task data
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

    // called when either a) the user unclicks the play button in the editor, or
    // b) the user in the command center clicks the 'end session' button
    void OnApplicationQuit() {
        // set session end time
        data["session"].AsObject.Add("end", new JSONString(DateTime.UtcNow.ToString()));
        // log this in player prefs
    }
	
	// Update is called once per frame
	void Update () {
		
	}

}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoalObject : MonoBehaviour {

	private bool insideTaskSpace;

	// Use this for initialization
	void Start () {
		insideTaskSpace = true;
	}

	// --------------------------------

	public void setInsideTaskSpace(bool insideTaskSpace) {
		this.insideTaskSpace = insideTaskSpace;
	}

	public bool getInsideTaskSpace() {
		return this.insideTaskSpace;
	}
}

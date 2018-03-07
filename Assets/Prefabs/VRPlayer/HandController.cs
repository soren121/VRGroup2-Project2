using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(Rigidbody))]
public class HandController : MonoBehaviour {

	public VRPlayer player;
	public Vector3 controllerVelocity;
	public Vector3 controllerAngularVelocity;

	public Transform attachedObject;
	Transform attachPoint;
	public bool attachExact = true;
	public bool attachHide = false;
	public float breakingDist = Mathf.Infinity;
	private Collider lastIntersection;
	public float maxSpeed = 5.0f; //don't let the object move too fast between frames

	bool attachedKinematicSave;
	float attachedMaxAngularVelSave;
	Transform attachedParentSave;
	public bool attachedUseGravitySave;
	public float squeezeThreshold = .2f;

	public bool teleporterActive = false;

	public Transform teleporterBase; //set in the inspector, z forward is the telporter starting trajectory
	public float teleporterArcSpeed = 10.0f;
	public Vector3 teleporterArcGravity = new Vector3(0, -9.8f, 0);
	public float arcMaxDistance = 100.0f;

	public GameObject teleporterPointPrefab;
	public GameObject teleporterHitVisualization; //set in the inspector
	public List<GameObject> teleporterPoints = new List<GameObject>();  
	// Use this for initialization
	void Start () {
		
		attachPoint = (new GameObject()).transform;
		attachPoint.SetParent(this.transform);
		attachPoint.localPosition = Vector3.zero;
		attachPoint.localRotation = Quaternion.identity;
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	private void FixedUpdate()
	{
		
		if(attachedObject != null)
		{
			Rigidbody rb = attachedObject.GetComponent<Collider>().attachedRigidbody;
			if (rb != null && !rb.isKinematic) 
			{
				Vector3 vel = ((attachPoint.position - attachedObject.position)) / Time.fixedDeltaTime;
				if (vel.magnitude > maxSpeed)
				{
					vel = vel.normalized * maxSpeed;
				}
				rb.velocity = vel;

				Quaternion rotDiff = attachPoint.rotation * Quaternion.Inverse(attachedObject.rotation);
				float angle; Vector3 axis;
				rotDiff.ToAngleAxis(out angle, out axis);
				Vector3 angularVel = axis * angle * Mathf.Deg2Rad/Time.fixedDeltaTime;
				rb.angularVelocity = angularVel;
			}else
			{ //just move it
				attachedObject.position = attachPoint.position;
				attachedObject.rotation = attachPoint.rotation;
			}
		}
	}

	public void grabObject(Transform other)
	{
		
		if(attachedObject != null)
		{
			releaseObject();
		}
		attachedObject = other;
		if (attachExact)
		{
			attachPoint.position = other.position;
			attachPoint.rotation = other.rotation;
		}else
		{
			attachPoint.position = this.transform.position;
			attachPoint.rotation = other.rotation;
		}
		Rigidbody rb = other.GetComponent<Collider>().attachedRigidbody;
		if (rb != null)
		{
			
			attachedKinematicSave = rb.isKinematic;
			attachedMaxAngularVelSave = rb.maxAngularVelocity;
			attachedUseGravitySave = rb.useGravity;
			rb.maxAngularVelocity = Mathf.Infinity;

			
		}
		attachedParentSave = other.parent;
	}

	public void releaseObject()
	{
		attachedObject.SetParent(attachedParentSave);
		if(attachedObject != null)
		{
			Rigidbody rb = attachedObject.GetComponent<Collider>().attachedRigidbody;
			if (rb != null)
			{
				rb.isKinematic = attachedKinematicSave;
				//rb.maxAngularVelocity = attachedMaxAngularVelSave;
				
				rb.velocity = controllerVelocity;
				Vector3 between = attachedObject.position - transform.position;
				rb.angularVelocity = controllerAngularVelocity;
				rb.velocity = rb.GetRelativePointVelocity(between);
			}
		}
		attachedObject = null;
	}

	
	private void OnTriggerStay(Collider other)
	{
		if (other.attachedRigidbody != null)
		{
			lastIntersection = other;
		}
	}
	private void OnTriggerExit()
	{
		lastIntersection = null;
	}
	
	public void squeeze(float ratio)
	{

		
		if(ratio > squeezeThreshold)
		{

			if(attachedObject==null && lastIntersection != null)
			{
				grabObject(lastIntersection.transform);
			}

			if (attachedObject == null && lastIntersection == null)
			{
				Collider[] colliders = this.GetComponentsInChildren<Collider>();
				foreach (Collider c in colliders)
				{
					c.isTrigger = false;
				}
			}

		}
		else
		{
			if(ratio <= squeezeThreshold / 2.0f)
			{
				if(attachedObject != null)
				{
					releaseObject();
				}
				
				Collider[] colliders = this.GetComponentsInChildren<Collider>();
				foreach (Collider c in colliders)
				{
					c.isTrigger = true;
				}
				
			}


		}
	}


}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VR;
/* 
[System.Serializable]
public class AxleInfo {
	public WheelCollider leftWheel;
	public WheelCollider rightWheel;
	public bool motor;
	public bool steering;
}


public class roverScript : MonoBehaviour {
	public SteamVR_TrackedObject controllerLeft;
	public SteamVR_TrackedObject controllerRight;
	public Rigidbody rover;
	public HandController handLeft;
	public HandController handRight;
	public Transform cam;
private void handleControllerInputs()
	{
		int indexLeft = (int)controllerLeft.index;
		int indexRight = (int)controllerRight.index;

		//handLeft.controllerVelocity = getControllerVelocity(controllerLeft);
		//handRight.controllerVelocity = getControllerVelocity(controllerRight);
		//handLeft.controllerAngularVelocity = getControllerAngularVelocity(controllerLeft);
		//handRight.controllerAngularVelocity = getControllerAngularVelocity(controllerRight);
		
		//float triggerLeft = getTrigger(controllerLeft);
		//float triggerRight = getTrigger(controllerRight);

		Vector2 joyLeft = getJoystick(controllerLeft);
		Vector2 joyRight = getJoystick(controllerRight);
		//handLeft.squeeze(triggerLeft);
		//handRight.squeeze(triggerRight);
		}
	public List<AxleInfo> axleInfos;
	public float maxMotorTorque;
	public float maxSteeringAngle;

	public void FixedUpdate () {
		float motor = maxMotorTorque * Input.GetAxis("Vertical");//change this to left analogue stick
		float steering = maxSteeringAngle * Input.GetAxis("Horizontal");	

		foreach (AxleInfo axleInfo in axleInfos){
			if(axleInfo.steering){
				axleInfo.leftWheel.steerAngle = steering;
				axleInfo.rightWheel.steerAngle = steering;
			}
			if(axleInfo.motor){
				axleInfo.leftWheel.motorTorque = motor;
				axleInfo.rightWheel.motorTorque = motor;
			}
		}
	}
	


	private Vector2 getJoystick(SteamVR_TrackedObject controller)
	{
		Debug.Log("controller index = " + controller.index);
		return controller.index >= 0 ? SteamVR_Controller.Input((int)controller.index).GetAxis(Valve.VR.EVRButtonId.k_EButton_SteamVR_Touchpad) : Vector2.zero;
	}

	public void FixedUpdate(){
		float motor = Input.GetAxis("Vertical");
		float steer = Input.GetAxis("Horizontal");
		cam.position = rover.position + new Vector3(0, 0, -1);
		//float motor = getJoystick(controllerLeft).y;
		//float steer = getJoystick(controllerRight).x;
		Vector3 displacement = (motor*Vector3.forward) * Time.deltaTime;
		rover.transform.Translate(displacement, Space.World);
		rover.transform.Rotate(new Vector3(0, 0, 0));
		

	}
}
*/

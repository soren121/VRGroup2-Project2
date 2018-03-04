using UnityEngine;
using System.Collections;
using System.Collections.Generic;


[System.Serializable]
public class AxleInfo {
	public WheelCollider leftWheel;
	public WheelCollider rightWheel;
	public bool motor;
	public bool steering;

}

public class CarScript : MonoBehaviour {
	public List<AxleInfo> axleInfos; 
	public float maxMotorTorque;
	public float maxSteeringAngle;
	public Transform cam;
	public Transform rover;
	public HandController lefthand;// = GameObject.Find("Hand_Left");
	public SteamVR_TrackedObject lh;
	public HandController righthand;// = GameObject.Find("Hand_Right");
	public SteamVR_TrackedObject rh;

	// finds the corresponding visual wheel
	// correctly applies the transform
	private Vector2 getJoystick(SteamVR_TrackedObject controller)
	{
		return controller.index >= 0 ? SteamVR_Controller.Input((int)controller.index).GetAxis(Valve.VR.EVRButtonId.k_EButton_SteamVR_Touchpad) : Vector2.zero;
	}

	public void ApplyLocalPositionToVisuals(WheelCollider collider)
	{
		if (collider.transform.childCount == 0) {
			return;
		}

		Transform visualWheel = collider.transform.GetChild(0);

		Vector3 position;
		Quaternion rotation;
		collider.GetWorldPose(out position, out rotation);

		visualWheel.transform.position = position;
		visualWheel.transform.rotation = rotation;
	}

	public void FixedUpdate()
	{
        cam.position = rover.position;// + 3*Vector3.down + Vector3.back;
		Debug.Log(Input.GetJoystickNames());
		float motor = maxMotorTorque * Input.GetAxis("Vertical");
		float steering = maxSteeringAngle * Input.GetAxis("Horizontal");
		//float motor = maxMotorTorque * getJoystick(lh).y;
		//float steering = maxSteeringAngle * getJoystick(rh).x;

		foreach (AxleInfo axleInfo in axleInfos) {
			if (axleInfo.steering) {
				axleInfo.leftWheel.steerAngle = steering;
				axleInfo.rightWheel.steerAngle = steering;
			}
			if (axleInfo.motor) {
				axleInfo.leftWheel.motorTorque = motor;
				axleInfo.rightWheel.motorTorque = motor;
			}
			ApplyLocalPositionToVisuals(axleInfo.leftWheel);
			ApplyLocalPositionToVisuals(axleInfo.rightWheel);
		}
	}
}
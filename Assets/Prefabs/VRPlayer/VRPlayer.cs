using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VR;
using UnityEngine.Networking;




public class VRPlayer : NetworkBehaviour {
	//public Blinker hmdBlinker;

	public Transform SteamVR_Rig;
	public SteamVR_TrackedObject hmd;
	public SteamVR_TrackedObject controllerLeft;
	public SteamVR_TrackedObject controllerRight;
	public Transform head;
	//public Transform cam;
	public HandController handLeft;
	public HandController handRight;
	public Transform feet;
	public enum LocomotionMode { TELEPORT, JOYSTICK_DRIVE, JOYSTICK_VEHICLE_DRIVE, FLYING, HUMAN_JOYSTICK, WALKING_IN_PLACE};
	public LocomotionMode locomotionMode = LocomotionMode.JOYSTICK_DRIVE;
	public Vector3 lastHipPosition;
	public bool isWalking;
	public Transform rover;
	public GameObject littleRover;
	public GameObject bigCenter;
	public GameObject littleCenter;
    
	public float thrust;
	// Use this for initialization
	[SyncVar]
	Vector3 headPos;
	[SyncVar]
	Quaternion headRot;
	[SyncVar]
	Vector3 leftHandPos;
	[SyncVar]
	Quaternion leftHandRot;
	[SyncVar]
	Vector3 rightHandPos;
	[SyncVar]
	Quaternion rightHandRot;

	void Start () {
		head.transform.position = new Vector3(0, 4, 0);
		rover = GameObject.Find("BigRover").transform;
		rover.gameObject.SetActive (true);
		littleRover = GameObject.Find ("littleRover");
		bigCenter = bigCenter.transform.GetChild (0).gameObject;
		littleCenter = littleCenter.transform.GetChild (0).gameObject;
	}
	
	void Update () {
	
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
	private void FixedUpdate()
	{
		//cam.position = rover.position;
		//Debug.Log(Input.GetJoystickNames());
		//float motor = maxMotorTorque * Input.GetAxis("Vertical");
		//float steering = maxSteeringAngle * Input.GetAxis("Horizontal");

		if (isServer) {
			RpcSyncLittleRover (rover.position, rover.rotation);
		}

		if (isLocalPlayer)
		{
			if (UnityEngine.XR.XRSettings.enabled)
			{
				if (SteamVR_Rig == null)
				{
					GameManager gm = GameObject.Find("GameManager").GetComponent<GameManager>();
					SteamVR_Rig = gm.vrCameraRig.transform;
					hmd = gm.hmd;
					controllerLeft = gm.controllerLeft;
					controllerRight = gm.controllerRight;
					//hmdBlinker = gm.hmdBlinker;
				}
				//the controllers are the easy ones, just move them directly
				copyTransform(controllerLeft.transform, handLeft.transform);
				copyTransform(controllerRight.transform, handRight.transform);
				//now move the head to the HMD position, this is actually the eye position
				copyTransform(hmd.transform, head);
				//move the feet to be in the tracking space, but on the ground (maybe do this with physics to ensure a good foot position later)
				feet.position = Vector3.Scale(head.position, new Vector3(1, 0, 1)) + Vector3.Scale(SteamVR_Rig.position, new Vector3(0, 1, 0));
				handleControllerInputs();

			}
			else
			{
				float vertical = Input.GetAxis("Vertical");
				float horizontal = Input.GetAxis("Horizontal");
				transform.Translate(vertical * Time.fixedDeltaTime * (new Vector3(0, 0, 1)));
				transform.Translate(horizontal * Time.fixedDeltaTime * (new Vector3(1, 0, 0)));
			}

			CmdSyncPlayer(head.transform.position,head.transform.rotation, handLeft.transform.position, handLeft.transform.rotation, handRight.transform.position, handRight.transform.rotation);
		} else {
			//runs on all other clients and  the server
			//move to the syncvars
			head.position = Vector3.Lerp(head.position, headPos,.2f);
			head.rotation = Quaternion.Slerp(head.rotation, headRot, .2f);
			handLeft.transform.position = leftHandPos;
			handLeft.transform.rotation = leftHandRot;
			handRight.transform.position = rightHandPos;
			handRight.transform.rotation = rightHandRot;
		}

	}

	[Command]
	void CmdSyncPlayer(Vector3 pos, Quaternion rot, Vector3 lhpos, Quaternion lhrot, Vector3 rhpos, Quaternion rhrot)
	{
		head.transform.position = pos;
		head.transform.rotation = rot;
		handLeft.transform.position = lhpos;
		handRight.transform.position = rhpos;
		handLeft.transform.rotation = lhrot;
		handRight.transform.rotation = rhrot;
		headPos = pos;
		headRot = rot;
		leftHandPos = lhpos;
		leftHandRot = lhrot;
		rightHandPos = rhpos;
		rightHandRot = rhrot;
	}

	[ClientRpc]
	void RpcSyncLittleRover(Vector3 pos, Quaternion rot)
	{
		Vector3 differencePos = pos - bigCenter.transform.position;
		differencePos = differencePos * .1f;
		littleRover.transform.rotation = rot;
		differencePos = differencePos * .1f;
		littleRover.transform.position = littleCenter.transform.position + differencePos;
	}

	private void copyTransform(Transform from, Transform to)
	{
		to.position = from.position;
		to.rotation = from.rotation;
	}
	private void handleControllerInputs()
	{
		int indexLeft = (int)controllerLeft.index;
		int indexRight = (int)controllerRight.index;

		handLeft.controllerVelocity = getControllerVelocity(controllerLeft);
		handRight.controllerVelocity = getControllerVelocity(controllerRight);
		handLeft.controllerAngularVelocity = getControllerAngularVelocity(controllerLeft);
		handRight.controllerAngularVelocity = getControllerAngularVelocity(controllerRight);
		
		float triggerLeft = getTrigger(controllerLeft);
		float triggerRight = getTrigger(controllerRight);

		Vector2 joyLeft = getJoystick(controllerLeft);
		Vector2 joyRight = getJoystick(controllerRight);
		handLeft.squeeze(triggerLeft);
		handRight.squeeze(triggerRight);

		switch (locomotionMode)
		{
			case LocomotionMode.JOYSTICK_DRIVE:
				{
					drive(joyLeft, joyRight);
					break;
				}
			case LocomotionMode.JOYSTICK_VEHICLE_DRIVE:
				{
					vehicleDrive(joyLeft, joyRight);
					break;
				}
			case LocomotionMode.FLYING:
				{
					fly(joyLeft, joyRight);
					break;
				}
			case LocomotionMode.HUMAN_JOYSTICK:
				{
					Vector3 footDispacement = (feet.position - SteamVR_Rig.position);
					if (footDispacement.magnitude > .25f) {
						vehicleDrive(new Vector2(footDispacement.x,footDispacement.z), Vector2.zero);
					}
					break;
				}
			case LocomotionMode.WALKING_IN_PLACE:
				{
					walkInPlace(joyLeft, joyRight);
					break;
				}
			case LocomotionMode.TELEPORT:
				{
					handLeft.joystick(joyLeft);
					handRight.joystick(joyRight);
					break;
				}
		}
		

	}
	private float getTrigger(SteamVR_TrackedObject controller)
	{
		return controller.index >= 0 ? SteamVR_Controller.Input((int)controller.index).GetAxis(Valve.VR.EVRButtonId.k_EButton_SteamVR_Trigger).magnitude : 0.0f;
	}

	private Vector2 getJoystick(SteamVR_TrackedObject controller)
	{
		return controller.index >= 0 ? SteamVR_Controller.Input((int)controller.index).GetAxis(Valve.VR.EVRButtonId.k_EButton_SteamVR_Touchpad) : Vector2.zero;
	}

	private Vector3 getControllerVelocity(SteamVR_TrackedObject controller)
	{
		Vector3 controllerVelocity = controller.index >= 0 ? SteamVR_Controller.Input((int)controller.index).velocity : Vector3.zero;
		return SteamVR_Rig.localToWorldMatrix.MultiplyVector(controllerVelocity.normalized)*controllerVelocity.magnitude;
	}

	private Vector3 getControllerAngularVelocity(SteamVR_TrackedObject controller)
	{
		Vector3 angularVelocity = controller.index >= 0 ? SteamVR_Controller.Input((int)controller.index).angularVelocity : Vector3.zero;
		return SteamVR_Rig.localToWorldMatrix.MultiplyVector(angularVelocity.normalized) * angularVelocity.magnitude ;
	}

	public void teleport(Vector3 pos, Vector3 forward)
	{
		//hmdBlinker.blink(.1f);
		Vector3 offset = pos-feet.position;
		SteamVR_Rig.Translate(offset, Space.World);
		Vector3 facingDirection = new Vector3(head.forward.x,0,head.forward.z);
		float angleBetween = Vector3.SignedAngle(facingDirection, forward, Vector3.up);
		SteamVR_Rig.Rotate(Vector3.up, angleBetween, Space.World);
	}
	//called within an update method
	public void drive(Vector2 leftJoystick, Vector2 rightJoystick)
	{
		//Debug.Log("Driving!");
		float motor = Input.GetAxis("Vertical");
		float steer = Input.GetAxis("Horizontal");
		//float motor = leftJoystick.y;
		//float steer = rightJoystick.x;
		//Debug.Log("motor = " + motor);
		//Debug.Log("steer = " + steer);

		
		Vector3 displacement = (motor*Vector3.forward) * Time.deltaTime;
		rover.transform.Translate(displacement, Space.World);
		rover.transform.Rotate(new Vector3(0, steer, 0));
		
		//we'll use the gaze in the x-z plane as the facing direction
		/*Vector3 facingDirection = new Vector3(head.forward.x, 0, head.forward.z);
		Vector3 rightDirection = new Vector3(head.right.x, 0, head.right.z);
		Vector3 displacement = (facingDirection * leftJoystick.y + rightDirection*leftJoystick.x) * Time.deltaTime;
		SteamVR_Rig.transform.Translate(displacement, Space.World);
		float angleDisplacement = 90*rightJoystick.x * Time.deltaTime;
		SteamVR_Rig.transform.Rotate(0, angleDisplacement, 0, Space.World);
*/
	}

	//called within an update method
	public void vehicleDrive(Vector2 leftJoystick, Vector2 rightJoystick)
	{
		//Debug.Log("driving vehicle!");
		Vector3 facingDirection = new Vector3(head.forward.x, 0, head.forward.z);
		Vector3 rightDirection = new Vector3(head.right.x, 0, head.right.z);
		Vector3 displacement = (facingDirection * leftJoystick.y + rightDirection*leftJoystick.x) * Time.deltaTime;
		//Debug.Log(displacement);
		//rover.AddRelativeForce(displacement * 10, ForceMode.Acceleration);

	}

	public void fly(Vector2 leftJoystick, Vector2 rightJoystick)
	{
		float leftSpeed = Mathf.Clamp(leftJoystick.y,0,1);
		float rightSpeed = Mathf.Clamp(rightJoystick.y, 0, 1);
		Vector3 leftDirection = handLeft.transform.forward;
		Vector3 rightDirection = handRight.transform.forward;
		Vector3 displacement = (leftDirection * leftSpeed + rightDirection * rightSpeed) * Time.deltaTime;
		SteamVR_Rig.transform.Translate(displacement, Space.World);
	}

	public void walkInPlace(Vector2 leftJoystick, Vector2 rightJoystick)
	{
		Vector3 rightVector = handRight.transform.position - handLeft.transform.position;
		rightVector = new Vector3(rightVector.x, 0, rightVector.z);
		Vector3 facingDirection = Vector3.Cross(rightVector.normalized, Vector3.up);
		if(Mathf.Abs(leftJoystick.y) > 0.1f && Mathf.Abs(rightJoystick.y) > 0.1f )
		{
			Vector3 hipPosition = (handLeft.transform.position + handRight.transform.position) / 2.0f;
			if (!isWalking)
			{
				isWalking = true;
				lastHipPosition = hipPosition;
			}
			else
			{
				//figure out the y displacement of the central point
				float yDisplacement = Mathf.Abs(((handLeft.transform.position + handRight.transform.position) / 2).y - lastHipPosition.y);
				//go forward by that y displacement
				SteamVR_Rig.Translate(yDisplacement * facingDirection, Space.World);
			}
		}
		else
		{
			isWalking = false;
		}

	}



}

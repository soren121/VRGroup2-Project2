using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class LittleTerrainScript : NetworkBehaviour
{
    public GameObject BigTerrain;
    public GameObject BigCenter;
    public GameObject LittleTerrain;
    public GameObject LittleCenter;

    private VRPlayer localPlayer;

    private void OnCollisionEnter(Collision collision)
    {
        StartCoroutine(HandleCollision(collision));   
    }

    private VRPlayer GetPlayer() {
        if (localPlayer == null) {
            VRPlayer[] players = FindObjectsOfType<VRPlayer>() as VRPlayer[];
            foreach (var player in players) {
                if (player.GetComponent<NetworkIdentity>().isLocalPlayer) {
                    localPlayer = player;
                    break;
                }
            }
        }

        return localPlayer;
    }

    // Determines what hit the floor, and then performs the appropriate action
    private IEnumerator HandleCollision(Collision collision)
    {
		if (collision.gameObject.GetComponent<TaskCube>() != null && 
            collision.rigidbody.isKinematic == false) {
            //Make the TaskObject so that it won't bounce everywhere if you drop it on the terrain
            collision.rigidbody.isKinematic = true;

            //get the TaskObject position
            Vector3 otherPos = collision.transform.position;

            //get the difference from the position of the taskObject and the little terrain center
            Vector3 differencePos = collision.transform.position - LittleCenter.transform.position;

            //scale the coordinates so they will be in the correct position on the big terrain
            differencePos.x += differencePos.x * 100;
            differencePos.y += differencePos.y * 100;
            differencePos.z += differencePos.z * 100;
            //differencePos = differencePos * 100f;
            Vector3 newPos = BigCenter.transform.position + differencePos;
            collision.rigidbody.useGravity = false;

            GetPlayer().CmdInstantiateTaskObjects(newPos);
        }

        yield return null;
    }
}

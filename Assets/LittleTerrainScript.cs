using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LittleTerrainScript : MonoBehaviour
{
    public GameObject cube1;
    public GameObject BigTerrain;
    public GameObject BigCenter;
    public GameObject LittleTerrain;
    public GameObject LittleCenter;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log(BigTerrain.transform.name + " has position position " + BigTerrain.transform.position);
        Debug.Log(BigCenter.transform.name + " has position position " + BigCenter.transform.position);
        Debug.Log(LittleTerrain.transform.name + " has position position " + LittleTerrain.transform.position);
        Debug.Log(LittleCenter.transform.name + " has position position " + LittleCenter.transform.position);


        Vector3 otherPos = collision.transform.position;
        Debug.Log(collision.transform.name + "has position position " + collision.transform.name);
        Vector3 differencePos = collision.transform.position - LittleCenter.transform.position;
        differencePos.x += differencePos.x * 100;
        differencePos.z += differencePos.z * 100;
        Vector3 newPos = BigCenter.transform.position + differencePos;

        //Debug.Log(collision.transform.name + " has position on " + otherTerrain + "terrain" + otherPos);
        //float newy = 100 + otherPos.y;
        //float newx = 10 * otherPos.x;
        //float newz = 10 * otherPos.z;
        //Vector3 newV = new Vector3(newx, newy, newz);
        //Debug.Log(collision.transform.name + " has position on big terrain" + newV);

        GameObject newcube1 = GameObject.Instantiate(cube1);
        newcube1.transform.localScale += new Vector3(10, 10, 10);
        newcube1.transform.position += newPos;
    }
}

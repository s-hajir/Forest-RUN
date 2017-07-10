using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamFollow : MonoBehaviour {

    public GameObject player;
    Vector3 camPos;
	void Start () {
		
	}
	
	void Update () {
        camPos = transform.position;
        transform.position = new Vector3(camPos.x, camPos.y, player.transform.position.z - 8);
	}
}

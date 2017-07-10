using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKey(KeyCode.W))
        {
            transform.position = Vector3.Lerp(transform.position, new Vector3(transform.position.x, transform.position.y, transform.position.z + 50), Time.deltaTime); 
        }
        else if (Input.GetKey(KeyCode.Space))
        {
            transform.position = Vector3.Lerp(transform.position, new Vector3(transform.position.x, transform.position.y+20, transform.position.z + 50), Time.deltaTime);
        }
	}
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColliderScript : MonoBehaviour {

    public Player playerScript;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Collider[] colliders = GetComponents<Collider>();
            foreach (Collider coll in colliders)
            {
                coll.enabled = false;
            }
            playerScript.playerCollision(gameObject);
        }
    }
}

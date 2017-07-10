using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerWallGenNextMap : MonoBehaviour {

    public MapGenerator mapGenerator;
    bool isTriggered = false;

    private void OnTriggerEnter(Collider other)
    {
        if (!isTriggered)
        {
            if (other.gameObject.CompareTag("Player"))
            {
                isTriggered = true;
                mapGenerator.generateNextMap();
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        isTriggered = false;
    }
}

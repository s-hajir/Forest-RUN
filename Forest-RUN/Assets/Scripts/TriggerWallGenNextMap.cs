using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerWallGenNextMap : MonoBehaviour {

    public MapGenerator mapGenerator;

    private void OnTriggerEnter(Collider other)
    {
            if (other.gameObject.CompareTag("Player"))
            {
                mapGenerator.generateNextMap();
                gameObject.SetActive(false);
            }
    }
}

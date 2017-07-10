using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//KONZEPT:  Queue ist Haupt Map Loop. Default rotieren wir die Queue per Hand weiter (setzen Maps hintereinander). 
//          Aber wir können durch Abfragen die Queue in Ruhe lassen und dynamische Maps zwischendruch aus Array(mapParts) nehmen und setzen. 
public class MapGenerator : MonoBehaviour {

    public GameObject[] mapParts;           //alles maps ausser Beginpart
    public GameObject beginPart;

    private Queue<GameObject> mapQueue = new Queue<GameObject>();  //Queue Head -> nextMap   //Queue Tail -> currentMap
    private GameObject currentMap;    

    private void Start()
    {
        int length = mapParts.Length;
        currentMap = beginPart;            //on start our currentMap is beginPart -> will be overriden at first generateNextMap() call 

        for (int i = 0; i < length; i++)   //fill Queue with array
        {
            mapQueue.Enqueue(mapParts[i]); 
        }
    }

    public void generateNextMap()
    {
        GameObject nextMap = mapQueue.Dequeue();  //1.get Queue head. Head is always nextMap
        Vector3 currentMapPos = currentMap.transform.position;

        //**DirtyFix
        int tmp = 0;
        if (nextMap.CompareTag("Cave"))
        {
            tmp = 90;   //näher an currentMap
        }
        else if (currentMap.CompareTag("Cave"))
        {
            tmp = -79;  //weiter weg von currentMap
        }
        else
        {
            tmp = 0;
        }
        //**DirtyFix End

        nextMap.transform.position = new Vector3(0, 0, currentMapPos.z + currentMap.GetComponent<Collider>().bounds.size.z-tmp);
        Debug.Log("currentmap: '"+currentMap.name+"' just build nextmap: '"+nextMap.name+"'");

        mapQueue.Enqueue(nextMap); //2.Enqueue nextMap into Queue -> is now Tail
        currentMap = nextMap;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//KONZEPT:  Queue ist Haupt Map Loop. Default rotieren wir die Queue 'per Hand' weiter (setzen Maps hintereinander). 
//          Aber wir können durch Abfragen die Queue in Ruhe lassen und dynamische Maps zwischendruch (aus Array 'mapParts' nehmen und) setzen. 
public class MapGenerator : MonoBehaviour {

    public GameObject[] mapParts;           //alle maps ausser Beginpart
    public GameObject beginPart;
    public GameObject player;
    public ObstacleGenerator obsGenerator;
    public GameObject obsLanesParent;
    public GameObject obsLanesParentClone;

    private Queue<GameObject> mapQueue = new Queue<GameObject>();  //Queue Head -> nextMap   //Queue Tail -> currentMap
    private GameObject currentMap;
    private GameObject currentMapRef;
    private GameObject nextMap;
    private Vector3 currentMapPos;
    private Vector3 nextMapPos;
    private Vector3 cameraPos;
    private Vector3 playerPos;

    private void Start()
    {
        currentMap = beginPart;            //on start our currentMap is beginPart -> will be overriden at first generateNextMap() call 
        int length = mapParts.Length;
        for (int i = 0; i < length; i++)   //fill Queue with array
        {
            mapQueue.Enqueue(mapParts[i]); 
        }
        obsGenerator.buildObstacleLanes(beginPart);
    }

    public void generateNextMap()
    {
        nextMap = mapQueue.Dequeue();  //1.get Queue head. Head is always nextMap

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

        currentMapPos = currentMap.transform.position;
        currentMapRef = currentMap;  //referenz speichern, da unten überschrieben wird
        nextMap.transform.position = new Vector3(0, 0, currentMapPos.z + currentMap.GetComponent<Collider>().bounds.size.z-tmp);

        obsGenerator.buildObstacleLanes(nextMap);//Build ObsLanes for nextMap

        nextMap.transform.GetChild(0).gameObject.SetActive(true); //TriggerWall an
        nextMapPos = nextMap.transform.position;
        Debug.Log("currentmap: '"+currentMap.name+"' just build nextmap: '"+nextMap.name+"'");

        centerEverything();  //Unity Problematik Fix 

        mapQueue.Enqueue(nextMap); //2.Enqueue nextMap into Queue -> is now Tail
        currentMap = nextMap;
    }

    void centerEverything() 
    {                           //Problematik : Unitys Transform Position X,Y,Z ist auf 7 'significant digits' limitiert
                                //je weiter vom 'origin' 0,0,0 entfernt -> desto mehr floating-point Präzision verliert man
                                //Lösung: alles nah am origin halten ab 10000 alles um 9000 zurückschieben
        playerPos = player.transform.position;
        cameraPos = gameObject.transform.position;
                 
        if (playerPos.z > 10000)                      
        {
            Debug.Log("Old player z :"+playerPos.z);
            Debug.Log("Old currMap, nextMap: '"+currentMapPos.z+"'  '"+nextMapPos.z+"'");
            currentMapRef.transform.position = new Vector3(currentMapPos.x,currentMapPos.y,currentMapPos.z - 9000);  //hier 'currentMap' variable nicht benutzt, da es überschr. wird
            nextMap.transform.position = new Vector3(nextMapPos.x, nextMapPos.y, nextMapPos.z - 9000);
            gameObject.transform.position = new Vector3(cameraPos.x, cameraPos.y, cameraPos.z - 9000);
            player.transform.position = new Vector3(playerPos.x, playerPos.y, playerPos.z - 9000);

            //alle Elemente aus obsLanesParent und obsLanesParentClone auch um 9000 verschieben
            foreach (Transform child in obsLanesParent.transform)
            {
                Vector3 currentPos = child.position;
                child.position = new Vector3(currentPos.x, currentPos.y, currentPos.z - 9000);
            }
            foreach (Transform child in obsLanesParentClone.transform)
            {
                Vector3 currentPos = child.position;
                child.position = new Vector3(currentPos.x, currentPos.y, currentPos.z - 9000);
            }

            Debug.Log("New Player z: "+player.transform.position.z);
            Debug.Log("New currMap, nextMap: '"+currentMap.transform.position.z+"'  '"+nextMap.transform.position.z+"'");
        }
    }
}

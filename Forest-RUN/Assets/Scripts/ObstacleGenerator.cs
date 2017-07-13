using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleGenerator : MonoBehaviour {

    public GameObject[] obsBearArray = new GameObject[4];  //4x bear

    public GameObject obsZero;  //stamm 4
    public GameObject obsOne;   //slide
    public GameObject obsTwo;   //stamm2
    public GameObject obsThree; //stein
    public GameObject obsFour;  //holzhaufen

    public GameObject unusedObstacles;
    public GameObject obsLanesParent;
    public GameObject obsLanesParentClone;

    private bool useClones = false; //false -> use 'obsZeroArray', 'obsOneArray' etc. to buil Obstacle Lane. true-> use 'obsZeroArrayClone', 'obsOneArrayClone' ...

    private int mapObstacleOffset = 0;
    public GameObject[] obsZeroArray = new GameObject[12];  //12x stamm 4
    private GameObject[] obsOneArray = new GameObject[12];   //12x slide
    private GameObject[] obsTwoArray = new GameObject[12];   //12x stamm2
    private GameObject[] obsThreeArray = new GameObject[12]; //12x stein
    private GameObject[] obsFourArray = new GameObject[12];  //12x holzhaufen
    private int obsZeroArray_index = 0;
    private int obsOneArray_index = 0;
    private int obsTwoArray_index = 0;
    private int obsThreeArray_index = 0;
    private int obsFourArray_index = 0;

    private GameObject[] obsZeroArrayClone = new GameObject[12];  //12x stamm 4
    private GameObject[] obsOneArrayClone = new GameObject[12];   //12x slide
    private GameObject[] obsTwoArrayClone = new GameObject[12];   //12x stamm2
    private GameObject[] obsThreeArrayClone = new GameObject[12]; //12x stein
    private GameObject[] obsFourArrayClone = new GameObject[12];  //12x holzhaufen
    private int obsZeroArrayClone_index = 0;
    private int obsOneArrayClone_index = 0;
    private int obsTwoArrayClone_index = 0;
    private int obsThreeArrayClone_index = 0;
    private int obsFourArrayClone_index = 0;

    float[] posX = { -5.94f, -2.17f, 1.42f, 5.23f}; //Obstacle Positionen auf einer Lane  -5.94, -2.17, 1.42, 5.23


    private void Awake()   //MapGenerator Start() ruft buildObstacleLanes() auf -> vorher alle obs initialisieren
    {
        for (int i = 0; i < 12; i++)  //init all obstacle arrays
        {
            (obsZeroArray[i] = (GameObject) Instantiate(obsZero)).transform.position = new Vector3(0,-100,0);
            obsZeroArray[i].transform.SetParent(unusedObstacles.transform);
            obsZeroArray[i].name = "obsZero"+i;

            (obsZeroArrayClone[i] = (GameObject)Instantiate(obsZero)).transform.position = new Vector3(0, -100, 0);
            obsZeroArrayClone[i].transform.SetParent(unusedObstacles.transform);
            obsZeroArrayClone[i].name = "obsZeroClone" + i;

            (obsOneArray[i] = (GameObject) Instantiate(obsOne)).transform.position = new Vector3(0, -100, 0);
            obsOneArray[i].transform.SetParent(unusedObstacles.transform);
            obsOneArray[i].name = "obsOne" + i;

            (obsOneArrayClone[i] = (GameObject)Instantiate(obsOne)).transform.position = new Vector3(0, -100, 0);
            obsOneArrayClone[i].transform.SetParent(unusedObstacles.transform);
            obsOneArrayClone[i].name = "obsOneClone" + i;

            (obsTwoArray[i] = (GameObject) Instantiate(obsTwo)).transform.position = new Vector3(0, -100, 0);
            obsTwoArray[i].transform.SetParent(unusedObstacles.transform);
            obsTwoArray[i].name = "obsTwo" + i;

            (obsTwoArrayClone[i] = (GameObject)Instantiate(obsTwo)).transform.position = new Vector3(0, -100, 0);
            obsTwoArrayClone[i].transform.SetParent(unusedObstacles.transform);
            obsTwoArrayClone[i].name = "obsTwoClone" + i;

            (obsThreeArray[i] = (GameObject)Instantiate(obsThree)).transform.position = new Vector3(0, -100, 0);
            obsThreeArray[i].transform.SetParent(unusedObstacles.transform);
            obsThreeArray[i].name = "obsThree" + i;

            (obsThreeArrayClone[i] = (GameObject)Instantiate(obsThree)).transform.position = new Vector3(0, -100, 0);
            obsThreeArrayClone[i].transform.SetParent(unusedObstacles.transform);
            obsThreeArrayClone[i].name = "obsThreeClone" + i;

            (obsFourArray[i] = (GameObject)Instantiate(obsFour)).transform.position = new Vector3(0, -100, 0);
            obsFourArray[i].transform.SetParent(unusedObstacles.transform);
            obsFourArray[i].name = "obsFour" + i;

            (obsFourArrayClone[i] = (GameObject)Instantiate(obsFour)).transform.position = new Vector3(0, -100, 0);
            obsFourArrayClone[i].transform.SetParent(unusedObstacles.transform);
            obsFourArrayClone[i].name = "obsFourClone" + i;
        }
    }

    public void buildObstacleLanes(GameObject mapPart) 
    {
        if (mapPart.CompareTag("Cave"))
        {
            //generate bears in the Cave
            generateBears(mapPart);
        }
        else
        {
            //Debug.Log("generate Lanes useClones: " + useClones);
            if (useClones)
            {
                Debug.Log("build lane CLONE at nextMap");
                unparentChildren(obsLanesParentClone);
                for (int i = 0; i < 3; i++)  //build 5 obsLanes at the correct positions
                {
                    buildSingleObsLaneWithC(mapPart, i);  //obstacles will be parented to 'obsLanesParentClone'
                }
                useClones = false;
            }
            else
            {
                Debug.Log("build lane at nextMap");
                unparentChildren(obsLanesParent);
                for (int i = 0; i < 3; i++)
                {
                    buildSingleObsLane(mapPart, i);       //obstacles will be parented to 'obsLanesParent'
                }
                useClones = true;
            }
        }
    }

    public void buildSingleObsLane(GameObject mappart, int a)
    {
        //GameObject lane = new GameObject("Lane"+a);
        //lane.transform.SetParent(obsLanesParent.transform);
        float mapPosZ = mappart.transform.position.z;

        for (int i = 0; i < 4; i++)   //4 Iterationen -> 4 Obstacles zufällig auswählen -> Lane bilden 
        {
            int randomObstacle = Random.Range(0, 5); // 0-4 (5 exclusive)
            switch (randomObstacle)
            {
                case 0:
                    //aus obsZeroArray (nacheinander) entnehmen -> parenten
                    obsZeroArray[obsZeroArray_index].transform.SetParent(obsLanesParent.transform);
                    obsZeroArray[obsZeroArray_index].transform.position = new Vector3(posX[i], 0, (mapPosZ-100) + (100 * a));  //Lane Abstand 10 (immer Vielfache)
                    if (obsZeroArray_index < 11)
                    {
                        obsZeroArray_index++;
                    }else
                    {
                        obsZeroArray_index = 0;
                    }
                    break;
                case 1:
                    //aus obsOneArray entnehmen
                    obsOneArray[obsOneArray_index].transform.SetParent(obsLanesParent.transform);
                    obsOneArray[obsOneArray_index].transform.position = new Vector3(posX[i], 0, (mapPosZ - 100) + (100 * a)); 
                    if (obsOneArray_index < 11)
                    {
                        obsOneArray_index++;
                    }
                    else
                    {
                        obsOneArray_index = 0;
                    }
                    break;
                case 2:
                    //aus obsTwoArray nehmen
                    obsTwoArray[obsTwoArray_index].transform.SetParent(obsLanesParent.transform);
                    obsTwoArray[obsTwoArray_index].transform.position = new Vector3(posX[i], 0, (mapPosZ - 100) + (100 * a));  //Lane Abstand 10 (immer Vielfache)
                    if (obsTwoArray_index < 11)
                    {
                        obsTwoArray_index++;
                    }
                    else
                    {
                        obsTwoArray_index = 0;
                    }
                    break;
                case 3:
                    //aus obsThreeArray nehmen
                    obsThreeArray[obsThreeArray_index].transform.SetParent(obsLanesParent.transform);
                    obsThreeArray[obsThreeArray_index].transform.position = new Vector3(posX[i], 0, (mapPosZ - 100) + (100 * a));  //Lane Abstand 10 (immer Vielfache)
                    if (obsThreeArray_index < 11)
                    {
                        obsThreeArray_index++;
                    }
                    else
                    {
                        obsThreeArray_index = 0;
                    }
                    break;
                case 4:
                    //aus obsFourArray nehmen
                    obsFourArray[obsFourArray_index].transform.SetParent(obsLanesParent.transform);
                    obsFourArray[obsFourArray_index].transform.position = new Vector3(posX[i], 0, (mapPosZ - 100) + (100 * a));  //Lane Abstand 10 (immer Vielfache)
                    if (obsFourArray_index < 11)
                    {
                        obsFourArray_index++;
                    }
                    else
                    {
                        obsFourArray_index = 0;
                    }
                    break;
                default:
                    break;
            }
        }
    }

    public void buildSingleObsLaneWithC(GameObject mappart, int a)
    {
        float mapPosZ = mappart.transform.position.z;
        //GameObject laneClone = new GameObject("laneClone"+i);

        for (int i = 0; i < 4; i++)   //4 Iterationen -> 4 Obstacles zufällig auswählen -> Lane bilden 
        {
            int randomObstale = Random.Range(0, 5); // 0-4 (5 exclusive)
            switch (randomObstale)
            {
                case 0:
                    //aus obsZeroArray (nacheinander) entnehmen -> parenten
                    obsZeroArrayClone[obsZeroArrayClone_index].transform.SetParent(obsLanesParentClone.transform);
                    obsZeroArrayClone[obsZeroArrayClone_index].transform.position = new Vector3(posX[i], 0, (mapPosZ - 100) + (100 * a));  //Lane Abstand 10 (immer Vielfache)
                    if (obsZeroArrayClone_index < 11)
                    {
                        obsZeroArrayClone_index++;
                    }
                    else
                    {
                        obsZeroArrayClone_index = 0;
                    }
                    break;
                case 1:
                    //aus obsOneArray entnehmen
                    obsOneArrayClone[obsOneArrayClone_index].transform.SetParent(obsLanesParentClone.transform);
                    obsOneArrayClone[obsOneArrayClone_index].transform.position = new Vector3(posX[i], 0, (mapPosZ - 100) + (100 * a));
                    if (obsOneArrayClone_index < 11)
                    {
                        obsOneArrayClone_index++;
                    }
                    else
                    {
                        obsOneArrayClone_index = 0;
                    }
                    break;
                case 2:
                    //aus obsTwoArray nehmen
                    obsTwoArrayClone[obsTwoArrayClone_index].transform.SetParent(obsLanesParentClone.transform);
                    obsTwoArrayClone[obsTwoArrayClone_index].transform.position = new Vector3(posX[i], 0, (mapPosZ - 100) + (100 * a));  //Lane Abstand 10 (immer Vielfache)
                    if (obsTwoArrayClone_index < 11)
                    {
                        obsTwoArrayClone_index++;
                    }
                    else
                    {
                        obsTwoArrayClone_index = 0;
                    }
                    break;
                case 3:
                    //aus obsThreeArray nehmen
                    obsThreeArrayClone[obsThreeArrayClone_index].transform.SetParent(obsLanesParentClone.transform);
                    obsThreeArrayClone[obsThreeArrayClone_index].transform.position = new Vector3(posX[i], 0, (mapPosZ - 100) + (100 * a));  //Lane Abstand 10 (immer Vielfache)
                    if (obsThreeArrayClone_index < 11)
                    {
                        obsThreeArrayClone_index++;
                    }
                    else
                    {
                        obsThreeArrayClone_index = 0;
                    }
                    break;
                case 4:
                    //aus obsFourArray nehmen
                    obsFourArrayClone[obsFourArrayClone_index].transform.SetParent(obsLanesParentClone.transform);
                    obsFourArrayClone[obsFourArrayClone_index].transform.position = new Vector3(posX[i], 0, (mapPosZ - 100) + (100 * a));  //Lane Abstand 10 (immer Vielfache)
                    if (obsFourArrayClone_index < 11)
                    {
                        obsFourArrayClone_index++;
                    }
                    else
                    {
                        obsFourArrayClone_index = 0;
                    }
                    break;
                default:
                    break;
            }
        }
    }

    void unparentChildren(GameObject gOToUnparentAllChildren)   //unparent all children from this gO -> parent its children to 'unusedObstacles'
    {
        int debugCounter = 0;
        
        Transform t = gOToUnparentAllChildren.transform;
        //Debug.Log("'" + gOToUnparentAllChildren.name + "' childCount: " + t.childCount);
        List<Transform> tChildList = new List<Transform>(t.childCount);
        foreach (Transform child in t)
        {
            tChildList.Add(child);
        }
        foreach (Transform child in tChildList)   //****Todo: if child.tag == "HasCoins"  -> loop through children(coins) -> setActive(true)
        {
            debugCounter++;
            child.SetParent(unusedObstacles.transform);
            Collider[] colliders = child.GetComponents<Collider>();  //enable obstacle colliders
            foreach (Collider coll in colliders)
            {
                coll.enabled = true;
            }
            child.position = new Vector3(0, -100, 0);
        }
        //Debug.Log("'"+ gOToUnparentAllChildren.name+ "' childCount: " + t.childCount+" unparented: "+debugCounter);
    }

    void generateBears(GameObject mappart)
    {
        float mapPosZ = mappart.transform.position.z;
        for (int i = 0; i < obsBearArray.Length; i++)  //4 bears in cave
        {
            int random = Random.Range(0, 4); // 0-3 (4 exclusive)
            obsBearArray[i].transform.position = new Vector3(posX[random], 0, (mapPosZ - 70) + (20 * i));  //Lane Abstand 20 (immer Vielfache)
            obsBearArray[i].transform.GetChild(1).gameObject.GetComponentInChildren<Collider>().enabled = true; //enable bear colliders
            Animator bAnimator = obsBearArray[i].GetComponent<Animator>();
            if (bAnimator.GetCurrentAnimatorStateInfo(0).IsName("pose1") || bAnimator.GetCurrentAnimatorStateInfo(0).IsName("pose2"))
            {
                obsBearArray[i].GetComponent<Animator>().SetTrigger("TriggerBackToBoxing");
            }
        }
    }
}

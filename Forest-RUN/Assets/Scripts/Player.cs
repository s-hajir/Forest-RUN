using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {
    public float gravity = 50;
    public float speed = 40;
    public Vector3 moveDirection;

    private CharacterController charContr;
    private Animator playerAnimator;
    private float[] posX = { -5.8f, -2.1f, 1.36f, 5f };
    private int posX_currentIndex = 1;    //index tells us on which of the 4 paths we currently are
    private float playerToPosX = -2.1f;

    void Start () {
        charContr = GetComponent<CharacterController>();
        playerAnimator = GetComponent<Animator>();

        moveDirection = Vector3.forward * speed;       //init movedirection
    }
	

	void Update () {

        if (Input.GetKeyDown(KeyCode.W))
        {
            //manipulate moveDirection
            if (charContr.isGrounded)
            {
                if (Random.Range(0, 10) > 5)
                {
                    moveDirection = Vector3.forward * speed + Vector3.up * 18;
                    playerAnimator.SetTrigger("TriggerUpAlt");
                }
                else
                {
                    moveDirection = Vector3.forward * speed + Vector3.up * 18;
                    playerAnimator.SetTrigger("TriggerUp");
                }
            }
        }
        else if (Input.GetKeyDown(KeyCode.S))
        {
            if (!charContr.isGrounded)
            {
                moveDirection = Vector3.forward * speed + Vector3.down * 18;
            }
            else
            {
                moveDirection = Vector3.forward * speed;
                playerAnimator.SetTrigger("TriggerDown");
            }
        }
        else if (Input.GetKeyDown(KeyCode.A))  
        {
            playerToPosX = switchLaneLeft();
            if (charContr.isGrounded)
            {
                if (!playerAnimator.GetCurrentAnimatorStateInfo(0).IsName("down"))
                {
                    playerAnimator.SetTrigger("TriggerLeft");
                }
            }
        }
        else if (Input.GetKeyDown(KeyCode.D))
        {
            playerToPosX = switchLaneRight();
            if (charContr.isGrounded)
            {
                if (!playerAnimator.GetCurrentAnimatorStateInfo(0).IsName("down"))
                {
                    playerAnimator.SetTrigger("TriggerRight");
                }
            }
        }
        else if (Input.GetKeyDown(KeyCode.J))
        {
            Time.timeScale = 0.1f;
        }
        else if (Input.GetKeyDown(KeyCode.K))
        {
            Time.timeScale = 1f;
        }

        //MoveCharacter
        charContr.Move(moveDirection * Time.deltaTime);

        if (!charContr.isGrounded)
        {
                //moveDirection.y = 0; //delta is 0 -> stop moving up or down
                moveDirection.y -= gravity * Time.deltaTime;      //move down at each update
            
        }
        //Switsch lane(x-Axis) only
        gameObject.transform.position = new Vector3(Mathf.Lerp(transform.position.x, playerToPosX, Time.fixedDeltaTime * 10), transform.position.y, transform.position.z);
    }

    float switchLaneRight()  //from current lane -> move 1 lane to right
    {
        float x = 0f;
        switch (posX_currentIndex)
        {
            case 0:
                Debug.Log("RIGHT from 0 -> 1");
                posX_currentIndex = 1;
                x = posX[posX_currentIndex];
                break;
            case 1:
                Debug.Log("RIGHT from 1 -> 2");
                posX_currentIndex = 2;
                x = posX[posX_currentIndex];
                break;
            case 2:
                Debug.Log("RIGHT from 2 -> 3");
                posX_currentIndex = 3;
                x = posX[posX_currentIndex];
                break;
            case 3:
                Debug.Log("RIGHT from 3 -> 3");
                posX_currentIndex = 3;
                x = posX[posX_currentIndex];
                break;
            default:
                break;
        }
        return x;
    }
    float switchLaneLeft() //from current lane -> move 1 lane to left
    {
        float x = 0f;
        switch (posX_currentIndex)
        {
            case 0:
                Debug.Log("Left from 0 -> 0");
                posX_currentIndex = 0;
                x = posX[posX_currentIndex];
                break;
            case 1:
                Debug.Log("Left from 1 -> 0");
                posX_currentIndex = 0;
                x = posX[posX_currentIndex];
                break;
            case 2:
                Debug.Log("Left from 2 -> 1");
                posX_currentIndex = 1;
                x = posX[posX_currentIndex];
                break;
            case 3:
                Debug.Log("Left from 3 -> 2");
                posX_currentIndex = 2;
                x = posX[posX_currentIndex];
                break;
            default:
                break;
        }
        return x;
    }
}

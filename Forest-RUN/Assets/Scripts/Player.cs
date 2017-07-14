﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine.Windows.Speech;
using System.Linq;
using UnityEngine;
using System;

public class Player : MonoBehaviour {
    public float gravity = 50;
    public Vector3 moveDirection;

    private CharacterController charContr;
    private Animator playerAnimator;
    private float[] posX = { -5.8f, -2.1f, 1.36f, 5f };
    private int posX_currentIndex = 1;    //index tells us on which of the 4 paths we currently are
    private float playerToPosX = -2.1f;
    private int stopMovementCounter = 40;   //stop movement after collision (for 40 frames)
    private bool stopMovement = false;
    private Vector3 tmpMoveDirection;

    KeywordRecognizer keywordRcognizer;    //speech recognition
    Dictionary<string, System.Action> keywords = new Dictionary<string, System.Action>(); //speech recognition

    void Start () {
        charContr = GetComponent<CharacterController>();
        playerAnimator = GetComponent<Animator>();
        moveDirection = Vector3.forward * GameStateManager.speed;       //init movedirection

        //init speech recognition
        keywords.Add("up", () =>
        {
            upCalled();
        });
        keywords.Add("down", () =>
        {
            downCalled();
        });

        keywordRcognizer = new KeywordRecognizer(keywords.Keys.ToArray());
        keywordRcognizer.OnPhraseRecognized += KeywordRcognizer_OnPhraseRecognized;
        keywordRcognizer.Start();
    }

    private void KeywordRcognizer_OnPhraseRecognized(PhraseRecognizedEventArgs args)//when any phareses are recognized ->we get an event
    {
        System.Action keywordAction;

        if (keywords.TryGetValue(args.text, out keywordAction))
        {
            keywordAction.Invoke();
        }
    }

    private void upCalled()
    {
        print("UP recognized");
        //manipulate moveDirection
        if (charContr.isGrounded)
        {
            if (UnityEngine.Random.Range(0, 10) > 5)
            {
                moveDirection = Vector3.forward * GameStateManager.speed + Vector3.up * 18;
                playerAnimator.SetTrigger("TriggerUpAlt");
            }
            else
            {
                moveDirection = Vector3.forward * GameStateManager.speed + Vector3.up * 18;
                playerAnimator.SetTrigger("TriggerUp");
            }
        }
    }
    private void downCalled()
    {
        print("DOWN recognized");
        if (!charContr.isGrounded)
        {
            moveDirection = Vector3.forward * GameStateManager.speed + Vector3.down * 18;
        }
        else
        {
            moveDirection = Vector3.forward * GameStateManager.speed;
            playerAnimator.SetTrigger("TriggerDown");
        }
    }

    void Update () {
        if (stopMovementCounter < 40 && stopMovement) stopMovementCounter++;
        else if (stopMovementCounter == 40 && stopMovement)
        {
            moveDirection = Vector3.forward * GameStateManager.speed;
            stopMovement = false;
        }

        if (Input.GetKeyDown(KeyCode.W))
        {
            //up
        }
        else if (Input.GetKeyDown(KeyCode.S))
        {
            //down
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
                //Debug.Log("RIGHT from 0 -> 1");
                posX_currentIndex = 1;
                x = posX[posX_currentIndex];
                break;
            case 1:
                //Debug.Log("RIGHT from 1 -> 2");
                posX_currentIndex = 2;
                x = posX[posX_currentIndex];
                break;
            case 2:
                //Debug.Log("RIGHT from 2 -> 3");
                posX_currentIndex = 3;
                x = posX[posX_currentIndex];
                break;
            case 3:
                //Debug.Log("RIGHT from 3 -> 3");
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
                //Debug.Log("Left from 0 -> 0");
                posX_currentIndex = 0;
                x = posX[posX_currentIndex];
                break;
            case 1:
                //Debug.Log("Left from 1 -> 0");
                posX_currentIndex = 0;
                x = posX[posX_currentIndex];
                break;
            case 2:
                //Debug.Log("Left from 2 -> 1");
                posX_currentIndex = 1;
                x = posX[posX_currentIndex];
                break;
            case 3:
                //Debug.Log("Left from 3 -> 2");
                posX_currentIndex = 2;
                x = posX[posX_currentIndex];
                break;
            default:
                break;
        }
        return x;
    }
    public void playerCollision(GameObject collidedWith)
    {
        //collision with stone -> check if animator state is 'down' ->if yes: play default collision animation 
        //                                                          ->else: play stumble animation
        if (collidedWith.CompareTag("bear"))
        {
            Debug.Log("collided w. bear: " + collidedWith.transform.parent.parent.gameObject.name);
        }
        else Debug.Log("player collided with: '" + collidedWith.name + "'");

        if (collidedWith.CompareTag("stone"))
        {
            if (playerAnimator.GetCurrentAnimatorStateInfo(0).IsName("down")) //slide against stone
            {
                processDefaultCollision();
            }
            else
            {
                playerAnimator.SetTrigger("TriggerStumbleCollision");
            }
        }
        else if (collidedWith.CompareTag("bear"))
        {
            if (playerAnimator.GetCurrentAnimatorStateInfo(0).IsName("down")) //slide against bear
            {
                //bear dies
            }
            else
            {
                processDefaultCollision();
            }
        }
        else   //all other obstacles
        {
            processDefaultCollision();
        }
    }
    void processDefaultCollision()
    {
        GameStateManager.deleteOneLife();
        playerAnimator.SetTrigger("TriggerDefaultCollision");
        moveDirection = Vector3.forward * 0;
        stopMovementCounter = 0;
        stopMovement = true;
    }
}

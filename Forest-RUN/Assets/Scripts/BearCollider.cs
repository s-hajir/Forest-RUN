using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BearCollider : MonoBehaviour {

    public Player playerScript;
    public Animator playerAnimator;
    public Animator bearAnimator;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            GetComponent<Collider>().enabled = false;
            playerScript.playerCollision(gameObject);
            if (playerAnimator.GetCurrentAnimatorStateInfo(0).IsName("down"))
            {
                bearAnimator.SetTrigger("TriggerFlyDeath");
                Debug.Log("bear trigger death");
            }
            else
            {
                bearAnimator.SetTrigger("TriggerFlyDefault");
                Debug.Log("bear trigger flyDefault");
            }
        }
    }

    public void disableParent()  //AnimationEvent is called from parent
    {
        gameObject.SetActive(false);
    }
}

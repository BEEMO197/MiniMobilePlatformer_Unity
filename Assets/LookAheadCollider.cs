using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAheadCollider : MonoBehaviour
{
    public bool touchingAhead;
    public bool playerAhead;

    public PlayerMovement pm;

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Ground") || col.CompareTag("Ramp") || col.CompareTag("Enemy") || col.CompareTag("Wall"))
        {
            touchingAhead = true;
        }
        else if (col.CompareTag("Player"))
        {
            playerAhead = true;
            pm = col.gameObject.GetComponent<PlayerMovement>();
        }
    }

    void OnTriggerExit2D(Collider2D col)
    {
        if (col.CompareTag("Ground") || col.CompareTag("Ramp") || col.CompareTag("Enemy") || col.CompareTag("Wall"))
        {
            touchingAhead = false;
        }
        else if(col.CompareTag("Player"))
        {
            playerAhead = false;
            pm = null;
        }
    }
}

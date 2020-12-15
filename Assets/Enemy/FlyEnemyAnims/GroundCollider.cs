using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundCollider : MonoBehaviour
{
    public bool touchingGround = true;

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Ground") || col.CompareTag("Ramp"))
        {
            touchingGround = true;
        }
    }

    void OnTriggerStay2D(Collider2D col)
    {
        if (col.CompareTag("Ground") || col.CompareTag("Ramp"))
        {
            touchingGround = true;
        }
    }

    void OnTriggerExit2D(Collider2D col)
    {
        if (col.CompareTag("Ground") || col.CompareTag("Ramp"))
        {
            touchingGround = false;
        }
    }
}

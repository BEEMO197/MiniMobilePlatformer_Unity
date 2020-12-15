using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackCollider : MonoBehaviour
{
    public Collider2D attackCol;
    public bool hitEnemy;
    public Collider2D colliderHit;

    void OnTriggerEnter2D(Collider2D col)
    {
        if(col.CompareTag("Enemy"))
        {
            hitEnemy = true;
            colliderHit = col;
        }
    }

    void OnTriggerExit2D(Collider2D col)
    {
        if (col.CompareTag("Enemy"))
        {
            hitEnemy = false;

            colliderHit = null;
        }
    }

}

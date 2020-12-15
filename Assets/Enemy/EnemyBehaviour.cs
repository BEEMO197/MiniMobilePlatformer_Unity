using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBehaviour : MonoBehaviour
{
    public Rigidbody2D rigidBody2D;
    public Collider2D col;
    public Animator animator;

    public GroundCollider groundCollider;
    public LookAheadCollider lookAheadCollider;
    public float gravScale = 1.0f;

    public Transform LookAheadPoint;

    public float maxVelX = 2.5f;
    public float Speed = 1.0f;
    public float ForcePower = 500.0f;
    public int direction = 1;

    public bool attack = false;
    public bool attacked = false;
    public bool attackingWait = false;

    public int Lives = 3;
    public bool isDying = false;

    // Start is called before the first frame update
    //void Start()
    //{
    //    
    //}

    // Update is called once per frame
    void Update()
    {
        FloatMovement();
        CheckAhead();
        Move();
        AnimateFlyingEnemy();
    }

    void FloatMovement()
    {
        if (!groundCollider.touchingGround)
        {
            rigidBody2D.gravityScale = gravScale;
        }
        else
        {
            rigidBody2D.gravityScale = -gravScale;
        }

    }

    void CheckAhead()
    {
        if (lookAheadCollider.touchingAhead)
        {
            direction *= -1;
            transform.localScale = new Vector2(transform.localScale.x * -1, transform.localScale.y);
        }

        if (lookAheadCollider.playerAhead)
        {
            attack = true;
            if (!attackingWait)
            {
                StartCoroutine(AttackPlayer());
            }
        }
        else
        {
            attack = false;
        }
    }

    public void Damage()
    {
        Lives -= 1;
        attacked = true;
        StartCoroutine(ChangeAttacked());
        
    }

    IEnumerator ChangeAttacked()
    {
        yield return new WaitForSeconds(0.5f);
        attacked = false;
        if (Lives <= 0)
        {
            isDying = true;
        }
    }

    IEnumerator AttackPlayer()
    {
        attackingWait = true;
        yield return new WaitForSeconds(0.833f);

        if (attack)
        {
            attackingWait = false;
            lookAheadCollider.pm.lives -= 1;
        }
        else
        {
            attackingWait = false;
        }
        yield return null;
    }

    void Move()
    {
        if (!isDying)
        {
            if(attacked)
            {
                rigidBody2D.AddForce(new Vector2(ForcePower * direction * -1 * Time.deltaTime * Speed, 0.0f));
            }
            else if (!attack)
            {
                rigidBody2D.AddForce(new Vector2(ForcePower * direction * Time.deltaTime * Speed, 0.0f));
            }
            else
            {
                rigidBody2D.velocity = new Vector2(float.Epsilon, float.Epsilon);
            }

            if (rigidBody2D.velocity.x >= maxVelX)
            {
                rigidBody2D.velocity = new Vector2(maxVelX, rigidBody2D.velocity.y);
            }

            else if (rigidBody2D.velocity.x <= -maxVelX)
            {
                rigidBody2D.velocity = new Vector2(-maxVelX, rigidBody2D.velocity.y);
            }
        }
        else
        {
            StartCoroutine(Kill());
        }
    }

    IEnumerator Kill()
    {
        yield return new WaitForSeconds(0.0f);

        Destroy(gameObject);

        yield return null;
    }

    void AnimateFlyingEnemy()
    {
        // 0 Default/Walking, 
        // 1 Walking, 
        // 2 Hit

        Debug.Log("Animating");
        if(isDying)
        {
            // Hit
            animator.SetInteger("AnimState", 2);
        }
        else if (attacked)
        {
            // Hit
            animator.SetInteger("AnimState", 2);
        }
        else if (attack)
        {
            // Attacking
            Debug.Log("Attacking");
            animator.SetInteger("AnimState", 1);
        }
        else if (rigidBody2D.velocity.x > float.Epsilon || rigidBody2D.velocity.x < -float.Epsilon)
        {
            // Walking / Moving
            animator.SetInteger("AnimState", 0);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public Joystick joystick;

    public float joystickHorizontalSensitivity;
    public float joystickVerticalSensitivity;

    public float maxHorizontalVelocity;
    public float horizontalVelocity;
    public float verticalVelocity;
    
    public bool isGrounded;
    public bool isJumping;
    public bool isCrouching;
    
    public Transform spawnPoint;

    public Transform lookAheadPoint;
    public Transform lookBelowPoint;

    public bool rampAhead;
    public bool rampBelow;

    private Rigidbody2D m_rigidBody2D;
    private SpriteRenderer m_spriteRenderer;
    private Animator m_animator;

    public int lives = 3;

    public bool attack = false;
    public bool attacked = false;
    public bool attackingWait = false;
    public GameObject attackCol;

    // Start is called before the first frame update
    void Start()
    {
        m_rigidBody2D = GetComponent<Rigidbody2D>();
        m_spriteRenderer = GetComponent<SpriteRenderer>();
        m_animator = GetComponent<Animator>();
    }

    void FixedUpdate()
    {
        LookAheadBelow();
        _Move();
    }

    void _Move()
    {
        if (isGrounded && !attack)
        {
            if (!isJumping && !isCrouching)
            {
                if (joystick.Horizontal > joystickHorizontalSensitivity)
                {
                    // move right
                    if(rampAhead && rampBelow)
                    {
                        m_rigidBody2D.AddForce(Vector2.right * horizontalVelocity * Time.deltaTime);
                    }
                    m_rigidBody2D.AddForce(Vector2.right * horizontalVelocity * Time.deltaTime);

                    if(m_rigidBody2D.velocity.x > 5.0f)
                    {
                        m_rigidBody2D.velocity = new Vector2(5.0f, m_rigidBody2D.velocity.y);
                    }

                    //m_rigidBody2D.velocity = new Vector2(Vector2.right.x * horizontalVelocity, m_rigidBody2D.velocity.y);
                    transform.localScale = new Vector3(1.0f, transform.localScale.y, transform.localScale.z);

                    if(m_animator.GetInteger("AnimState") != (int)PlayerMovementType.RUN)
                        m_animator.SetInteger("AnimState", (int)PlayerMovementType.RUN);
                }
                else if (joystick.Horizontal < -joystickHorizontalSensitivity)
                {
                    // move left
                    if(rampAhead && rampBelow)
                    {
                        m_rigidBody2D.AddForce(Vector2.left * horizontalVelocity * Time.deltaTime);
                    }

                    m_rigidBody2D.AddForce(Vector2.left * horizontalVelocity * Time.deltaTime);

                    if (m_rigidBody2D.velocity.x < -5.0f)
                    {
                        m_rigidBody2D.velocity = new Vector2(-5.0f, m_rigidBody2D.velocity.y);
                    }

                    //m_rigidBody2D.velocity = new Vector2(Vector2.left.x * horizontalVelocity, m_rigidBody2D.velocity.y);
                    transform.localScale = new Vector3(-1.0f, transform.localScale.y, transform.localScale.z);

                    if (m_animator.GetInteger("AnimState") != (int)PlayerMovementType.RUN)
                        m_animator.SetInteger("AnimState", (int)PlayerMovementType.RUN);
                }
                else if (!isJumping)
                {
                    if (m_animator.GetInteger("AnimState") != (int)PlayerMovementType.IDLE)
                        m_animator.SetInteger("AnimState", (int)PlayerMovementType.IDLE);
                }
            }

            if ((joystick.Vertical > joystickVerticalSensitivity) && (!isJumping))
            {
                // jump
                Jump();
            }
            else
            {
                isJumping = false;
            }
        }

    }

    public void Jump()
    {
        if (!isJumping)
        {
            m_rigidBody2D.AddForce(Vector2.up * verticalVelocity);
            if (m_animator.GetInteger("AnimState") != (int)PlayerMovementType.JUMP)
                m_animator.SetInteger("AnimState", (int)PlayerMovementType.JUMP);
            isJumping = true;
            isGrounded = false;
        }
    }

    public void Attack()
    {
        if (!attackingWait)
        {
            attack = true;
            attackingWait = true;
            m_animator.SetInteger("AnimState", (int)PlayerMovementType.ATTACK);
            attackCol.SetActive(true);
            StartCoroutine(AttackChange());
            
        }
    }

    IEnumerator AttackChange()
    {
        yield return new WaitForSeconds(0.667f);
        attackCol.SetActive(false);
        attackingWait = false;
        attack = false;
    }
    
    void LookAheadBelow()
    {
        var rayAhead = Physics2D.Linecast(lookAheadPoint.position, transform.position);
        Debug.DrawLine(transform.position, lookAheadPoint.position, Color.red);
        

        if (rayAhead.collider.CompareTag("Ramp"))
        {
            rampAhead = true;
        }
        else
        {
            rampAhead = false;
        }

        var rayBelow = Physics2D.Linecast(lookBelowPoint.position, transform.position);
        Debug.DrawLine(transform.position, lookBelowPoint.position, Color.yellow);

        if (rayBelow.collider.CompareTag("Ramp"))
        {
            rampBelow = true;
        }
        else
        {
            rampBelow = false;
        }

    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        isGrounded = true;
        //if (m_animator.GetInteger("AnimState") != (int)PlayerMovementType.JUMP_LAND)
        //    m_animator.SetInteger("AnimState", (int)PlayerMovementType.JUMP_LAND);
    }

    private void OnCollisionExit2D(Collision2D other)
    {
        if(other.collider.CompareTag("Ramp"))
        {
            //isGrounded = true;
        }
        else
        {
            if(!rampAhead)
            {
                isGrounded = false;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // respawn
        if (other.gameObject.CompareTag("DeathPlane"))
        {
            transform.position = spawnPoint.position;
        }
    }
}

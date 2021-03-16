using System.Collections;
using System.Collections.Generic;
using System.Data.SqlTypes;
using UnityEngine;

public class Bunny : Monster
{
    public enum State
    {
        Idle,
        Run,
        Attack,
        Jump
    };
    public State currentState = State.Idle;

    public Transform[] wallCheck;
    WaitForSeconds Delay500 = new WaitForSeconds(0.5f);

    Animator animator;
    GameObject traceTarget;
    bool isTracing;
    Vector3 movement;
    int movementFlag = 0;
    Vector2 boxColliderOffset;
    Vector2 boxColliderJumpOffset;

    void Start()
    {
        animator = GetComponentInChildren<Animator>();
    }
    void Awake()
    {
        base.Awake();
        moveSpeed = 3f;
        jumpPower = 13f;
        currentHp = 60;
        atkCoolTime = 3f;
        atkCoolTimeCalc = atkCoolTime;

        boxColliderOffset = boxCollider.offset;
        boxColliderJumpOffset = new Vector2(boxColliderOffset.x, 1f);

        StartCoroutine(FSM());
    }
    IEnumerator FSM()
    {
        while (true)
        {
            yield return StartCoroutine(currentState.ToString());
        }
    }

    IEnumerator Idle()
    {
        boxCollider.offset = boxColliderOffset;
        yield return Delay500;
        currentState = State.Run;
    }

    IEnumerator Run()
    {
        yield return null;
        float runTime = Random.Range(2f, 4f);
        while (runTime >= 0f)
        {
            runTime -= Time.deltaTime;
            if (!isHit)
            {
                MyAnimSetTrigger("isMoving");
                rb.velocity = new Vector2(-transform.localScale.x * moveSpeed, rb.velocity.y);

                if (!Physics2D.OverlapCircle(wallCheck[0].position, 0.01f, layerMask) &&
                Physics2D.OverlapCircle(wallCheck[1].position, 0.01f, layerMask) &&
                 !Physics2D.Raycast(transform.position, -transform.localScale.x * transform.right, 1f, layerMask))
                {
                    currentState = State.Jump;
                    break;
                }
                else if (Physics2D.OverlapCircle(wallCheck[1].position, 0.01f, layerMask))
                {
                    MonsterFlip();
                }

            }
            yield return null;
        }
    }
    IEnumerator Attack()
    {
        yield return null;
        if (!isHit && isGround)
        {
            boxCollider.offset = boxColliderJumpOffset;
            canAtk = false;
            rb.velocity = new Vector2(-transform.localScale.x * 18f, jumpPower / 1.25f);
            MyAnimSetTrigger("Attack");
            yield return Delay500;
            currentState = State.Idle;
        }
        else
        {
            currentState = State.Run;
        }
    }

    IEnumerator Jump()
    {
        yield return null;
        boxCollider.offset = boxColliderJumpOffset;

        rb.velocity = new Vector2(-transform.localScale.x * 6f, jumpPower);
        MyAnimSetTrigger("Attack");
        yield return Delay500;
        currentState = State.Idle;
    }

    void Update()
    {
        GroundCheck();
        if (!isHit && isGround && !IsPlayingAnim("Run"))
        {
            boxCollider.offset = boxColliderOffset;
            MyAnimSetTrigger("Idle");
        }
    }
    void FixedUpdate()
    {
        Move();
    }

    void Move()
    {
        Vector3 moveVelocity = Vector3.zero;
        string dist = "";

        //추적기능
        if (isTracing)
        {
            Vector3 playerPos = traceTarget.transform.position;
            currentState = State.Attack;

            if (playerPos.x < transform.position.x)
                dist = "Left";
            else if (playerPos.x > transform.position.x)
                dist = "Right";
        }
        else
        {
            if (movementFlag == 1)
                dist = "Left";
            else if (movementFlag == 2)
                dist = "Right";
        }

        if (dist == "Left")
        {
            moveVelocity = Vector3.left;
            transform.localScale = new Vector3((float)1.5, (float)1.5, 1);
        }
        else if (dist == "Right")
        {
            moveVelocity = Vector3.right;
            transform.localScale = new Vector3((float)-1.5, (float)1.5, 1);
        };

        transform.position += moveVelocity * moveSpeed * Time.deltaTime;
    }

    void OnTriggerEnter2D(Collider2D other)
    {

        if (other.gameObject.tag == "Player")
        {
            traceTarget = other.gameObject;

            //StopCoroutine("ChangeMovement");
        }
    }
    void OnTriggerStay2D(Collider2D other)
    {

        if (other.gameObject.tag == "Player" || other.gameObject.tag == "PlayerDamaged")
        {
            isTracing = true;
            animator.SetBool("isMoving", true);
            

        }


    }
    void OnTriggerExit2D(Collider2D other)
    {

        if (other.gameObject.tag == "Player")
        {
            isTracing = false;
            //StartCoroutine("ChangeMovement");
        }
    }
}

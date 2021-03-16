using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using Random = UnityEngine.Random;

public class Trunk : Monster
{
    public enum State
    {
        Idle,
        Run,
        Attack,
    };
    public State currentState = State.Idle;

    public Transform[] wallCheck;
    public Transform genPoint;
    public GameObject Bullet;

    Animator animator;
    Vector3 movement;
    int movementFlag = 0;
    bool isTracing;
    GameObject traceTarget;

    WaitForSeconds Delay1000 = new WaitForSeconds(1f);

    // Use this for initialization
    void Start()
    {
        animator = GetComponentInChildren<Animator>();

        //StartCoroutine("ChangeMovement");
    }

    /*IEnumerator ChangeMovement()
    {
        movementFlag = Random.Range(0, 3);

        if (movementFlag == 0)
            animator.SetBool("isMoving", false);
        else
            animator.SetBool("isMoving", true);

        yield return new WaitForSeconds(5f);

        //StartCoroutine("ChangeMovement");
    }*/
    void Awake()
    {
        base.Awake();
        moveSpeed = 2f;
        jumpPower = 10f;
        currentHp = 50;
        atkCoolTime = 1f;
        atkCoolTimeCalc = atkCoolTime;

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
        yield return null;
        MyAnimSetTrigger("Idle");

        if (Random.value > 0.5f)
        {
            MonsterFlip();
        }
        yield return Delay1000;
        currentState = State.Run;
    }
    IEnumerator Run()
    {
        yield return null;
        float runTime = Random.Range(2f, 3f);
        while (runTime >= 0f)
        {
            runTime -= Time.deltaTime;
            MyAnimSetTrigger("isMoving");
            if (!isHit)
            {
                rb.velocity = new Vector2(-transform.localScale.x * moveSpeed, rb.velocity.y);

                /*if (Physics2D.OverlapCircle(wallCheck[1].position, 0.01f, layerMask))
                {
                    MonsterFlip();
                }*/
                /*if (canAtk && IsPlayerDir())
                {
                    if (Vector2.Distance(transform.position, PlayerData.Instance.Player.transform.position) < 15f)
                    {
                        currentState = State.Attack;
                        break;
                    }
                }*/
            }
            yield return null;
        }
        if (currentState != State.Attack)
        {
            if (Random.value > 0.5f)
            {
                MonsterFlip();
            }
            else
            {
                currentState = State.Idle;
            }
        }
    }
    IEnumerator Attack()
    {
        yield return null;

        canAtk = false;
        rb.velocity = new Vector2(0, 0);
        MyAnimSetTrigger("Attack");

        yield return Delay1000;
        currentState = State.Idle;
    }
    // Update is called once per frame

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
            transform.localScale = new Vector3((float)1.3, (float)1.5, 1);
        }
        else if (dist == "Right")
        {
            moveVelocity = Vector3.right;
            transform.localScale = new Vector3((float)-1.3, (float)1.5, 1);
        }

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
            currentState = State.Attack;

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
    

    // Update is called once per frame
    void Update()
    {
        if (!isHit)
        {
            rb.velocity = new Vector2(-transform.localScale.x * moveSpeed, rb.velocity.y);

            if (!Physics2D.OverlapCircle(wallCheck[0].position, 0.01f, layerMask) &&
                Physics2D.OverlapCircle(wallCheck[1].position, 0.01f, layerMask) &&
                 !Physics2D.Raycast(transform.position, -transform.localScale.x * transform.right, 1f, layerMask))
            {
                rb.velocity = new Vector2(rb.velocity.x, jumpPower);
            }
            else if (Physics2D.OverlapCircle(wallCheck[1].position, 0.01f, layerMask))
            {
                MonsterFlip();
            }
        }

        //MoveToTarget();

    }
    void Fire() 
    {
        GameObject bulletClone = Instantiate(Bullet, genPoint.position, transform.rotation);
        bulletClone.GetComponent<Rigidbody2D>().velocity = transform.right * -transform.localScale.x * 10f;
        bulletClone.transform.localScale = new Vector2(transform.localScale.x, 1f);
    }
}
/*
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using Random = UnityEngine.Random;

public class Trunk : Monster
{
    public Transform[] wallCheck;
    public Transform genPoint;
    public GameObject Bullet;

    Animator animator;
    Vector3 movement;
    int movementFlag = 0;
    bool isTracing;
    GameObject traceTarget;

    WaitForSeconds Delay1000 = new WaitForSeconds(1f);

    // Use this for initialization
    void Start()
    {
        animator = GetComponentInChildren<Animator>();

        //StartCoroutine("ChangeMovement");
    }

    /*IEnumerator ChangeMovement()
    {
        movementFlag = Random.Range(0, 3);

        if (movementFlag == 0)
            animator.SetBool("isMoving", false);
        else
            animator.SetBool("isMoving", true);

        yield return new WaitForSeconds(5f);

        //StartCoroutine("ChangeMovement");
    }
    void Awake()
    {
        base.Awake();
        moveSpeed = 1f;
        jumpPower = 10f;
        currentHp = 4;
        atkCoolTime = 3f;
        atkCoolTimeCalc = atkCoolTime;

        //StartCoroutine(FSM());
    }

    // Update is called once per frame

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
            transform.localScale = new Vector3((float)1.3, (float)1.5, 1);
        }
        else if (dist == "Right")
        {
            moveVelocity = Vector3.right;
            transform.localScale = new Vector3((float)-1.3, (float)1.5, 1);
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


    // Update is called once per frame
    void Update()
    {
        if (!isHit)
        {
            rb.velocity = new Vector2(-transform.localScale.x * moveSpeed, rb.velocity.y);

            if (!Physics2D.OverlapCircle(wallCheck[0].position, 0.01f, layerMask) &&
                Physics2D.OverlapCircle(wallCheck[1].position, 0.01f, layerMask) &&
                 !Physics2D.Raycast(transform.position, -transform.localScale.x * transform.right, 1f, layerMask))
            {
                rb.velocity = new Vector2(rb.velocity.x, jumpPower);
            }
            else if (Physics2D.OverlapCircle(wallCheck[1].position, 0.01f, layerMask))
            {
                MonsterFlip();
                Invoke("Fire", 0.5f);
            }
        }

        //MoveToTarget();

    }
    void Fire()
    {
        GameObject bulletClone = Instantiate(Bullet, genPoint.position, transform.rotation);
        bulletClone.GetComponent<Rigidbody2D>().velocity = transform.right * -transform.localScale.x * 10f;
        bulletClone.transform.localScale = new Vector2(transform.localScale.x, 1f);
    }
}*/
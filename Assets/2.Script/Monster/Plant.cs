using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plant : Monster
{
    public enum State
    {
        Idle,
        Attack,
    };
    public State currentState = State.Idle;

    public Transform genPoint;
    public GameObject Bullet;
    Animator animator;
    bool isTracing;
    GameObject traceTarget;
    int movementFlag = 0;

    WaitForSeconds Delay1000 = new WaitForSeconds(1f);
    void start()
    {
        animator = GetComponentInChildren<Animator>();
    }

    void Awake()
    {
        base.Awake();
        currentHp = 40;
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
        currentState = State.Attack;
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
            transform.localScale = new Vector3((float)1.1, (float)1.0, 1);
        }
        else if (dist == "Right")
        {
            moveVelocity = Vector3.right;
            transform.localScale = new Vector3((float)-1.1, (float)1.0, 1);
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

    void Fire()
    {
        GameObject bulletClone = Instantiate(Bullet, genPoint.position, transform.rotation);
        bulletClone.GetComponent<Rigidbody2D>().velocity = transform.right * -transform.localScale.x * 10f;
        bulletClone.transform.localScale = new Vector2(transform.localScale.x, 1f);
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using Random = UnityEngine.Random;

public class Mushroom : Monster
{
    public Transform[] wallCheck;

    Animator animator;
    Vector3 movement;
    int movementFlag = 0;
    bool isTracing;
    GameObject traceTarget;


    // Use this for initialization
    void Start()
    {
        animator = GetComponentInChildren<Animator>();

        //StartCoroutine("ChangeMovement");
    }

/*    IEnumerator ChangeMovement()
    {
        movementFlag = Random.Range(0, 3);

        if (movementFlag == 0)
            animator.SetBool("isMoving", false);
        else
            animator.SetBool("isMoving", true);

        yield return new WaitForSeconds(3f);

        StartCoroutine("ChangeMovement");
    }*/

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
    private void Awake()
    {
        base.Awake();
        moveSpeed = 2f;
        jumpPower = 6f;
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

/*    public void MoveToTarget()
    {
        // Player의 현재 위치를 받아오는 Object
        target = GameObject.Find("Player").transform;
        // Player의 위치와 이 객체의 위치를 빼고 단위 벡터화 한다.
        direction = (target.position - transform.position).normalized;
        // 가속도 지정 (추후 힘과 질량, 거리 등 계산해서 수정할 것)
        accelaration = 0.05f;
        // 초가 아닌 한 프레임으로 가속도 계산하여 속도 증가
        velocity = (velocity + accelaration * Time.deltaTime);
        // Player와 객체 간의 거리 계산
        float distance = Vector3.Distance(target.position, transform.position);
        // 일정거리 안에 있을 시, 해당 방향으로 무빙
        if (distance <= 10.0f)
        {
            this.transform.position = new Vector3(transform.position.x + (direction.x * velocity),
                                                   transform.position.y + (direction.y * velocity),
                                                     transform.position.z);
        }
        // 일정거리 밖에 있을 시, 속도 초기화 
        else
        {
            velocity = 0.0f;
        }
    }*/

/*    protected void OnTriggerEnter2D(Collider2D collision)
    {
        base.OnTriggerEnter2D(collision);
        if (collision.transform.CompareTag("Player"))
        {
            MonsterFlip();
        }
    }*/
}

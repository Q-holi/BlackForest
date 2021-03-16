using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] float m_rollForce = 10.0f;
    private bool m_slide = false;
    private int m_facingDirection = 1;
    private float m_timeSlice = 0.0f;

    public GameManager gameManager;
    public CharacterController2D controller;
    public Animator animator; // 플레이어 캐릭터의 애니메이션을 가져온다.
    private Rigidbody2D m_body2d;
    private CapsuleCollider2D capsuleCollider;
    private SpriteRenderer spriteRenderer;

    private AudioSource audioSource;
    public AudioClip audioJump;
    public AudioClip audioAttack;
    public AudioClip audioDamaged;
    public AudioClip audioItem;
    public AudioClip audioDie;
    public AudioClip audioFinish;

    public float runSpeed = 80f;//플레이어의 달리기 속도 

    float horizontalMove = 0f;
    bool jump = false;//junmp가 참일때 점프를 할수 있도록 bool형식의 변수 선언
    bool crouch = false;//위 jump와 마찬가지로 숙이기 변수 선언

    private void Start()
    {
        animator = GetComponent<Animator>();
        m_body2d = GetComponent<Rigidbody2D>();
    }
    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        capsuleCollider = GetComponent<CapsuleCollider2D>();
        audioSource = GetComponent<AudioSource>();
    }
    // Update is called once per frame
    void Update()
    {
        m_timeSlice += Time.deltaTime;
        horizontalMove = Input.GetAxisRaw("Horizontal") * runSpeed;//플레이어 캐릭터가 이동을할때 최고 속도를 곱하여 변수에 대입해준다.
        animator.SetFloat("Speed", Mathf.Abs(horizontalMove));//달리기 애니메이션은 0.01이상이면 출력할수있도록 설정을 하였다.

        float inputX = Input.GetAxis("Horizontal");

        if (inputX > 0)
        {
            m_facingDirection = 1;
        }

        else if (inputX < 0)
        {
            m_facingDirection = -1;
        }

        if (Input.GetButtonDown("Jump"))
        {
            jump = true;
            animator.SetBool("IsJump", true);
            PlaySound("JUMP");
        }

        if (Input.GetButtonDown("Crouch"))
        {
            crouch = true;
        }
        else if (Input.GetButtonUp("Crouch"))
        {
            crouch = false;
        }
        // m_slide
        else if (Input.GetKeyDown("left shift") && m_timeSlice > 1f/*&& !m_slide*/)
        {
            m_slide = true;
            animator.SetTrigger("Slide");
            m_body2d.velocity = new Vector2(m_facingDirection * m_rollForce, m_body2d.velocity.y);
            gameObject.layer = 11;
            Invoke("OffDamaged", 1.0f);

            m_timeSlice = 0.0f;
        }

    }

    public void OnLanding()
    {
        animator.SetBool("IsJump", false);
    }

    public void OnCrouching(bool isCrouching)
    {
        animator.SetBool("IsCrouching", isCrouching);
    }

    void FixedUpdate()
    {
        controller.Move(horizontalMove * Time.fixedDeltaTime, crouch, jump);//Controller2D에 움직임 속도와 숙이기 점프 등 데이터를 넘겨준다.
        jump = false;
    }
    void PlaySound(string action)
    {
        switch (action)
        {
            case "JUMP":
                audioSource.clip = audioJump;
                break;
            case "ATTACK":
                audioSource.clip = audioAttack;
                break;
            case "DAMAGED":
                audioSource.clip = audioDamaged;
                break;
            case "ITEM":
                audioSource.clip = audioItem;
                break;
            case "DIE":
                audioSource.clip = audioDie;
                break;
            case "FINISH":
                audioSource.clip = audioFinish;
                break;
        }
        audioSource.Play();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Item")
        {
            // Point
            bool isBronze = collision.gameObject.name.Contains("Bronze");
            bool isSilver = collision.gameObject.name.Contains("Silver");
            bool isGold = collision.gameObject.name.Contains("Gold");

            if (isBronze)
                gameManager.stagePoint += 50;
            else if (isSilver)
                gameManager.stagePoint += 100;
            else if (isGold)
                gameManager.stagePoint += 300;

            // Deactive Item
            collision.gameObject.SetActive(false);

            PlaySound("ITEM");
        }
        else if (collision.gameObject.tag == "Finish")
        {
            // Next Stage
            gameManager.NextStage();
            Debug.Log("클리어");
            PlaySound("FINISH");
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (gameObject.tag == "Player")
        {
            if (collision.gameObject.tag == "Enemy" || collision.gameObject.tag == "Projectile")
            {
                //Damaged
                OnDamaged(collision.transform.position);
                PlaySound("DAMAGED");
            }
        }
    }

    void OnDamaged(Vector2 targetPos)
    {
        // Health Down
        gameManager.HealthDown();
        // Change Layer (Immortal Active)
        gameObject.layer = 11;

        // View Alpha
        spriteRenderer.color = new Color(1, 1, 1, 0.4f);

        // Reaction Force
        int dirc = transform.position.x - targetPos.x > 0 ? 1 : -1;
        m_body2d.AddForce(new Vector2(dirc, 1) * 4, ForceMode2D.Impulse);

        // Animation
        //animator.SetTrigger("Hurt");

        Invoke("OffDamaged", 1.5f);
    }

    void OffDamaged()
    {
        gameObject.layer = 9;
        spriteRenderer.color = new Color(1, 1, 1, 1);
    }
    public void OnDie()
    {
        //Sprite Alpha
        //spriteRenderer.color = new Color(1, 1, 1, 0.4f);
        //Spite Flip Y
        //spriteRenderer.flipY = true;
        //Collider Disable
        //capsuleCollider.enabled = false;
        //Die Effect Jump
        m_body2d.AddForce(Vector2.up * 3, ForceMode2D.Impulse);
        //animator.SetTrigger("Death");
        PlaySound("DIE");
        gameObject.layer = 11;
        gameObject.tag = "Enemy";
        Invoke("Playersetflase", 1.5f);
        //Invoke("PlayerDestroy", 3f);
    }
    public void Playersetflase()
    {
        gameObject.SetActive(false);
    }

    public void VelocityZero()
    {
        m_body2d.velocity = Vector2.zero;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCombat : MonoBehaviour//플레이어 공격 구현
{

    public Animator animator;
    private AudioSource audioSource;
    public AudioClip audioAttack;
    private Rigidbody2D m_body2d;

    public Transform attackPoint;//플레이어 공격을 하면 피격을 하기 위한 위치
    public LayerMask enemyLayers;//몬스터의 레이어를 가져와서 몬스터 인것을 타격 

    public float attackRange = 0.5f;//플레이어의 공격 범위
    public int attackDamage = 40;//플레이어의 공격 데미지

    public float attackRate = 2f;//공격 딜레이
    float nextAttackTime = 0f;
    private float m_timeSinceAttack = 0.0f;
    
    private int m_currentAttack = 0;
    private bool m_grounded = false;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }
    void Update()
    {
        m_timeSinceAttack += Time.deltaTime;
        
        if (Input.GetMouseButtonDown(0) && m_timeSinceAttack > 0.25f)
        {

            if (Time.time >= nextAttackTime)
            {
                if (Input.GetMouseButtonDown(0))
                {
                    Attack();
                    nextAttackTime = Time.time + 1f / attackRate;
                }
            }
        }

    }
    void Attack()
    {
        m_currentAttack++;

        animator.SetTrigger("Attack1");//공격이 활성화되면 공격 애니메이션을 추가한다.
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayers);

        foreach (Collider2D enemy in hitEnemies)
        {
            if (enemy.tag == "Enemy")
            {
                Debug.Log(enemy.tag);
                enemy.GetComponent<Monster>().TakeDamage(10);
                break;
            }
            else if (enemy.tag == "BossEnemy")
            {
                Debug.Log(enemy.tag);
                enemy.GetComponent<Enemy>().TakeDamage(10);
                break;
            }
            //enemy.GetComponent<Monster>().TakeDamage(attackDamage);//피격 데미지는 몬스터스크립트에 구현이 되어있다.
        }

        // Loop back to one after third attack
        if (m_currentAttack > 3)
            m_currentAttack = 1;

        // Reset Attack combo if time since last attack is too large
        if (m_timeSinceAttack > 1.0f)
            m_currentAttack = 1;

        // Call one of three attack animations "Attack1", "Attack2", "Attack3"
        animator.SetTrigger("Attack" + m_currentAttack);
        PlaySound("ATTACK");

        // Reset timer
        m_timeSinceAttack = 0.0f;
    }

    void OnDrawGizmosSelected()
    {
        if (attackPoint == null)
            return;


        Gizmos.DrawWireSphere(attackPoint.position, attackRange);

    }

    void PlaySound(string action)
    {
        switch (action)
        {
            case "ATTACK":
                audioSource.clip = audioAttack;
                break;
        }
        audioSource.Play();
    }
}

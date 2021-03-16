using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemy : MonoBehaviour
{
    public Animator animator;   
    public int maxHealth = 100;//몬스터의 최대 체력
    int currentHealth;//최대체력에서 깍여서 남을 체력
    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;

        animator.SetTrigger("Hurt");

        if(currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        Debug.Log("Enemy died!");

        animator.SetBool("IsDead", true);

        GetComponent<Collider2D>().enabled = false;//Collider도 마찬가지로비활성화
        this.enabled = false;//몬스터가 사라지면서 활성화를 비활성화로 바꾼다
    }


}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public Animator animator;
    public int maxHealth = 100;//몬스터의 최대 체력
    int currentHealth;//최대체력에서 깍여서 남을 체력

    private AudioSource audioSource;
    public AudioClip audioDamaged;
    public AudioClip audioDie;
    private GameManager gameManager;
    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    void Start()
    {
        currentHealth = maxHealth;
    }
    public void TakeDamage(int damage)
    {
        currentHealth -= damage;

        animator.SetTrigger("Hurt");
        PlaySound("DAMAGED");
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        Debug.Log("Enemy died!");

        animator.SetBool("IsDead", true);
        PlaySound("DIE");
        GetComponent<Collider2D>().enabled = false;//Collider도 마찬가지로비활성화
        this.enabled = false;//몬스터가 사라지면서 활성화를 비활성화로 바꾼다

        //Invoke("Clear", 2.0f);      
    }

    void PlaySound(string action)
    {
        switch (action)
        {
            case "DAMAGED":
                audioSource.clip = audioDamaged;
                break;   
            case "DIE":
                audioSource.clip = audioDie;
                break;
        }
        audioSource.Play();
    }

    void Clear()
    {
        Time.timeScale = 0;
        gameManager.UIFinishImage.SetActive(true);
    }
}

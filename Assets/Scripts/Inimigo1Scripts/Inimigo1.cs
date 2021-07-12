using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inimigo1 : MonoBehaviour
{
    // Start is called before the first frame update
    public int maxHealth;
    private int currentHealth;
    private Animator animator;
    void Start()
    {
        currentHealth = maxHealth;
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;

        // animacao de machucar

        animator.SetTrigger("Inimigo1Damage");

        if(currentHealth <= 0)
        {
            Die();
        }

    }

    void Die()
    {
        Debug.Log("Enemy "+ this.name + " died!");

        animator.SetBool("Dead", true);
    }
}

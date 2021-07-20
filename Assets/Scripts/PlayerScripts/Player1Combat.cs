using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player1Combat : MonoBehaviour
{
    // Start is called before the first frame update
    public Transform attackPoint;
    public float attackRange=0.5f;
    public int attackDamage = 20;
    public Animator animator;
    public LayerMask enemyLayers;
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.X) || Input.GetKeyDown(KeyCode.Z))
        {
            playerAttackAnim();
        }
    }


    void playerAttackAnim()
    {

        animator.SetTrigger("AttackTrigger");
                                 
    }

    void playerAttackCollDetection()
    {
        Debug.Log("Analisando colisao de ataque...");
        Collider2D[] hitEnemies =  Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayers);
        foreach(Collider2D enemy in hitEnemies)
        {
            Debug.Log("We hit "+ enemy.name);
            Debug.Log("Tipo do inimigo = " + enemy.GetType().ToString());
            System.Type i = enemy.GetType();
            enemy.GetComponent<Inimigo1Script>().TakeDamage(attackDamage);
        }
    }

    void OnDrawGizmosSelected()
    {  
        if(!attackPoint)
            return;
        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }



}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player1Combat : MonoBehaviour
{
    // Start is called before the first frame update
    public Transform attackPoint;

    public GameObject playerGameObject;
    public float attackRange=0.5f;
    public int attackDamage = 20;

    public float attackFrequency=0.5f;
    float tempoAttack;
    public Animator animator;
    public LayerMask enemyLayers;

    public Player1 player1Script;
    void Start()
    {
        player1Script = playerGameObject.GetComponent<Player1>();
        animator = GetComponent<Animator>();
        tempoAttack= attackFrequency;
    }

    // Update is called once per frame
    void Update()
    {
        tempoAttack = Mathf.Clamp((tempoAttack - Time.deltaTime), 0, Mathf.Infinity);
        if(Input.GetKeyDown(KeyCode.X) || Input.GetKeyDown(KeyCode.Z) && (player1Script.isGrounded()) && (tempoAttack == 0))
        {
            
            playerAttackAnim();
            tempoAttack = attackFrequency;
        }
    }


    void playerAttackAnim()
    {

        animator.SetBool("Attack", true);
        animator.SetTrigger("AttackTrigger");
                                 
    }

    void playerStopAttackAnim()
    {
        animator.SetBool("Attack", false);
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

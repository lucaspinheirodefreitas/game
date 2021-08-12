using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inimigo1Script : MonoBehaviour
{
    public int maxHealth;
    private int currentHealth;
    public Animator animator;
    public bool noChao;
    public bool olhandoDireita;
    public bool Pulou;
    public float Speed;
    public float horizontalMove;
    public float verticalMove;
    public Transform attackPoint;
    public LayerMask enemyLayers;
    public float attackRange=0.5f;
    public int attackDamage = 20;
    public bool tomandoDano;
    public bool attack;
    [SerializeField]
    private LayerMask groundLayer;
    private Rigidbody2D rigidbody2D;
    private BoxCollider2D boxCollider2D;
    public LayerMask GroundLayer  { 
        get {
            return groundLayer;
        }
    }

    void Start()
    {
        currentHealth = maxHealth;
        animator = GetComponent<Animator>();
        boxCollider2D = GetComponent<BoxCollider2D>();
        Pulou = false;
        rigidbody2D = GetComponent<Rigidbody2D>();
        attack = false;
        tomandoDano = false;
    }

    void FixedUpdate() {
        // TODO revisar necessidade
        if(tomandoDano && animator.GetBool("Dead") == false) {
            Debug.Log("Inimigo morreu");
            animator.SetTrigger("Inimigo1Damage");
        }
        
        //properFlip();
    }

    public void TakeDamage(int damage) {
        tomandoDano = true;
        currentHealth -= damage;

        Debug.Log("Vida atual = " + currentHealth);

        animator.SetTrigger("Inimigo1Damage");

        if(currentHealth <= 0)
            Die();

    }

    public void ResetTakeDamage() {
        tomandoDano = false;
    }

    void Die() {
        Debug.Log("Enemy "+ this.name + " died!");
        animator.SetBool("Dead", true);
        

    }

    public void Jump() {
        if (noChao && Pulou) {
            rigidbody2D.velocity = Vector2.up * Speed;
            animator.SetFloat("Jump", Speed);
        }
        animator.SetFloat("Jump", 0.0f);
    }

    void playerStopJumpAnim() {
        if(noChao && rigidbody2D.velocity.y <0.1f)
            animator.SetFloat("Jump", 0f);
    }

    public bool isGrounded()
    {
        float TestLength = 0.1f, ajusteFlipRay;

        if(olhandoDireita)
            ajusteFlipRay = 0.3f;
        else
            ajusteFlipRay = 0f;

        Vector2 Start =  new Vector2(boxCollider2D.bounds.min.x + TestLength + ajusteFlipRay, boxCollider2D.bounds.min.y - TestLength);
        Vector2 Direction = new Vector2((boxCollider2D.bounds.extents.x), 0);

        RaycastHit2D hit2DSingle = Physics2D.Raycast(Start, Direction, Direction.magnitude, groundLayer);
        Color SingleRayColor;

        SingleRayColor = Color.magenta;

        if(hit2DSingle.collider != null)
        {
            SingleRayColor = Color.green;
        }
        else
        {
            SingleRayColor = Color.red;
        }

        return hit2DSingle.collider != null;

    }

    public void properFlip()
    {
        if((horizontalMove < 0 && olhandoDireita) || (horizontalMove > 0 && !olhandoDireita))
        {
            olhandoDireita = !olhandoDireita;
            transform.Rotate(new Vector3(0, 180, 0));
        }
    }
    
    public void properFlip(int direcao) {
        if((direcao < 0 && olhandoDireita) || (direcao > 0 && !olhandoDireita)) {
            olhandoDireita = !olhandoDireita;
            transform.Rotate(new Vector3(0, 180, 0));
        }
    }

    public void Move(float direcao)
    {
        Vector3 horizontal = new Vector3(direcao, 0.0f, 0.0f);
        transform.position = transform.position + (((horizontal * Time.deltaTime * Speed)));
        if (direcao > 0 || direcao < 0)  // direita
            animator.SetFloat("Speed",  Speed);
        else
           animator.SetFloat("Speed", 0.0f);
        
        properFlip((int)direcao);

    }
    
    public void enemyAttackAnim() {
        animator.SetTrigger("AttackTrigger");                    
    }

    void enemyAttackCollDetection()
    {
        Debug.Log("Iniciando Detecçao de collider de ataque do inimigo1");
        Collider2D[] hitEnemies =  Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayers);
        
        if(hitEnemies.Length > 0) { 
            Collider2D player = hitEnemies[0];

            foreach(Collider2D enemy in hitEnemies)
            {
                if(enemy.name == "Player")
                    player = enemy;

                Debug.Log("We hit "+ enemy.name);
                Debug.Log("Tipo do inimigo = " + enemy.GetType());
                System.Type i = enemy.GetType();
                
            }
            player.GetComponent<Player1>().TakeDamage(attackDamage); // TODO - ajustar BUG
        }
    }

    void OnDrawGizmosSelected()
    {  
        if(!attackPoint)
            return;
        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }



}

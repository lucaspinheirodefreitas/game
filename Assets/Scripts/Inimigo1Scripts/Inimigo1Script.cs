using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inimigo1Script : MonoBehaviour
{
 // Start is called before the first frame update
    public int maxHealth;
    private int currentHealth;
    public Animator animator;

    private bool noChao;
    public bool olhandoDireita;
    private bool Pulou;
    public float Speed;
    public float horizontalMove;
    public float verticalMove;
    public Transform attackPoint;
    public LayerMask enemyLayers;
    public float attackRange=0.5f;
    public int attackDamage = 20;

    public bool tomandoDano = false;

    public bool attack=false;

    [SerializeField]
    private LayerMask groundLayer;
    private Rigidbody2D rigidbody2D;
    private BoxCollider2D boxCollider2D;

    public LayerMask GroundLayer 
    { 
        get
        {
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
        Debug.Log("Inimigo1 attack = " + attack);
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void FixedUpdate()
    {
        //Debug.Log("Inimigo1 attack = " + attack);
        if (horizontalMove != 0)
            Move();
        else
            animator.SetFloat("Speed", 0f);

        if(verticalMove != 0)
            Jump();
        
        if(attack)
        {
            //Debug.Log("Inimigo1 atacando!");
            enemyAttackAnim();
            //attack = !attack;
        }
            

        properFlip();
    }

    public void TakeDamage(int damage)
    {
        tomandoDano = true;
        currentHealth -= damage;

        // animacao de machucar
        Debug.Log("Vida atual = " + currentHealth);

        animator.SetTrigger("Inimigo1Damage");

        if(currentHealth <= 0)
        {
            Die();
        }

    }

    public void ResetTakeDamage()
    {
        tomandoDano = false;
    }

    void Die()
    {
        Debug.Log("Enemy "+ this.name + " died!");

        animator.SetBool("Dead", true);

    }

    void Jump()
    {
        if (noChao && Pulou)
        {
            //transform.position = transform.position + vertical * (Mathf.Clamp((Time.del
            rigidbody2D.velocity = Vector2.up * Speed;
            //Debug.Log("Pulou!");
            animator.SetFloat("Jump", Speed);
        }
    }

    void playerStopJumpAnim()
    {
        // 
        if(noChao && rigidbody2D.velocity.y <0.1f)
        {
            animator.SetFloat("Jump", 0f);
        }
    }

    public bool isGrounded()
    {
        float TestLength = 0.1f, ajusteFlipRay;

        if(olhandoDireita)
        {
            ajusteFlipRay = 0.3f;
        }
        else
        {
            ajusteFlipRay = 0f;
        }

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

    void Move()
    {

        Vector3 horizontal = new Vector3(horizontalMove, 0.0f, 0.0f);

        transform.position = transform.position + (((horizontal * Time.deltaTime * Speed)));

        Vector3 vertical = new Vector3(0.0f, verticalMove, 0.0f);

        if (horizontalMove > 0.1f || horizontalMove < -0.1f)  // direita
        {
            animator.SetFloat("Speed",  Speed);
        }
        else
        {
            animator.SetFloat("Speed", 0f);
        }

        //Debug.Log("speed do inimigo animator = " + horizontalMove);
        properFlip();

    }
    void enemyAttackAnim()
    {

        animator.SetTrigger("AttackTrigger");
        //enemyAttackCollDetection();
        //attack = ! attack;
                                 
    }

    void enemyAttackCollDetection()
    {
        Debug.Log("Iniciando Detecçao de collider de ataque do inimigo1");
        Collider2D[] hitEnemies =  Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayers);
        Collider2D player = hitEnemies[0];

        foreach(Collider2D enemy in hitEnemies)
        {
            if(enemy.name == "Player")
                player = enemy;

            Debug.Log("We hit "+ enemy.name);
            Debug.Log("Tipo do inimigo = " + enemy.GetType());
            System.Type i = enemy.GetType();
            
        }

        player.GetComponent<Player1>().TakeDamage(attackDamage);
    }

    void OnDrawGizmosSelected()
    {  
        if(!attackPoint)
            return;
        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }



}

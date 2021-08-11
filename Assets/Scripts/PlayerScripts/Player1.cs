using UnityEngine;
using UnityEngine.SceneManagement;

public class Player1 : MonoBehaviour
{
    // Start is called before the first frame update
    public float Speed;
    public int maxHealth=100;
    public int currentHealth;
    public bool olhandoDireita;
    public Animator animator;
    private float horizontalMove;
    private float verticalMove;
    private bool noChao;
    private bool dead;
    private bool Pulou;

    [SerializeField]
    private LayerMask groundLayer;
    private BoxCollider2D boxCollider2D;
    private Rigidbody2D rigidbody2D;

    public LayerMask GroundLayer 
    { 
        get
        {
            return groundLayer;
        }
    }

    void Start()
    {
        animator = GetComponent<Animator>();
        boxCollider2D = GetComponent<BoxCollider2D>();
        rigidbody2D = GetComponent<Rigidbody2D>();
        noChao = isGrounded();
        Pulou = false;
        currentHealth = maxHealth;
        dead = false;
    }

    void Update()
    {
        if (!dead)
        {
            horizontalMove = Input.GetAxisRaw("Horizontal");
            verticalMove   = Input.GetAxisRaw("Vertical");
            noChao = isGrounded();

            if((Input.GetKeyDown(KeyCode.UpArrow)|| Input.GetKeyDown(KeyCode.Space)))
            {
                Pulou = true;
                playerJump();

            }
            else if(rigidbody2D.velocity.y < 0)
            {
                animator.SetBool("Falling", true);
            }
            else
            {
                playerStopJumpAnim();
                Pulou = false;
            }
        } else {
            SceneManager.LoadScene("GameOver");
        }    
    }

    void FixedUpdate()
    {
        if(dead) {
            SceneManager.LoadScene("GameOver");
            return;
        }

        Vector3 horizontal = new Vector3(horizontalMove, 0.0f, 0.0f);

        transform.position = transform.position + (((horizontal * Time.deltaTime * Speed)));

        Vector3 vertical = new Vector3(0.0f, verticalMove, 0.0f);

        if (horizontalMove > 0.5f)  // direita
        {

            animator.SetFloat("Speed",  Speed);

        }
        else if (horizontalMove < -0.5f)   // esquerda
        {

            animator.SetFloat("Speed", Speed);

        }
        else
        {
            animator.SetFloat("Speed", 0f);
        }

        properFlip();


    }

    void playerJump()
    {

        if (noChao && Pulou)
        {
            rigidbody2D.velocity = Vector2.up * (Speed*1.2f);
            animator.SetFloat("Jump", Speed);
            
        }

    }

    void playerStopJumpAnim()
    {
        if(noChao && rigidbody2D.velocity.y <0.1f)
        {
            animator.SetFloat("Jump", 0f);
            animator.SetBool("Falling", false);
        }
    }

    public bool isGroundedBox()
    {

        float TestLength = 0.1f;
        RaycastHit2D hit2D = Physics2D.BoxCast((boxCollider2D.bounds.center),(boxCollider2D.bounds.size), 0,Vector2.down, TestLength, groundLayer);
        RaycastHit2D hit2DSingle = Physics2D.Raycast(new Vector2(boxCollider2D.bounds.min.x , boxCollider2D.bounds.min.y - TestLength), Vector2.right, boxCollider2D.bounds.extents.x);
        Color RayColor;

        if(hit2D.collider != null)
        {
            RayColor = Color.green;
        }
        else
        {
            RayColor = Color.red;
        }
        
        Debug.DrawRay(boxCollider2D.bounds.center + new Vector3(boxCollider2D.bounds.extents.x, 0, 0), Vector2.down * (boxCollider2D.bounds.extents.y + TestLength), RayColor);
        Debug.DrawRay(boxCollider2D.bounds.center - new Vector3(boxCollider2D.bounds.extents.x, 0, 0), Vector2.down * (boxCollider2D.bounds.extents.y + TestLength), RayColor);
        Debug.DrawRay(boxCollider2D.bounds.center - new Vector3(boxCollider2D.bounds.extents.x, boxCollider2D.bounds.extents.y + TestLength, 0), Vector2.right * (boxCollider2D.bounds.extents.x*2), RayColor);

        return hit2D.collider != null;
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
        Debug.DrawRay(Start,Direction,SingleRayColor);

        return hit2DSingle.collider != null;

    }

    void properFlip()
    {
        if((Input.GetAxis("Horizontal") < 0 && olhandoDireita) || (Input.GetAxis("Horizontal") > 0 && !olhandoDireita))
        {
            olhandoDireita = !olhandoDireita;
            transform.Rotate(new Vector3(0, 180, 0));
        }
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;

        // animacao de machucar
        Debug.Log("Vida atual = " + currentHealth);

        animator.SetTrigger("Player1Damage");

        if(currentHealth <= 0)
        {
            Die();
        }

    }

    public bool isDie() {
        return dead;
    }
    void Die()
    {
        dead = true;
        Debug.Log("Enemy "+ this.name + " died!");

        
        animator.SetBool("Dead", true);

        //Destroy(this);
    }

    public void destroyPlayer()
    {
        // Avisar de alguma maneira o gamemanager que o jogo acabou!
        //DestroyImmediate(animator);

        //Destroy(this);


    }

}

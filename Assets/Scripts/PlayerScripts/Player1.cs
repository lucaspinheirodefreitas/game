using UnityEngine;
using UnityEngine.SceneManagement;

public class Player1 : MonoBehaviour
{
    // Start is called before the first frame update
    public float Speed;
    public int maxHealth=100;
    public int currentHealth;
    public bool olhandoDireita;
    public Vector3[] posicao;

    public Animator animator;
    public GameManager gameManager;
    private float horizontalMove;
    private float verticalMove;
    private bool noChao;
    private bool dead;
    private bool Pulou;
    private bool caiu;

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
        gameManager = GetComponent<GameManager>();
        noChao = isGrounded();
        Pulou = false;
        currentHealth = maxHealth;
        dead = false;
        caiu = !verificacaoSolo();
    }

    void Update()
    {
        if (!dead && !caiu)
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
            GameManager.Instance.GameOver=true;
            SceneManager.LoadScene("GameOver");
        }    
    }

    void FixedUpdate()
    {
        caiu = !verificacaoSolo();
        if(dead || caiu) {
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


    // verifica se caiu num abismo
    private bool verificacaoSolo() {
        float distancia = 50.0f;
        RaycastHit2D soloEsq = Raycast(new Vector2(posicao[0].x, posicao[0].y), new Vector2(-1,-1), distancia, groundLayer);
        RaycastHit2D soloDir = Raycast(new Vector2(posicao[1].x, posicao[1].y), new Vector2(1,-1), distancia, groundLayer);
        RaycastHit2D soloEsqDuplo = Raycast(new Vector2(posicao[2].x, posicao[2].y), Vector2.down, distancia, groundLayer);
        RaycastHit2D soloDirDuplo = Raycast(new Vector2(posicao[3].x, posicao[3].y), Vector2.down, distancia, groundLayer);

        return ((soloDir || soloDirDuplo || soloEsq || soloEsqDuplo));
    }

    // se detectar objeto com a mascara informada e com a distantia informada retorna raio verde, caso contrario retorna raio vermelho.
    private RaycastHit2D Raycast(Vector3 origem, Vector2 direcaoRaio, float distanciaSolo, LayerMask mask) {
        Vector3 posicaoAtual = transform.position;
        RaycastHit2D hit = Physics2D.Raycast(posicaoAtual + origem, direcaoRaio, distanciaSolo, mask);
        Color corRaio = hit ? Color.green : Color.red;

        Debug.DrawRay(posicaoAtual + origem, direcaoRaio*distanciaSolo, corRaio);

        return hit;
    }

    public void destroyPlayer()
    {
        // Avisar de alguma maneira o gamemanager que o jogo acabou!
        //DestroyImmediate(animator);

        //Destroy(this);


    }

}

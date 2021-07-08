using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player1 : MonoBehaviour
{
    // Start is called before the first frame update
    public float Speed;
    public bool olhandoDireita;
    public Animator animator;

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
        //animator.SetBool("CaindoEsquerda", false);
    }


    // Update is called once per frame
    void Update()
    {
        //animator.SetFloat("Horizontal", Input.GetAxis("Horizontal"));
        //animator.SetFloat("Vertical", Input.GetAxis("Vertical"));
        //animator.SetBool("CaindoEsquerda", false);

        Vector3 horizontal = new Vector3(Input.GetAxis("Horizontal"), 0.0f, 0.0f);

        transform.position = transform.position + (((horizontal * Time.deltaTime * Speed)));

        Vector3 vertical = new Vector3(0.0f, Input.GetAxis("Vertical"), 0.0f);

        if (Input.GetAxis("Horizontal") > 0.5f)  // direita
        {

            animator.SetFloat("Speed",  Speed);

        }
        else if (Input.GetAxis("Horizontal") < -0.5f)   // esquerda
        {

            animator.SetFloat("Speed", Speed);

        }
        else
        {
            animator.SetFloat("Speed", 0f);
        }

        properFlip();

        if (isGrounded() && (Input.GetKeyDown(KeyCode.UpArrow)|| Input.GetKeyDown(KeyCode.Space)))
        {
            
            //transform.position = transform.position + vertical * (Mathf.Clamp((Time.deltaTime * Speed), 0, Speed));
            rigidbody2D.velocity = Vector2.up * Speed;
            //Debug.Log("Pulou!");
            animator.SetFloat("Jump", Speed);
        }
        else if(isGrounded() && rigidbody2D.velocity.y <0.8f)
        {
            animator.SetFloat("Jump", 0f);
        }


        //transform.rotation = new Quaternion(0.0f,0.0f,0.0f, 0.0f);
        
        //Debug.Log("Velocidade Horizontal = " + rigidbody2D.velocity.x);
        //Debug.Log("Velocidade Vertical = " + rigidbody2D.velocity.y);

        //Debug.Log("Horizontal = " + horizontal);
        //Debug.Log("Vertical = " + vertical);

    }

    public bool isGrounded()
    {

        float TestLength = 0.1f;
        RaycastHit2D hit2D = Physics2D.BoxCast((boxCollider2D.bounds.center),(boxCollider2D.bounds.size), 0,Vector2.down, TestLength, groundLayer);
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

        //Debug.Log(hit2D.distance);

        //Debug.Log(hit2D.collider.tag);
        if(hit2D.collider != null)
        {
            //Debug.Log("Ta no chao!");
        }
        else
        {
            //Debug.Log("Nao esta no chao!");
        }

        return hit2D.collider != null;
    }

    void properFlip()
    {
        if((Input.GetAxis("Horizontal") < 0 && olhandoDireita) || (Input.GetAxis("Horizontal") > 0 && !olhandoDireita))
        {
            Debug.Log("Vai flipar");
            Debug.Log("Rotaçao inicial = " + transform.rotation);
            Debug.Log("Flipou!");
            olhandoDireita = !olhandoDireita;
            transform.Rotate(new Vector3(0, 180, 0));
            Debug.Log("Rotaçao final = " + transform.rotation);

        }
    }

}

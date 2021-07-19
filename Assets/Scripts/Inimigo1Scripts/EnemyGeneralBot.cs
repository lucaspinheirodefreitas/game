using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyGeneralBot : MonoBehaviour
{
    // Start is called before the first frame update

    private BoxCollider2D boxCollider2D;
    public GameObject enemy;
    public float visionRange=10f;
    public LayerMask player;
    private bool olhandoDireita;
    Inimigo1Script inimigo1Script;

    private float ajusteDirecao=1;
    void Start()
    {
        boxCollider2D = GetComponent<BoxCollider2D>();
        inimigo1Script = enemy.GetComponent<Inimigo1Script>();
        
    }

    // Update is called once per frame
    void Update()
    {
        
        olhandoDireita = inimigo1Script.olhandoDireita;
        
    }

    void FixedUpdate()
    {
        int detectPlayer = DetectPlayer();

        if(detectPlayer != 0)
        {
            Debug.Log("Player detectado!");
            inimigo1Script.horizontalMove = 1*ajusteDirecao*detectPlayer;
            //Debug.Log("Horizontal move = " + inimigo1Script.horizontalMove);
        }
        else
        {
            Debug.Log("Player nao detectado");
            inimigo1Script.horizontalMove = 0;
            Debug.Log("Horizontal move = " + inimigo1Script.horizontalMove);
        }

    }

    int DetectPlayer()
    {
        float TestLength = 0.1f, ajusteFlipRay=0;

        if(olhandoDireita)
        {
            ajusteDirecao = 1;
        }
        else
        {
            ajusteDirecao = -1;
        }

        Vector2 Start =  new Vector2(boxCollider2D.bounds.min.x + boxCollider2D.bounds.extents.x + TestLength + ajusteFlipRay, boxCollider2D.bounds.min.y + boxCollider2D.bounds.extents.y - TestLength);
        Vector2 Direction = new Vector2((boxCollider2D.bounds.extents.x)*visionRange*ajusteDirecao, 0);

        RaycastHit2D hit2DSingleFront = Physics2D.Raycast(Start, Direction, Direction.magnitude, (player));
        RaycastHit2D hit2DSingleBack = Physics2D.Raycast(Start, Direction*-1, Direction.magnitude, (player));

        Color SingleRayColor;

        SingleRayColor = Color.magenta;

        if(hit2DSingleFront.collider != null || hit2DSingleBack.collider != null)
        {
            SingleRayColor = Color.green;
        }
        else
        {
            SingleRayColor = Color.red;
        }

        Debug.DrawRay(Start,Direction,SingleRayColor);
        Debug.DrawRay(Start,Direction*-1,SingleRayColor);

        if (hit2DSingleFront.collider != null)
        {
            return 1;
        }
        else if(hit2DSingleBack.collider != null)
        {
            return -1;
        }
        

        return 0;
    }
}

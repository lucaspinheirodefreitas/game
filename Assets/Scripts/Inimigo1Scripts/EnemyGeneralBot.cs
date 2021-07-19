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

    public Transform attackPoint;
    public LayerMask enemyLayers;
    public float attackRange=0.5f;

    private float ajusteDirecao=1;
    void Start()
    {
        boxCollider2D = GetComponent<BoxCollider2D>();
        inimigo1Script = enemy.GetComponent<Inimigo1Script>();

        attackPoint = inimigo1Script.attackPoint;
        enemyLayers = inimigo1Script.enemyLayers;
        attackRange = inimigo1Script.attackRange;
        
    }

    // Update is called once per frame
    void Update()
    {
        if(inimigo1Script.animator.GetBool("Dead"))
        {
            enemy.layer = LayerMask.NameToLayer("ChaoTiles");;
            //Object.Destroy(inimigo1Script);
            Object.Destroy(this);
        }

        olhandoDireita = inimigo1Script.olhandoDireita;
        
    }

    void FixedUpdate()
    {
        int detectPlayer = DetectPlayer();

        bool tomandoDano = inimigo1Script.tomandoDano;

        if(detectPlayer != 0 && tomandoDano == false)
        {

            if(DetectAttackRange())
            {
                inimigo1Script.attack = true;
                inimigo1Script.horizontalMove = 0;
                inimigo1Script.verticalMove = 0;

            }
            else
            {
                inimigo1Script.attack = false;
                //Debug.Log("Player detectado!");
                inimigo1Script.horizontalMove = 1*ajusteDirecao*detectPlayer;
                //Debug.Log("Horizontal move = " + inimigo1Script.horizontalMove);

            }

        }
        else
        {
            inimigo1Script.attack = false;
            //Debug.Log("Player nao detectado");
            inimigo1Script.horizontalMove = 0;
            //Debug.Log("Horizontal move = " + inimigo1Script.horizontalMove);
        }

    }

    bool DetectAttackRange()
    {

        Collider2D[] hitEnemies =  Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayers);

        foreach(Collider2D enemy in hitEnemies)
        {
            if (enemy != null)
            {
                //Debug.Log(enemy.name);
                return true;
            }
                
        }

        return false;
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

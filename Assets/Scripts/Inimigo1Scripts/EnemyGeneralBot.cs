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


    /* IA */
    RaycastHit2D soloFrente;
    RaycastHit2D soloTras;
    RaycastHit2D soloEsq;
    RaycastHit2D soloDir;
    RaycastHit2D soloEsqDuplo;
    RaycastHit2D soloDirDuplo;
    RaycastHit2D obstaculoDireita;
    RaycastHit2D obstaculoEsquerda;
    public LayerMask solo;
    public bool temSolo;
    public bool temSoloEsq;
    public bool temSoloDir;
    public bool temObstaculoFrente;
    public bool temObstaculoTras;
    public Vector3[] posicao;
    public float distanciaSolo;
    public float distanciaSoloCentro;
    float tempoDeEspera;
    bool direita;
    bool esquerda;
    int direcaoAnterior;
    int direcao;
    bool start;
    float tempoProcessamento;

    private int ajusteDirecao=1;
    void Start()
    {
        boxCollider2D = GetComponent<BoxCollider2D>();
        inimigo1Script = enemy.GetComponent<Inimigo1Script>();

        attackPoint = inimigo1Script.attackPoint;
        enemyLayers = inimigo1Script.enemyLayers;
        attackRange = inimigo1Script.attackRange;
        direcao = 0;
        start = true;
    }

    // Update is called once per frame
    void Update()
    {
        if(inimigo1Script.animator.GetBool("Dead"))
        {
            enemy.layer = LayerMask.NameToLayer("ChaoTiles");;
            Object.Destroy(inimigo1Script);
            Object.Destroy(this);
        }

        olhandoDireita = inimigo1Script.olhandoDireita;
        
    }

    void FixedUpdate() {
        int detectPlayer = DetectPlayer();
        bool tomandoDano = inimigo1Script.tomandoDano;
        inimigo1Script.noChao = false;


        if(detectPlayer != 0) {
            if(!tomandoDano /*&& player1.currentHealth > 0*/) {

                if(DetectAttackRange()) {  

                    inimigo1Script.attack = true;
                    direcao = 0;
                    start = true;
                    inimigo1Script.horizontalMove = direcao;
                    inimigo1Script.verticalMove = direcao;
                }
                else {
                    inimigo1Script.attack = false;
                    direcao = ajusteDirecao*detectPlayer; 
                    if(verificacaoObstaculo(direcao)) {
                        pularObstaculo();
                    }
                }
            }
        } else {
            if (verificarEstaSolo() && start) {
                direcao = Random.Range(0, 2);
                direcao = direcao > 0 ? 1 : -1;
                start = false;
            } else if(verificacaoSolo(direcao)) {
                if(verificacaoObstaculo(direcao)) {
                    pularObstaculo();
                }
            } else {
                direcao *= -1;
            } 
            inimigo1Script.attack = false;
            inimigo1Script.horizontalMove = direcao;
        }  

        //tempoProcessamento = Mathf.Clamp(tempoProcessamento - Time.fixedDeltaTime, 0, Mathf.Infinity);

    }

    bool DetectAttackRange()
    {

        Collider2D[] hitEnemies =  Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayers);

        foreach(Collider2D enemy in hitEnemies)
        {
            if (enemy != null)
            {
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

    private bool verificacaoSolo(int direcao) {
        temSoloEsq = temSoloDir = false;
        soloEsq = Raycast(new Vector2(posicao[0].x, posicao[0].y), Vector2.down, distanciaSolo, solo);
        soloDir = Raycast(new Vector2(posicao[1].x, posicao[1].y), Vector2.down, distanciaSolo, solo);
        soloEsqDuplo = Raycast(new Vector2(posicao[2].x, posicao[2].y), Vector2.down, distanciaSolo, solo);
        soloDirDuplo = Raycast(new Vector2(posicao[3].x, posicao[3].y), Vector2.down, distanciaSolo, solo);
        
        if (soloEsq || soloEsqDuplo) {
            temSoloEsq = true;
        } 
        if (soloDir || soloDirDuplo) { 
            temSoloDir = true;
        }

        return (temSoloDir && direcao>0) || (temSoloEsq && direcao<0);
    }

    private bool verificarEstaSolo() {
        temSolo = false; // TODO remover variavel
        soloFrente = Raycast(new Vector2(posicao[4].x, posicao[4].y), Vector2.down, distanciaSoloCentro, solo);
        soloTras = Raycast(new Vector2(posicao[5].x, posicao[5].y), Vector2.down, distanciaSoloCentro, solo);

        if (soloFrente || soloTras) {
            temSolo = true;
        }
        return soloFrente || soloTras;
    }

    private bool verificacaoObstaculo(int direcao) {
        temObstaculoFrente = temObstaculoTras = false;
        obstaculoDireita = Raycast(new Vector2(posicao[6].x, posicao[6].y), Vector2.right, 0.6f, solo);
        obstaculoEsquerda = Raycast(new Vector2(posicao[7].x, posicao[7].y), Vector2.left, 0.6f, solo);

        if (obstaculoDireita) { 
            temObstaculoFrente = true;
        }
        if (obstaculoEsquerda) {
            temObstaculoTras = true;
        }
        return temObstaculoFrente && direcao>0 || temObstaculoTras && direcao<0;
    }

    private void pularObstaculo() {
        inimigo1Script.verticalMove = 1;
        inimigo1Script.Pulou = true;
        inimigo1Script.noChao = true;
        inimigo1Script.Jump();
        inimigo1Script.noChao = false;
    }
    private RaycastHit2D Raycast(Vector3 origem, Vector2 direcaoRaio, float distanciaSolo, LayerMask mask) {
        Vector3 posicaoAtual = transform.position;
        RaycastHit2D hit = Physics2D.Raycast(posicaoAtual + origem, direcaoRaio, distanciaSolo, mask);
        Color corRaio = hit ? Color.green : Color.red;

        Debug.DrawRay(posicaoAtual + origem, direcaoRaio*distanciaSolo, corRaio);

        return hit;
    }
}

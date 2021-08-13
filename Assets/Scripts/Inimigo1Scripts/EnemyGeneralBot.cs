using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyGeneralBot : MonoBehaviour
{
    // Start is called before the first frame update

    private BoxCollider2D boxCollider2D;
    public GameObject enemy;
    public GameObject playerOponente;
    public float visionRange=10f;
    public LayerMask player;
    Inimigo1Script inimigo1Script;

    public Transform attackPoint;
    public LayerMask enemyLayers;
    public float attackRange=0.5f;


    /* IA */
    Player1 oponente;
    RaycastHit2D soloFrente;
    RaycastHit2D soloTras;
    RaycastHit2D soloEsq;
    RaycastHit2D soloDir;
    RaycastHit2D soloEsqDuplo;
    RaycastHit2D soloDirDuplo;
    RaycastHit2D atacanteDireita;
    RaycastHit2D atacanteEsquerda;
    public LayerMask solo;
    public LayerMask atacante;
    public Vector3[] posicao;
    public float distanciaSolo;
    public float distanciaSoloCentro;
    float tempoDeEspera;
    int direcao;
    bool start;
    private int ajusteDirecao;
    float tempoProcessamento;
    public float distanciaObstaculo;
    public float distanciaAtacante;
    void Start()
    {
        boxCollider2D = GetComponent<BoxCollider2D>();
        inimigo1Script = enemy.GetComponent<Inimigo1Script>();
        oponente = playerOponente.GetComponent<Player1>();

        attackPoint = inimigo1Script.attackPoint;
        enemyLayers = inimigo1Script.enemyLayers;
        attackRange = inimigo1Script.attackRange;
        direcao = 0;
        start = true;
        ajusteDirecao = 1;
        tempoProcessamento = 0.5f;
    }
    void FixedUpdate() {
        if(inimigo1Script.animator.GetBool("Dead")) {
            enemy.layer = LayerMask.NameToLayer("ChaoTiles");;
            Object.Destroy(inimigo1Script);
            Object.Destroy(this);
        }

        int detectPlayer = DetectPlayer();
    
        if (inimigo1Script.attack == false)
        {
            if(detectPlayer != 0) {
                if(tempoProcessamento==0 && !oponente.isDie()) {
                    if(DetectAttackRange()) {  
                        direcao = 0;
                        inimigo1Script.enemyAttackAnim();
                        tempoProcessamento=0.5f;
                        start = true;
                    }
                    else {
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
                } else if (verificacaoAtacante(direcao)){
                    direcao *= -1;
                } else if(verificacaoSolo(direcao)) {
                    if(verificacaoObstaculo(direcao)) {
                        pularObstaculo();
                    }
                } else {
                    direcao *= -1;
                } 
            }
            inimigo1Script.Move((float) direcao);
            inimigo1Script.properFlip(direcao);
            tempoProcessamento = Mathf.Clamp(tempoProcessamento - Time.fixedDeltaTime, 0, Mathf.Infinity);
        }
    }

    bool DetectAttackRange()
    {

        Collider2D[] hitEnemies =  Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayers);

        foreach(Collider2D enemy in hitEnemies)  {
            if (enemy != null)
                return true;    
        }
        return false;
    }

    int DetectPlayer()
    {
        float TestLength = 0.1f, ajusteFlipRay=0;

        if(inimigo1Script.olhandoDireita) 
            ajusteDirecao = 1;
        else 
            ajusteDirecao = -1;

        Vector2 Start =  new Vector2(boxCollider2D.bounds.min.x + boxCollider2D.bounds.extents.x + TestLength + ajusteFlipRay, boxCollider2D.bounds.min.y + boxCollider2D.bounds.extents.y - TestLength);
        Vector2 Direction = new Vector2((boxCollider2D.bounds.extents.x)*visionRange*ajusteDirecao, 0);

        RaycastHit2D hit2DSingleFront = Physics2D.Raycast(Start, Direction, Direction.magnitude, (player));
        RaycastHit2D hit2DSingleBack = Physics2D.Raycast(Start, Direction*-1, Direction.magnitude, (player));
        RaycastHit2D hit2DBoxFront = Physics2D.BoxCast(Start, new Vector2(3, 5), 0, Direction, player);

        Color SingleRayColor;

        SingleRayColor = Color.magenta;

        if(hit2DSingleFront.collider != null || hit2DSingleBack.collider != null)
            SingleRayColor = Color.blue;
        else
            SingleRayColor = Color.yellow;

        Debug.DrawRay(Start,Direction,SingleRayColor);
        Debug.DrawRay(Start,Direction*-1,SingleRayColor);

        if (hit2DSingleFront.collider != null || hit2DBoxFront.collider != null)
            return 1;
        else if(hit2DSingleBack.collider != null)
            return -1;
        return 0;
    }

    // verifica se tem solo ou se está proximo a um abismo
    private bool verificacaoSolo(int direcao) {
        soloEsq = Raycast(new Vector2(posicao[0].x, posicao[0].y), Vector2.down, distanciaSolo, solo);
        soloDir = Raycast(new Vector2(posicao[1].x, posicao[1].y), Vector2.down, distanciaSolo, solo);
        soloEsqDuplo = Raycast(new Vector2(posicao[2].x, posicao[2].y), Vector2.down, distanciaSolo, solo);
        soloDirDuplo = Raycast(new Vector2(posicao[3].x, posicao[3].y), Vector2.down, distanciaSolo, solo);

        return ((soloDir || soloDirDuplo) && direcao>0) || ((soloEsq || soloEsqDuplo) && direcao<0);
    }

    // verifica se está proximo a tocar o solo
    private bool verificarEstaSolo() {
        soloFrente = Raycast(new Vector2(posicao[4].x, posicao[4].y), Vector2.down, distanciaSoloCentro, solo);
        soloTras = Raycast(new Vector2(posicao[5].x, posicao[5].y), Vector2.down, distanciaSoloCentro, solo);

        return soloFrente || soloTras;
    }

    // verifica se tem algum obstaculo proximo 
    private bool verificacaoObstaculo(int direcao) {
        RaycastHit2D obstaculoDireitaCentro = Raycast(new Vector2(posicao[6].x, posicao[6].y), Vector2.right, distanciaObstaculo, solo);
        RaycastHit2D obstaculoDireitaCima = Raycast(new Vector2(posicao[6].x, posicao[6].y), new Vector2(1, 1), distanciaObstaculo, solo);
        RaycastHit2D obstaculoDireitaBaixo = Raycast(new Vector2(posicao[6].x, posicao[6].y), new Vector2(1, -1), distanciaObstaculo, solo);
        RaycastHit2D obstaculoEsquerdaCentro = Raycast(new Vector2(posicao[7].x, posicao[7].y), Vector2.left, distanciaObstaculo, solo);            
        RaycastHit2D obstaculoEsquerdaBaixo = Raycast(new Vector2(posicao[7].x, posicao[7].y), new Vector2(-1, -1), distanciaObstaculo, solo);
        RaycastHit2D obstaculoEsquerdaCima = Raycast(new Vector2(posicao[7].x, posicao[7].y), new Vector2(-1, 1), distanciaObstaculo, solo);


        return ((obstaculoDireitaCentro || obstaculoDireitaBaixo || obstaculoDireitaCima) && direcao>0) || 
            ((obstaculoEsquerdaCentro || obstaculoEsquerdaBaixo || obstaculoEsquerdaCima) && direcao<0);
    }

    // verifica se tem algum outro atacante ao player proximo
    private bool verificacaoAtacante(int direcao) {
        RaycastHit2D atacanteDireitaCentro = Raycast(new Vector2(posicao[8].x, posicao[8].y), Vector2.right, distanciaAtacante, atacante);
        RaycastHit2D atacanteDireitaCima = Raycast(new Vector2(posicao[8].x, posicao[8].y), new Vector2(1, 1), distanciaAtacante, atacante);
        RaycastHit2D atacanteDireitaBaixo = Raycast(new Vector2(posicao[8].x, posicao[8].y), new Vector2(1, -1), distanciaAtacante, atacante);
        RaycastHit2D atacanteEsquerdaCentro = Raycast(new Vector2(posicao[9].x, posicao[9].y), Vector2.left, distanciaAtacante, atacante);            
        RaycastHit2D atacanteEsquerdaBaixo = Raycast(new Vector2(posicao[9].x, posicao[9].y), new Vector2(-1, -1), distanciaAtacante, atacante);
        RaycastHit2D atacanteEsquerdaCima = Raycast(new Vector2(posicao[9].x, posicao[9].y), new Vector2(-1, 1), distanciaAtacante, atacante);
        
        return ((atacanteDireitaCentro || atacanteDireitaBaixo || atacanteDireitaCima) && direcao>0) || 
            ((atacanteEsquerdaCentro || atacanteEsquerdaBaixo || atacanteEsquerdaCima) && direcao<0);
    }

    // pula obstaculo
    private void pularObstaculo() {
        inimigo1Script.Pulou = true;
        inimigo1Script.noChao = true;
        inimigo1Script.Jump();
        inimigo1Script.noChao = false;
    }

    // se detectar objeto com a mascara informada e com a distantia informada retorna raio verde, caso contrario retorna raio vermelho.
    private RaycastHit2D Raycast(Vector3 origem, Vector2 direcaoRaio, float distanciaSolo, LayerMask mask) {
        Vector3 posicaoAtual = transform.position;
        RaycastHit2D hit = Physics2D.Raycast(posicaoAtual + origem, direcaoRaio, distanciaSolo, mask);
        Color corRaio = hit ? Color.green : Color.red;

        Debug.DrawRay(posicaoAtual + origem, direcaoRaio*distanciaSolo, corRaio);

        return hit;
    }
}

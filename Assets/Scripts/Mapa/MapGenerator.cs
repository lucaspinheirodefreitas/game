using UnityEngine;
using UnityEngine.SceneManagement;

public class MapGenerator : MonoBehaviour
{
    [SerializeField] int width, height;
    [SerializeField] GameObject dirt, grass, inimigo1, inimigoKnight, player;
    Vector3 vetor;
    void Start()
    {
        Generation();
    }

    void Generation()
    {
        int y = 0;
        for (int x = 0; x < width; x++)
        {
            
            int minHeight = height - 1;
            int maxHeight = height + 2;
            height = Random.Range(minHeight, maxHeight);

            spawnObj(dirt, x + y, height - 1);
            spawnObj(grass, x + y, height);
            vetor = new Vector3(x + y, height, 0.0f);
            y += 2;
            int r = Random.Range(1, 10);
            if(r == 1)
            {
                y += 2;
            }

            if (r == 2)
            {
                y += 3;
            }

            if (r == 2)
            {
                y += 4;
            }

        }
        
    }

    void spawnObj(GameObject obj, int width, int height) {

        obj = Instantiate(obj, new Vector2(width, height), Quaternion.identity);
        obj.transform.parent = this.transform;

        int r = Random.Range(1, 100);
        if (r == 1 || r == 2 || r == 3)
        {
            inimigo1 = Instantiate(inimigo1, new Vector2(width, height + 1), Quaternion.identity);
        }

        if (r == 6)
        {
            inimigoKnight = Instantiate(inimigoKnight, new Vector2(width, height + 1), Quaternion.identity);
        }

    }


    void FixedUpdate()
    {
            if(Vector3.Distance(player.transform.position, vetor) < 3)
            {
                SceneManager.LoadScene("boss fight");
            }
    }
   
}
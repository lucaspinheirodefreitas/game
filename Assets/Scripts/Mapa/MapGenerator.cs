using UnityEngine;

public class MapGenerator : MonoBehaviour
{
    [SerializeField] int width, height;
    [SerializeField] GameObject dirt, grass, inimigo1, inimigoKnight;
    void Start()
    {
        Generation();
    }

    void Generation()
    {
        int y = 0;
        int x = 0;
        for (x=0; x < width; x++)
        {
            
            int minHeight = height - 1;
            int maxHeight = height + 2;
            height = Random.Range(minHeight, maxHeight);

            spawnObj(dirt, x + y, height - 1);
            spawnObj(grass, x + y, height);
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
        //spawnObj(dirt, x + y, height - 1);
        //spawnObj(grass, x + y, height);
        
    }

    void spawnObj(GameObject obj, int width, int height) {

        obj = Instantiate(obj, new Vector2(width, height), Quaternion.identity);
        obj.transform.parent = this.transform;

        int r = Random.Range(1, 100);
        if (r == 1 || r == 2 || r == 3 || r == 4 || r == 5)
        {
            inimigo1 = Instantiate(inimigo1, new Vector2(width, height + 2), Quaternion.identity);
        }

        if (r == 6)
        {
            inimigoKnight = Instantiate(inimigoKnight, new Vector2(width, height + 2), Quaternion.identity);
        }

    }

   
}
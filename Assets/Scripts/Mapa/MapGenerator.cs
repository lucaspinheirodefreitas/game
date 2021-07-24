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
        for (int x = 0; x < width; x++)//This will help spawn a tile on the x axis
        {
            // now for procedural generation we need to gradually increase and decrease the height value
            int minHeight = height - 1;
            int maxHeight = height + 2;
            height = Random.Range(minHeight, maxHeight);


            
            spawnObj(dirt, x + y, height - 1);
            spawnObj(grass, x + y, height);
            y += 2;
        }

        //for (int i = 0; i < 4; i++)
        //{
            //int xz = Random.Range(0, width);
            //inimigo1 = Instantiate(inimigo1, new Vector2(xz, height + 2), Quaternion.identity);
        //}


        //for (int i = 0; i < 2; i++)
        //{
            //int xy = Random.Range(0, width);
            //inimigo1 = Instantiate(inimigo1, new Vector2(xy, height + 2), Quaternion.identity);
        //}
    }

    void spawnObj(GameObject obj, int width, int height) {

        obj = Instantiate(obj, new Vector2(width, height), Quaternion.identity);
        obj.transform.parent = this.transform;

    }

   
}
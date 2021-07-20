using UnityEngine;

public class MapGenerator : MonoBehaviour
{
    [SerializeField] int width, height;
    [SerializeField] GameObject dirt, grass;
    void Start()
    {
        Generation();
    }

    void Generation()
    {
        for (int x = 0; x < width; x++)//This will help spawn a tile on the x axis
        {
            // now for procedural generation we need to gradually increase and decrease the height value
            int minHeight = height - 1;
            int maxHeight = height + 2;
            height = Random.Range(minHeight, maxHeight);


            
            spawnObj(dirt, x, height - 1);
            spawnObj(grass, x, height);
        }
    }

    void spawnObj(GameObject obj, int width, int height)//What ever we spawn will be a child of our procedural generation gameObj
    {
        obj = Instantiate(obj, new Vector2(width, height), Quaternion.identity);
        obj.transform.parent = this.transform;
    }
}
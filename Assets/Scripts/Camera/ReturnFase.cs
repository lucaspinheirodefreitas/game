using UnityEngine;
using UnityEngine.SceneManagement;


public class ReturnFase : MonoBehaviour
{
    [SerializeField] GameObject player, lastTier;


    void FixedUpdate()
    {
        if (Vector3.Distance(player.transform.position, lastTier.transform.position) < 3)
        {
            SceneManager.LoadScene("Game");
        }
    }
}

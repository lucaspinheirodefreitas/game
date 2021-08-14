using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    // Start is called before the first frame update

    public Transform Jogador;
    public Vector3 offset;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // Camera follows the player with specified offset position
        transform.position = new Vector3(Jogador.position.x + offset.x, Jogador.position.y + offset.y, offset.z);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    // Start is called before the first frame update

    public Transform Jogador;
    public Vector3 offset;
    [SerializeField] GameObject bg;

    public float smoothSpeed = 0.125f;

    private float yCamera, yDesired, ySmoothed;


    void Start()
    {
        yCamera = Jogador.position.y;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        // Camera follows smoothly the player with specified offset position and smoothSpeed
        yDesired = Jogador.position.y; 

        yCamera = Mathf.Lerp(yCamera, yDesired,smoothSpeed);
            
        transform.position = new Vector3(Jogador.position.x + offset.x, yCamera + offset.y, offset.z);
        bg.transform.position = new Vector3(Jogador.position.x + offset.x, yCamera + offset.y, 2);
    }

}

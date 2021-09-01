using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    GameManager gameManager;
    public float aspectRatioDesign = (16f / 9f);
    public float closenessMult = .32f;


    void Start()
    {
        gameManager = GameManager.Instance;



        transform.position = new Vector3(gameManager.CamX, gameManager.CamY, transform.position.z);

        aspectRatioDesign = (float)Screen.height / (float)Screen.width;
      

        Camera.main.orthographicSize = aspectRatioDesign * (gameManager.GridWidth) * closenessMult;
    }

 
}

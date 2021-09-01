using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class InputManager : MonoBehaviour
{

    [SerializeField] GameController gameController;
    GameManager gameManager;
    Vector2 firstPos;
    Vector2 lastPos;
    Vector2 swipe;
    float dragDistance;

    private void Start()
    {
        gameManager = GameManager.Instance;

        dragDistance = Screen.height * 5 / 100;
    }

    void Update()
    {

        BackKey();

        if (!gameManager.IsGameOver && gameManager.IsGameStarted)
        {
            #if UNITY_EDITOR

                    Click();
                    RotateUnityEditor();
         
                            

            #else
                            
                    Swipe();

            #endif
        }

    }

    public void Swipe()
    {
      

        Touch touch = Input.GetTouch(0);

        if (gameController.isMapFull && !gameController.isRotating && gameController.canRotate)
        {

            if (touch.phase == TouchPhase.Began)
            {
                
                firstPos = touch.position;
            }
            if (touch.phase == TouchPhase.Ended)
            {
                lastPos = touch.position;

                swipe = new Vector2(lastPos.x - firstPos.x, lastPos.y - firstPos.y);

                if (Mathf.Abs(lastPos.x - firstPos.x) > dragDistance ||  Mathf.Abs(lastPos.y - firstPos.y) > dragDistance)
                {
                    
                   
                        if (swipe.x > 0)
                        {
                            StartCoroutine(gameController.RotateHexagons());
                        }
                        else
                        {
                            StartCoroutine(gameController.RotateHexagonsClockWise());
                        }
                    
                }
                else
                {
                    Vector2 mousePos = Camera.main.ScreenToWorldPoint(firstPos);
                    RaycastHit2D hit = Physics2D.Raycast(mousePos, Vector2.zero);

                    if (hit && hit.transform.TryGetComponent<Hexagon.Neighbour>(out Hexagon.Neighbour neighbour))
                    {

                        gameController.NeighboursOutline(neighbour.GetClosestNeighbours(mousePos));


                    }
                }
            }
        }
        
        
       
    }

    public void Click()
    {
        

        if (Input.GetMouseButtonDown(0) && gameController.isMapFull && !gameController.isRotating && gameController.canRotate)
        {
            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(mousePos, Vector2.zero);

            if (hit && hit.transform.TryGetComponent<Hexagon.Neighbour>(out Hexagon.Neighbour neighbour))
            {

                gameController.NeighboursOutline(neighbour.GetClosestNeighbours(mousePos));


            }

        }

       
    }

    public void RotateUnityEditor()
    {
        if (Input.GetMouseButtonDown(1) && !gameController.isRotating && gameController.canRotate)
        {
            StartCoroutine(gameController.RotateHexagons());

        }
    }

    public void PlayAgainButton()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void BackKey()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
        }
    }
}

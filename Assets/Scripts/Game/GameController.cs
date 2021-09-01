using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;


public class GameController : MonoBehaviour
{

    private int gridWidth = 8;
    private int gridHeight = 9;

    private float gridYOffset = .6f;
    private float gridXOffset = .5f;



    private int bombTime = 7;
    private int bombScore = 1000;
    public bool isMapFull = false;
    public bool isMatch = false;
    public bool canRotate = true;
    public bool isRotating = false;

    private int colorCount = 5;



    GameManager gameManager;

    [SerializeField] GameObject gameOverPanel;
    [SerializeField] GameObject knobPrefab;
    [SerializeField] GameObject hexPrefab;
    [SerializeField] ScoreController scoreController;
    [SerializeField] ParticleSystem particleSystem;

    private GameObject[,] mapMatris;
    private GameObject bombHexagon;
    private GameObject prevKnob;


    List<GameObject> prevSelection;
    List<GameObject> selection;
    List<Color> colorList;


    

    private void Start()
    {

        gameManager = GameManager.Instance;

        prevSelection = new List<GameObject>();
        selection = new List<GameObject>();

        gameManager.IsGameStarted = true;
        gameManager.IsGameOver = false;
        
        gridWidth = gameManager.GridWidth;
        gridHeight = gameManager.GridHeight;
        bombTime = gameManager.BombTime;
        bombScore = gameManager.BombScore;
        colorCount = gameManager.ColorCount;
        gridYOffset = gameManager.GridYOffset;
        gridXOffset = gameManager.GridXOffset;

        gameOverPanel.SetActive(false);


        mapMatris = new GameObject[gridWidth, gridHeight];

        colorList = new List<Color>();
        for (int i = 0; i < colorCount; i++)
        {
            colorList.Add(gameManager.Colors[i]);
        }


        StartCoroutine(CreateMap());
    }

    private void Update()
    {

        if (bombHexagon != null && bombTime == 0)
        {

            gameManager.IsGameOver = true;
        }

        if (gameManager.IsGameOver && !gameOverPanel.activeSelf)
        {

            gameOverPanel.SetActive(true);
            
        }
    }


    #region CheckMap and Fill Empty Spaces

    public bool IsMapDeneme()
    {
        

            foreach (var item in mapMatris)
            {
                if (item == null)
                {
                    canRotate = false;
                    isMapFull = false;
                    return true;
                }
            }

        return false;
    }

    IEnumerator CheckMap()
    {
        if (!gameManager.IsGameOver)
        {
            for (int i = 0; i < gridWidth; i++)
            {
                for (int j = gridHeight - 1; j > 0; j--)
                {

                    int tempj = j;
                    if (mapMatris[i, gridHeight - 1] == null)
                    {

                        tempj = gridHeight - 1;
                        StartCoroutine(InstantiateHexagon(i, gridHeight - 1));

                    }

                    while (mapMatris[i, tempj] == null)
                    {
                        tempj++;
                    }
                    int y = tempj - 1;
                    while (mapMatris[i, y] == null)
                    {


                        float yPos = y * gridYOffset;
                        float xPos = i * gridXOffset;
                        if (i % 2 == 0)
                        {
                            yPos += gridYOffset / 2;

                        }



                        Vector2 pos = new Vector2(xPos, yPos);


                        mapMatris[i, tempj].GetComponent<Hexagon.HexagonMovement>().ChangePos(pos);
                        mapMatris[i, y] = mapMatris[i, tempj];
                        mapMatris[i, y].name = "Hex" + i + "_" + y;
                        mapMatris[i, y].GetComponent<Hexagon.HexagonController>().X = i;
                        mapMatris[i, y].GetComponent<Hexagon.HexagonController>().Y = y;
                        mapMatris[i, tempj] = null;
                        tempj = y;
                        y--;
                        if (y < 0)
                        {

                            break;
                        }
                    }


                }
                yield return new WaitForSeconds(.004f);



            }

            if (IsMapDeneme())
            {
                isMapFull = false;
                StartCoroutine(CheckMap());
            }
            else
            {

                yield return new WaitForSeconds(1f);
                

                CheckMatch();
            }
        }
     
               
    }

    IEnumerator InstantiateHexagon(int i , int j)
    {
        

        Vector2 pos = CalculatePosition(i,j);
        float screenHeightPos = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, 0)).y + gridHeight;
        Vector2 startPos = new Vector2(pos.x, gameManager.CamY + (screenHeightPos / 2f));


        GameObject go = Instantiate(hexPrefab, startPos, Quaternion.identity);

        go.GetComponent<Hexagon.HexagonController>().SetColor(colorList[Random.Range(0, colorList.Count)]);


        if (scoreController.Score >= bombScore && bombHexagon == null)
        {
            bombScore += gameManager.BombScore; 
            bombTime = gameManager.BombTime;
            go.GetComponent<Hexagon.HexagonController>().SetBombText(bombTime.ToString());
            bombHexagon = go;
        }

        go.GetComponent<Hexagon.HexagonMovement>().ChangePos(pos);

        go.transform.name = "Hex" + i + "_" + j;
        mapMatris[i, j] = go;
        mapMatris[i, j].GetComponent<Hexagon.HexagonController>().X = i;
        mapMatris[i, j].GetComponent<Hexagon.HexagonController>().Y = j;



        yield return null;
    }
    #endregion

    #region StartGame

    IEnumerator CreateMap()
    {


        for (int i = 0; i < gridWidth; i++)
        {
            List<GameObject> hexagonList = new List<GameObject>();
            for (int j = 0; j < gridHeight; j++)
            {


                hexagonList.Add(InstantiateHexagonForList(i, j));
            }

            StartCoroutine(MoveHexagonList(hexagonList));

            yield return new WaitForSeconds(.45f);
        }

        yield return new WaitForSeconds(2f);
        isMapFull = true;
        gameManager.IsGameStarted = true;
        CheckMatch();
    }
    public GameObject InstantiateHexagonForList(int i, int j)
    {
        Vector2 pos = CalculatePosition(i, j);
        float screenHeightPos = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, 0)).y + gridHeight;
        Vector2 startPos = new Vector2(pos.x, gameManager.CamY + (screenHeightPos / 2f));


        GameObject go = Instantiate(hexPrefab, startPos, Quaternion.identity);
        go.GetComponent<Hexagon.HexagonController>().HexPos = pos;
        go.GetComponent<Hexagon.HexagonMovement>().lerpPos = go.transform.position;
        go.GetComponent<Hexagon.HexagonController>().X = i;
        go.GetComponent<Hexagon.HexagonController>().Y = j;


        go.transform.name = "Hex" + i + "_" + j;
        mapMatris[i, j] = go;

        Color goColor = CheckStartColor(i,j);
        
        go.GetComponent<Hexagon.HexagonController>().SetColor(goColor);

        return go;
    }

    public Color CheckStartColor(int i , int j)
    {
        Color goColor = colorList[Random.Range(0, colorList.Count)];
        if (i > 0)
        {
            if (i % 2 == 0)
            {
                while (mapMatris[i - 1, j].GetComponent<Hexagon.HexagonController>().HexColor == goColor)
                {
                    goColor = colorList[Random.Range(0, colorList.Count)];
                }
            }
            else
            {
                if (j > 0)
                {

                    while (mapMatris[i - 1, j - 1].GetComponent<Hexagon.HexagonController>().HexColor == goColor)
                    {
                        goColor = colorList[Random.Range(0, colorList.Count)];
                    }
                }
            }
        }

        return goColor;

    }

    IEnumerator MoveHexagonList(List<GameObject> hexList)
    {
        foreach (var go in hexList)
        {
            go.GetComponent<Hexagon.HexagonMovement>().ChangePos(go.GetComponent<Hexagon.HexagonController>().HexPos);
            yield return new WaitForSeconds(.25f);
        }

    }

    #endregion

    #region SelectHexagons
    public void NeighboursOutline(List<GameObject> neighbours)
    {
        selection = neighbours;
        CloseNeighboursOutline(prevSelection);
        prevSelection = neighbours;
        InstantiateKnob(FindCenter(neighbours));

        foreach (var outline in neighbours)
        {
            outline.transform.GetChild(0).gameObject.SetActive(true);
        }

        
    }

    public void CloseNeighboursOutline(List<GameObject> prevNeighbours)
    {
       
        if (prevNeighbours != null)
        {

            foreach (var outline in prevNeighbours)
            {
                if (outline != null)
                {

                    outline.transform.parent = null;
                    outline.transform.GetChild(0).gameObject.SetActive(false);
                }
            }
            Destroy(prevKnob);
        }
        
        
    }
    public Vector2 FindCenter(List<GameObject> selection)
    {
        Vector3 tempPos = Vector3.zero;

        foreach (var item in selection)
        {
            tempPos += item.transform.position;
        }
        tempPos /= 3;

        return tempPos;

    }

    public void InstantiateKnob(Vector2 pos)
    {
        GameObject go = Instantiate(knobPrefab);
        go.transform.position = pos;


        prevKnob = go;
    }

    #endregion

    #region Match
    public void CheckMatch()
    {

        if (!gameManager.IsGameOver)
        {
            foreach (var hexagon in mapMatris)
            {


                hexagon.GetComponent<Hexagon.Neighbour>().FindNeighbours();
                foreach (var _neighbourList in hexagon.GetComponent<Hexagon.Neighbour>().NeighbourList)
                {
                    if (_neighbourList.Count == 3)
                    {

                        if (_neighbourList[0].GetComponent<Hexagon.HexagonController>().HexColor == _neighbourList[1].GetComponent<Hexagon.HexagonController>().HexColor
                            && _neighbourList[1].GetComponent<Hexagon.HexagonController>().HexColor == _neighbourList[2].GetComponent<Hexagon.HexagonController>().HexColor)
                        {
                            ExplosionEffect(_neighbourList);

                            canRotate = false;
                            isMapFull = false;
                            CloseNeighboursOutline(prevSelection);
                            prevSelection.Clear();
                            selection.Clear();
                            foreach (var item in _neighbourList)
                            {

                                mapMatris[item.GetComponent<Hexagon.HexagonController>().X, item.GetComponent<Hexagon.HexagonController>().Y] = null;
                                Destroy(item);
                            }
                            if (bombHexagon != null)
                            {
                                bombTime--;
                                bombHexagon.GetComponent<Hexagon.HexagonController>().SetBombText(bombTime.ToString());
                            }
                            int score = (_neighbourList.Count * scoreController.ScoreMult);

                            scoreController.Score += score;
                            scoreController.ScoreTextUpdate();
                            break;
                        }
                    }
                }
                if (IsMapDeneme())
                {
                    StartCoroutine(CheckMap());
                    break;
                }
            }
            if (!IsMapDeneme() && !isRotating)
            {

                gameManager.IsGameOver = CheckGameOver();
            }
            isMapFull = true;
            canRotate = true;
            isMatch = false;
        }

        
        
    }
    #endregion

    #region RotateHexagons

    public IEnumerator RotateHexagons()
    {
        List<GameObject> selectedGroup = new List<GameObject>(selection);
  
            isRotating = true;

            for (int i = selectedGroup.Count - 1; i >= 0 ; i--)
            {
                
              
                if (i == 0)
                {

                    SwapHexagonsInMatris(selectedGroup[2], selectedGroup[1]);
                    SwapHexagonsInMatris(selectedGroup[i], selectedGroup[2]);
                }
                else if (i == 1)
                {
                    SwapHexagonsInMatris(selectedGroup[i - 1], selectedGroup[2]);
                    SwapHexagonsInMatris(selectedGroup[i], selectedGroup[i - 1]);
                }
                else
                {

                    SwapHexagonsInMatris(selectedGroup[i - 1], selectedGroup[i - 2]);
                    SwapHexagonsInMatris(selectedGroup[i], selectedGroup[i - 1]);
                }


                yield return new WaitForSeconds(.7f);
                CheckMatch();

                if (IsMapDeneme())
                {
                CloseNeighboursOutline(selectedGroup);
                break;
                }
        }
      


        isRotating = false;
    }
    
    public IEnumerator RotateHexagonsClockWise()
    {
        List<GameObject> selectedGroup = new List<GameObject>(selection);


        isRotating = true;


        for (int i = 0; i < selectedGroup.Count; i++)
        {


            if (i == 0)
            {

                SwapHexagonsInMatris(selectedGroup[i + 1], selectedGroup[i + 2]);
                SwapHexagonsInMatris(selectedGroup[i], selectedGroup[i + 1]);
            }
            else if (i == 1)
            {
                SwapHexagonsInMatris(selectedGroup[i + 1], selectedGroup[0]);
                SwapHexagonsInMatris(selectedGroup[i], selectedGroup[i + 1]);
            }
            else
            {

                SwapHexagonsInMatris(selectedGroup[0], selectedGroup[1]);
                SwapHexagonsInMatris(selectedGroup[i], selectedGroup[0]);
            }


            yield return new WaitForSeconds(.7f);
            CheckMatch();

            if (IsMapDeneme())
            {
                CloseNeighboursOutline(selectedGroup);
                break;
            }
        }



        isRotating = false;
    }


    public void SwapHexagonsInMatris(GameObject hex1, GameObject hex2)
    {

      

        mapMatris[hex1.GetComponent<Hexagon.HexagonController>().X, hex1.GetComponent<Hexagon.HexagonController>().Y] = hex2;

        int tempX = hex1.GetComponent<Hexagon.HexagonController>().X;
        int tempY = hex1.GetComponent<Hexagon.HexagonController>().Y;


        hex1.GetComponent<Hexagon.HexagonController>().X = hex2.GetComponent<Hexagon.HexagonController>().X;
        hex1.GetComponent<Hexagon.HexagonController>().Y = hex2.GetComponent<Hexagon.HexagonController>().Y;
        hex1.GetComponent<Hexagon.HexagonMovement>().ChangePos(CalculatePosition(hex1.GetComponent<Hexagon.HexagonController>().X, hex1.GetComponent<Hexagon.HexagonController>().Y));
        mapMatris[hex2.GetComponent<Hexagon.HexagonController>().X, hex2.GetComponent<Hexagon.HexagonController>().Y] = hex1;
        mapMatris[hex2.GetComponent<Hexagon.HexagonController>().X, hex2.GetComponent<Hexagon.HexagonController>().Y].name = "Hex" + hex1.GetComponent<Hexagon.HexagonController>().X + "_" + hex1.GetComponent<Hexagon.HexagonController>().Y;

        hex2.GetComponent<Hexagon.HexagonController>().X = tempX;
        hex2.GetComponent<Hexagon.HexagonController>().Y = tempY;

        hex2.GetComponent<Hexagon.HexagonMovement>().ChangePos(CalculatePosition(tempX,tempY));
        mapMatris[tempX, tempY] = hex2;

        mapMatris[tempX, tempY].name = "Hex" + tempX + "_" + tempY;

      
    }



    Vector2 CalculatePosition(int x , int y)
    {
        float yPos = y * gridYOffset;
        float xPos = x * gridXOffset;
        if (x % 2 == 0)
        {
            yPos += gridYOffset / 2;

        }


        return new Vector2(xPos, yPos);
    }
    #endregion

    #region GameOverLogic


    public bool CheckGameOver()
    {


        foreach (var hexagon in mapMatris)
        {
            hexagon.GetComponent<Hexagon.Neighbour>().FindNeighbours();
            foreach (var _neighbourList in hexagon.GetComponent<Hexagon.Neighbour>().NeighbourList)
            {
                if (_neighbourList.Count == 3)
                {

                    if (_neighbourList[0].GetComponent<Hexagon.HexagonController>().HexColor == _neighbourList[1].GetComponent<Hexagon.HexagonController>().HexColor
                        || _neighbourList[1].GetComponent<Hexagon.HexagonController>().HexColor == _neighbourList[2].GetComponent<Hexagon.HexagonController>().HexColor
                        || _neighbourList[0].GetComponent<Hexagon.HexagonController>().HexColor == _neighbourList[2].GetComponent<Hexagon.HexagonController>().HexColor)
                    {

                        bool canMatch = CheckNeighbourForMatch(_neighbourList);
                        if (canMatch)
                        {

                            return false;
                        }
                    }
                }
            }

        }
        return true;
    }

    public bool CheckNeighbourForMatch(List<GameObject> _neighbourList)
    {
        GameObject differentObj = _neighbourList[0];
        if (_neighbourList[0].GetComponent<Hexagon.HexagonController>().HexColor == _neighbourList[1].GetComponent<Hexagon.HexagonController>().HexColor)
            differentObj = _neighbourList[2];
        else if (_neighbourList[0].GetComponent<Hexagon.HexagonController>().HexColor == _neighbourList[2].GetComponent<Hexagon.HexagonController>().HexColor)
            differentObj = _neighbourList[1];
        else if (_neighbourList[1].GetComponent<Hexagon.HexagonController>().HexColor == _neighbourList[2].GetComponent<Hexagon.HexagonController>().HexColor)
            differentObj = _neighbourList[0];





        Color sameColor = _neighbourList.Find(x => x.GetComponent<Hexagon.HexagonController>().HexColor != differentObj.GetComponent<Hexagon.HexagonController>().HexColor).GetComponent<Hexagon.HexagonController>().HexColor;
     

        List<GameObject> sameObjects = new List<GameObject>(_neighbourList);
        sameObjects.Remove(differentObj);

        

        List<List<GameObject>> neighbourLists = new List<List<GameObject>>(differentObj.GetComponent<Hexagon.Neighbour>().NeighbourList);
            foreach (var neighbourList in neighbourLists)
            {
                
                
                if (neighbourList.Count == 3 && !ListContains(neighbourList,sameObjects))
                {

                    if (neighbourList[0].GetComponent<Hexagon.HexagonController>().HexColor == sameColor
                        || neighbourList[1].GetComponent<Hexagon.HexagonController>().HexColor == sameColor
                        || neighbourList[2].GetComponent<Hexagon.HexagonController>().HexColor == sameColor)
                    {
                        return true;
                    }
                }
            }
        return false;
        

    }

    private bool ListContains(List<GameObject> _list, List<GameObject> _gameObjList)
    {
        foreach (var item in _list)
        {
            if (_gameObjList.Contains(item))
                return true;
            
        }
        return false;
    }

    #endregion

    public void ExplosionEffect(List<GameObject> _neighbourList)
    {

            
            
            particleSystem.transform.position = FindCenter(_neighbourList);
            
            var main = particleSystem.main;
             main.startColor = _neighbourList[0].GetComponent<Hexagon.HexagonController>().HexColor;
            particleSystem.Play();
       
    }
}

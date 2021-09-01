using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    #region Singleton

    private static GameManager _instance;
    public static GameManager Instance
    {
        get
        {

            _instance = FindObjectOfType<GameManager>();
            

            
                if (_instance == null)
                {
                   



                        GameObject singleton = new GameObject();
                        _instance = singleton.AddComponent<GameManager>();
                        singleton.name = typeof(GameManager).ToString();
                       

                         
                        

                    

                }

               DontDestroyOnLoad(_instance);
               

                return _instance;
        }
        
    }

    #endregion

    private bool _isGameOver = false;
    private bool _isGameStarted = false;


    [Header("Map")]
    [SerializeField] private int _gridWidth = 8;
    [SerializeField] private int _gridHeight = 9;
    private int _gridWidthMax = 10;
    private int _gridWidthMin = 6;
    private int _gridHeightMax = 11;
    private int _gridHeightMin = 7;
    private float gridYOffset = .626f;
    private float gridXOffset = .562f;


    [Header("Bomb")]
    [SerializeField] private int _bombTime = 7;
    private int _bombTimeMax = 10;
    private int _bombTimeMin = 5;

    [Header("Color")]
    [SerializeField] private int _colorCount = 5;
    private int _colorCountMax = 7;
    private int _colorCountMin = 5;
    [SerializeField] private Color[] _colors;

    public bool IsGameOver { get => _isGameOver; set => _isGameOver = value; }
    public bool IsGameStarted { get => _isGameStarted; set => _isGameStarted = value; }
    public int GridWidth { get => _gridWidth; set => _gridWidth = value; }
    public int GridHeight { get => _gridHeight; set => _gridHeight = value; }
    public int ColorCount { get => _colorCount; set => _colorCount = value; }
    public Color[] Colors { get => _colors; set => _colors = value; }
    public int BombTime { get => _bombTime; set => _bombTime = value; }
    public int ColorCountMax { get => _colorCountMax; set => _colorCountMax = value; }
    public int ColorCountMin { get => _colorCountMin; set => _colorCountMin = value; }
    public int GridWidthMax { get => _gridWidthMax; set => _gridWidthMax = value; }
    public int GridWidthMin { get => _gridWidthMin; set => _gridWidthMin = value; }
    public int GridHeightMax { get => _gridHeightMax; set => _gridHeightMax = value; }
    public int GridHeightMin { get => _gridHeightMin; set => _gridHeightMin = value; }
    public int BombTimeMax { get => _bombTimeMax; set => _bombTimeMax = value; }
    public int BombTimeMin { get => _bombTimeMin; set => _bombTimeMin = value; }
    public float GridYOffset { get => gridYOffset; set => gridYOffset = value; }
    public float GridXOffset { get => gridXOffset; set => gridXOffset = value; }
    public float CamX { get => (GridWidth / 4f);}
    public float CamY { get => ((GridHeight * GridYOffset) + 1f) / 2f;}
}

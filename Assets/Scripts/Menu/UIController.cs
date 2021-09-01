using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;
using UnityEngine.SceneManagement;

public class UIController : MonoBehaviour
{
    GameManager gameManager;

    [Header("Color")]
    [SerializeField] Slider colorCountSlider;
    [SerializeField] TextMeshProUGUI colorCountValueText;

    [Header("Map")]
    [SerializeField] Slider gridWidthSlider;
    [SerializeField] Slider gridHeightSlider;
    [SerializeField] TextMeshProUGUI gridWidthValueText;
    [SerializeField] TextMeshProUGUI gridHeightValueText;

    [Header("Bomb")]
    [SerializeField] Slider bombTimeSlider;
    [SerializeField] TextMeshProUGUI bombTimeValueText;

    [Header("Buttons")]
    [SerializeField] Button startButton;
    [SerializeField] Button settingsButton;
    [SerializeField] Button quitButton;
    [SerializeField] Button closeSettingsButton;

    [SerializeField] GameObject settingsPanel;

    private void Start()
    {
        gameManager = GameManager.Instance;

        colorCountSlider.wholeNumbers = true;
        colorCountSlider.maxValue = gameManager.ColorCountMax;
        colorCountSlider.minValue = gameManager.ColorCountMin;
        colorCountSlider.value = gameManager.ColorCount;
        colorCountValueText.text = ((int)colorCountSlider.value).ToString();
        colorCountSlider.onValueChanged.AddListener(delegate { ColorCountSliderChanged(); });

        gridWidthSlider.wholeNumbers = true;
        gridWidthSlider.maxValue = gameManager.GridWidthMax;
        gridWidthSlider.minValue = gameManager.GridWidthMin;
        gridWidthSlider.value = gameManager.GridWidth;
        gridWidthValueText.text = ((int)gridWidthSlider.value).ToString();
        gridWidthSlider.onValueChanged.AddListener(delegate { GridWidthSliderChanged(); });


        gridHeightSlider.wholeNumbers = true;
        gridHeightSlider.maxValue = gameManager.GridHeightMax;
        gridHeightSlider.minValue = gameManager.GridHeightMin;
        gridHeightSlider.value = gameManager.GridHeight;
        gridHeightValueText.text = ((int)gridHeightSlider.value).ToString();
        gridHeightSlider.onValueChanged.AddListener(delegate { GridHeightSliderChanged(); });


        bombTimeSlider.wholeNumbers = true;
        bombTimeSlider.maxValue = gameManager.BombTimeMax;
        bombTimeSlider.minValue = gameManager.BombTimeMin;
        bombTimeSlider.value = gameManager.BombTime;
        bombTimeValueText.text = ((int)bombTimeSlider.value).ToString();
        bombTimeSlider.onValueChanged.AddListener(delegate { BombTimeSliderChanged(); });


        

        startButton.onClick.AddListener(delegate { StartButton(); });
        settingsButton.onClick.AddListener(delegate { SettingsButton(); });
        quitButton.onClick.AddListener(delegate { QuitButton(); });
        closeSettingsButton.onClick.AddListener(delegate { CloseSettingsButton(); });

        settingsPanel.gameObject.SetActive(false);
        closeSettingsButton.gameObject.SetActive(false);
    }

    #region SliderAction

    private void ColorCountSliderChanged()
    {
        gameManager.ColorCount = (int)colorCountSlider.value;
        colorCountValueText.text = ((int)colorCountSlider.value).ToString();
    }
    private void GridWidthSliderChanged()
    {
        gameManager.GridWidth = (int)gridWidthSlider.value;
        gridWidthValueText.text = ((int)gridWidthSlider.value).ToString();
    }
    private void GridHeightSliderChanged()
    {
        if ((int)gridHeightSlider.value % 2 == 0)
        {
            gridHeightSlider.value++;
        }
        gameManager.GridHeight = (int)gridHeightSlider.value;
        gridHeightValueText.text = ((int)gridHeightSlider.value).ToString();
    }

    private void BombTimeSliderChanged()
    {
        gameManager.BombTime = (int)bombTimeSlider.value;
        bombTimeValueText.text = ((int)bombTimeSlider.value).ToString();
    }
    #endregion

    #region Buttons
    public void StartButton()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void SettingsButton()
    {
        if (settingsPanel.activeSelf)
        {
            settingsPanel.gameObject.SetActive(false);
        }
        else
        {
            closeSettingsButton.gameObject.SetActive(true);
            settingsPanel.gameObject.SetActive(true);
        }
    }
    private void CloseSettingsButton()
    {
        settingsPanel.gameObject.SetActive(false);
        closeSettingsButton.gameObject.SetActive(false);
    }
    public void QuitButton()
    {
        Application.Quit();
    }

    #endregion
}

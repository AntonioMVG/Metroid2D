using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private int score;
    private bool win;

    [HideInInspector] public GameObject statsPanel;

    /**********************/
    // Statistics variables
    /**********************/
    private int totalJumps;
    private float steps;
    private int collectibles;
    [HideInInspector] public TextMeshProUGUI jumpsTxt;
    [HideInInspector] public TextMeshProUGUI metersTxt;
    [HideInInspector] public TextMeshProUGUI collectiblesTxt;

    /**************/
    // Declarations
    /**************/
    public int Score { get => score; set => score = value; }
    public bool Win { get => win; set => win = value; }
    public int TotalJumps { get => totalJumps; set => totalJumps = value; }
    public float Steps { get => steps; set => steps = value; }
    public int Collectibles { get => collectibles; set => collectibles = value; }

    public static GameManager instance;

    private void Awake()
    {
        // First time
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void ShowPanel()
    {
        statsPanel.gameObject.SetActive(true);
        jumpsTxt.text = "Total jumps: " + totalJumps;
        metersTxt.text = "Total meters walked: " + (int)steps;
        collectiblesTxt.text = "Total collectibles recolected: " + collectibles;
    }

    public void LoadScene(string sceneName)
    {
        ResumeGame();
        SceneManager.LoadScene(sceneName);
    }

    public void PauseGame()
    {
        Time.timeScale = 0;
    }
    public void ResumeGame()
    {
        Time.timeScale = 1;
    }
}

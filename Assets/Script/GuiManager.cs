using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GuiManager : MonoBehaviour
{
    public static GuiManager Instance;

    private GameManager gameManager;

    public GameObject StartGameButton;

    public Text ScoreText;

    public GameObject Lives;

    public GameObject[] LiveGOs;

    private void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        gameManager = GameManager.Instance;
    }

    private void Update()
    {
        RefreshScore();
        RefreshLives();
    }

    public void RefreshScore()
    {
        ScoreText.text = "Score\n" + Mathf.FloorToInt(gameManager.Score);
    }

    public void RefreshLives()
    {
        for (int i = 0; i < gameManager.MaxLives; i++)
        {
            if(i< gameManager.Lives)
            LiveGOs[i].SetActive(true);
            else LiveGOs[i].SetActive(false);
        }
    }


}

using System.Text;
using UnityEngine;
using UnityEngine.UI;

public class GuiManager : MonoBehaviour
{
    public static GuiManager Instance;

    private GameManager gameManager;

    public GameObject StartGameButton;

    public Text ScoreText;

    public GameObject[] LiveGOs;

    private int m_TempScore = 0;

    private StringBuilder stringBuilder = new StringBuilder(50);

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
        //这个位置是不是会因为字符串的拼接导致产生GC？
        //就是这个问题，都写在官方反例上了。
        //保持效果或许可以使用stringBuilder
        stringBuilder.Clear();
        ScoreText.text = stringBuilder.Append("Score\n"+gameManager.Score).ToString();

        m_TempScore = gameManager.Score;
        
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

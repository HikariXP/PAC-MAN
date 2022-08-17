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
        //���λ���ǲ��ǻ���Ϊ�ַ�����ƴ�ӵ��²���GC��
        //����������⣬��д�ڹٷ��������ˡ�
        //����Ч���������ʹ��stringBuilder
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

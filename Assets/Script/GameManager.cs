using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public float Score = 0f;

    public int MaxLives = 3;
    public int Lives;

    public float GameStartDelay = 3f;

    public int DotRemain;

    public AIPathFinder Blinky;
    public AIPathFinder Pinky;
    public AIPathFinder Inky;
    public AIPathFinder Clyde;

    public Vector2 BlinkyStartPos;
    public Vector2 PinkyStartPos;
    public Vector2 InkyStartPos;
    public Vector2 ClydeStartPos;

    public PacMan player;

    public Vector2 playerStartPos;

    //Event
    public delegate void GameStartHandler();
    public event GameStartHandler GameStartEvent;

    public delegate void GameOverHandler();
    public event GameOverHandler GameOverEvent;

    public delegate void GameWinHandler();
    public event GameWinHandler GameWinEvent;

    public bool isWin = false;

    private void Awake()
    {
        if (Instance != null) Destroy(gameObject);
        else Instance = this;
    }

    public void Update()
    {
        if (DotRemain <= 0)
        {
            if (!isWin)
            {
                PlayerWin();
                isWin = true;
            }
            
        }
    }

    private void Start()
    {
        ResetGame();
    }

    //Reset all things
    public void ResetGame()
    {
        DotRemain = 156;
        //live
        Lives = MaxLives;
        //Elements
        ResetPos();
        //dots
        TilemapManager.Instance.SetDots();
        //score
        Score = 0f;

        isWin = false;
    }

    //when player be catch with enemy
    public void ResetPos()
    {
        //enemy
        ResetEnemyPos();
        //pacman
        player.gameObject.transform.position = playerStartPos;
        player.GetComponent<PacMan>().Respawn();
    }

    public void ResetStartPos()
    { 
    
    }

    public void StartGame()
    {
        StartCoroutine(StartGameDelay());
    }

    public void PlayerWin()
    {
        GameWinEvent?.Invoke();
        StartCoroutine(GameOverDelay());
    }

    public void PlayerBeCatched()
    {
        Change_canMove(false);
        if (Lives > 0)
        {
            StartCoroutine(ReStartGameDelay());
        }
        else
        {
            GameOverEvent?.Invoke();
            StartCoroutine(GameOverDelay());
        }
        
    }

    public IEnumerator ReStartGameDelay()
    {
        yield return new WaitForSeconds(4f);
        Lives -= 1;
        ResetPos();
        StartGame();
    }



    private IEnumerator StartGameDelay()
    {
        GameStartEvent?.Invoke();
        yield return new WaitForSeconds(GameStartDelay);
        Change_canMove(true);
    }

    private IEnumerator GameOverDelay()
    {
        yield return new WaitForSeconds(6f);
        ResetGame();
        GuiManager.Instance.StartGameButton.SetActive(true);
    }

    public void ResetEnemyPos()
    {
        Blinky.ResetPos(BlinkyStartPos);
        Pinky.ResetPos(PinkyStartPos);
        Inky.ResetPos(InkyStartPos);
        Clyde.ResetPos(ClydeStartPos);
    }

    private void Change_canMove(bool isCanMove)
    {
        player.canMove = isCanMove;
        Blinky.canMove = isCanMove;
        Pinky.canMove = isCanMove;
        Inky.canMove = isCanMove;
        Clyde.canMove = isCanMove;
    }
}

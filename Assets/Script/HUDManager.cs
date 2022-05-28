using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HUDManager : MonoBehaviour
{
    public GameManager gameManager;

    public GameObject GameStartTextGO;
    public GameObject GameOverTextGO;
    public GameObject GameWinTextGO;

    private void Start()
    {
        gameManager = GameManager.Instance;
    }

    private void OnEnable()
    {
        gameManager.GameStartEvent += OnGameStart;
        gameManager.GameOverEvent += OnGameOver;
        gameManager.GameWinEvent += OnGameWin;
    }

    private void OnDisable()
    {
        gameManager.GameStartEvent -= OnGameStart;
        gameManager.GameOverEvent -= OnGameOver;
        gameManager.GameWinEvent -= OnGameWin;
    }

    public void OnGameStart()
    {
        StartCoroutine(IE_GameStart());
    }

    public void OnGameOver()
    {
        StartCoroutine(IE_GameOver());
    }

    public void OnGameWin()
    {
        StartCoroutine(IE_GameWin());
    }

    private IEnumerator IE_GameStart()
    {
        GameStartTextGO.SetActive(true);
        yield return new WaitForSeconds(gameManager.GameStartDelay);
        GameStartTextGO.SetActive(false);
    }

    private IEnumerator IE_GameOver()
    {
        GameOverTextGO.SetActive(true);
        yield return new WaitForSeconds(1f);
        GameOverTextGO.SetActive(false);
        yield return new WaitForSeconds(1f);
        GameOverTextGO.SetActive(true);
        yield return new WaitForSeconds(1f);
        GameOverTextGO.SetActive(false);
        yield return new WaitForSeconds(1f);
        GameOverTextGO.SetActive(true);
        yield return new WaitForSeconds(3f);
        GameOverTextGO.SetActive(false);
    }    
    private IEnumerator IE_GameWin()
    {
        GameWinTextGO.SetActive(true);
        yield return new WaitForSeconds(1f);
        GameWinTextGO.SetActive(false);
        yield return new WaitForSeconds(1f);
        GameWinTextGO.SetActive(true);
        yield return new WaitForSeconds(1f);
        GameWinTextGO.SetActive(false);
        yield return new WaitForSeconds(1f);
        GameWinTextGO.SetActive(true);
        yield return new WaitForSeconds(3f);
        GameWinTextGO.SetActive(false);
    }
}

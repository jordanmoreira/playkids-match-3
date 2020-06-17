using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : Singleton<GameManager>
{
    public int movesLeft;
    public int scoreGoal;
    public float maxTime;
    public float currentTime;

    bool isRdyToBegin = false;
    bool isGameOver = false;
    bool isWinner = false;
    bool runTimer = true;

    Board board;

    // Start is called before the first frame update
    void Start()
    {
        board = GameObject.FindObjectOfType<Board>().GetComponent<Board>();
        Scene currentScene = SceneManager.GetActiveScene();

        if (UIManager.Instance.levelNameText != null)
        {
            UIManager.Instance.levelNameText.text = currentScene.name;
        }
        currentTime = maxTime;
        UIManager.Instance.UpdateMoves();

        StartCoroutine(ExecuteGameLoop());
    }

    public void LoadLevel()
    {
        Scene currentScene = SceneManager.GetActiveScene();

        if (isGameOver == true)
        {
            SceneManager.LoadSceneAsync(0);
        }
        if (isWinner == true)
        {
            SceneManager.LoadSceneAsync(0);
        }
        if (!isGameOver)
        {
            switch (currentScene.name)
            {
                case "Level1":
                    SceneManager.LoadSceneAsync(1);
                    break;
                case "Level2":
                    SceneManager.LoadSceneAsync(2);
                    break;
            }
        }
    }

    public void StartCountdown()
    {

        StartCoroutine(CountDownRoutine());
    }
    IEnumerator CountDownRoutine()
    {
        while (currentTime > 0 && runTimer)
        {
            yield return new WaitForSeconds(1f);
            currentTime--;
        }
    }
    IEnumerator ExecuteGameLoop()
    {
        yield return StartCoroutine(StartGameRoutine());
        yield return StartCoroutine(PlayGameRoutine());
        yield return StartCoroutine(EndGameRoutine());
    }
    IEnumerator StartGameRoutine()
    {
        while (!isRdyToBegin)
        {
            yield return null;
            yield return new WaitForSeconds(2f);
            isRdyToBegin = true;
        }
    }
    IEnumerator PlayGameRoutine()
    {
        StartCountdown();
        while (!isGameOver)
        {
            UIManager.Instance.UpdateTimer();
            if (ScoreManager.Instance != null)
            {
                if (ScoreManager.Instance.CurrentScore >= scoreGoal && SceneManager.GetActiveScene().name != "Level3")
                {
                    UIManager.Instance.ShowMessage("Congratulations,\n you can now go to \n the next level!", "Go");
                    UIManager.Instance.ActivateMessagePanel();
                    runTimer = false;
                }
                if (ScoreManager.Instance.CurrentScore >= scoreGoal && SceneManager.GetActiveScene().name == "Level3")
                {
                    UIManager.Instance.ShowMessage("YOU WON!!", "Restart Game");
                    UIManager.Instance.ActivateMessagePanel();
                    runTimer = false;
                    isWinner = true;
                }
            }
            if (movesLeft == 0 || currentTime <= 0)
            {
                isGameOver = true;
                runTimer = false;
            }

            yield return null;
        }
    }
    IEnumerator EndGameRoutine()
    {
        if (isGameOver == true)
        {
            Debug.Log("GAME OVER");
            UIManager.Instance.ShowMessage("YOU LOST!", "Restart");
            UIManager.Instance.ActivateMessagePanel();
        }

        yield return null;
    }
}

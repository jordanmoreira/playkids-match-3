using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : Singleton<GameManager>
{
    public int movesLeft;
    public int scoreGoal;
    public Text levelNameText;
    public Text movesLeftText;

    public Text messagePanelText;
    public Text messageButtonText;

    Board board;

    bool isRdyToBegin = false;
    bool isGameOver = false;
    bool isWinner = false;

    // Start is called before the first frame update
    void Start()
    {
        board = GameObject.FindObjectOfType<Board>().GetComponent<Board>();
        Scene currentScene = SceneManager.GetActiveScene();

        if (levelNameText != null)
        {
            levelNameText.text = currentScene.name;
        }

        UpdateMoves();

        StartCoroutine("ExecuteGameLoop");
    }

    public void UpdateMoves()
    {
        if (movesLeftText != null)
        {
            movesLeftText.text = movesLeft.ToString();
        }
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
    public void ActivateMessagePanel()
    {
        transform.GetChild(2).gameObject.SetActive(true);
    }
    public void ShowMessage(string msgPanelText, string msgButtonText)
    {
        messagePanelText.text = msgPanelText;
        messageButtonText.text = msgButtonText;
    }
    IEnumerator ExecuteGameLoop()
    {
        yield return StartCoroutine("StartGameRoutine");
        yield return StartCoroutine("PlayGameRoutine");
        yield return StartCoroutine("EndGameRoutine");
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
        while (!isGameOver)
        {
            if (ScoreManager.Instance != null)
            {
                if (ScoreManager.Instance.CurrentScore >= scoreGoal && SceneManager.GetActiveScene().name != "Level3")
                {
                    ShowMessage("Congratulations,\n you can now go to \n the next level!", "Go");
                    ActivateMessagePanel();
                }
                if (ScoreManager.Instance.CurrentScore >= scoreGoal && SceneManager.GetActiveScene().name == "Level3")
                {
                    ShowMessage("YOU WON!!", "Restart Game");
                    ActivateMessagePanel();
                    isWinner = true;
                }
            }
            if (movesLeft == 0)
            {
                isGameOver = true;
            }

            yield return null;
        }
    }
    IEnumerator EndGameRoutine()
    {
        if (isGameOver == true)
        {
            Debug.Log("GAME OVER");
            ShowMessage("You lose!", "Restart");
            ActivateMessagePanel();
        }

        yield return null;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : Singleton<UIManager>
{
    public Text levelNameText;
    public Text scoreGoalText;
    public Text movesLeftText;
    public Text timeLeftText;
    public Image timerBarImage;

    public Text messagePanelText;
    public Text messageButtonText;

    // Start is called before the first frame update
    void Start()
    {
        UpdateScoregoal();
    }

    public void UpdateMoves()
    {
        if (movesLeftText != null)
        {
            movesLeftText.text = GameManager.Instance.movesLeft.ToString();
        }
    }
    public void UpdateTimer()
    {
        timeLeftText.text = GameManager.Instance.currentTime.ToString();
        timerBarImage.fillAmount = GameManager.Instance.currentTime / GameManager.Instance.maxTime;
    }
    public void UpdateScoregoal()
    {
        scoreGoalText.text = "Score goal: " + GameManager.Instance.scoreGoal;
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
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : Singleton<UIManager>
{
    public Text levelNameText;
    public Text movesLeftText;
    public Text timeLeftText;
    public Image timerBarImage;

    public Text messagePanelText;
    public Text messageButtonText;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

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
        UIManager.Instance.timeLeftText.text = GameManager.Instance.currentTime.ToString();
        UIManager.Instance.timerBarImage.fillAmount = GameManager.Instance.currentTime / GameManager.Instance.maxTime;
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

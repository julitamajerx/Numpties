using System;
using TMPro;
using UnityEngine;

public class GameOver : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI coinsText;
    [SerializeField] private MiniGameMoneyManager moneyManager;

    private void Start()
    {
        coinsText.text = moneyManager.currentCoins.ToString();
    }

    public void ClickReturn()
    {
        Time.timeScale = 1f;

        if (GameFlowManager.Instance != null)
        {
            int reward = 0;

            Int32.TryParse(coinsText.text, out reward);

            GameFlowManager.Instance.ReturnToMainGame(reward);
        }
        else
        {
            Debug.LogError("Nie znaleziono GameFlowManager!");
        }
    }
}

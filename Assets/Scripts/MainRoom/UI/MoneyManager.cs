using TMPro;
using UnityEngine;

public class MoneyManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI coinsText;

    void Start()
    {
        UpdateCoinsUI();
    }
    public void UpdateCoinsUI()
    {
        int currentCoins = PlayerPrefs.GetInt("PlayerCoins", 0);

        if (coinsText != null)
        {
            coinsText.text = currentCoins.ToString();
        }
    }
}

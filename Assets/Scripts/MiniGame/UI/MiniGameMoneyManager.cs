using TMPro;
using UnityEngine;

public class MiniGameMoneyManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI coinsText;
    [SerializeField] private int coinsAmmount = 2;
    public int currentCoins = 0;

    public void UpdateCoinsUI()
    {
        currentCoins += coinsAmmount;

        coinsText.text = currentCoins.ToString();
    }
}

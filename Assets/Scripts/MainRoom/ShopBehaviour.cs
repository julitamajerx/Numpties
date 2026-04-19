using UnityEngine;

public class ShopBehaviour : MonoBehaviour
{
    [SerializeField] private GameObject shop;
    [SerializeField] private MoneyManager moneyManager;

    public void ShowShop()
    {
        shop.SetActive(true);
    }

    public void CloseShop()
    {
        shop.SetActive(false);
    }

    public void BuyItem(int cost)
    {
        int currentCoins = PlayerPrefs.GetInt("PlayerCoins", 0);
        if (currentCoins >= cost)
        {
            PlayerPrefs.SetInt("PlayerCoins", currentCoins - cost);
            PlayerPrefs.Save();
            Debug.Log("Zakupiono przedmiot! Pozosta³e monety: " + (currentCoins - cost));
            moneyManager.UpdateCoinsUI();
        }
        else
        {
            Debug.Log("Nie masz wystarczaj¹co monet!");
        }
    }
}

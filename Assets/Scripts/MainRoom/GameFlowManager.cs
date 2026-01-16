using UnityEngine;
using UnityEngine.SceneManagement;

public class GameFlowManager : MonoBehaviour
{
    public static GameFlowManager Instance;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void LoadMiniGame(string miniGameName)
    {
        PlayerPrefs.Save();
        SceneManager.LoadScene(miniGameName);
    }

    public void ReturnToMainGame(int reward)
    {
        int currentCoins = PlayerPrefs.GetInt("PlayerCoins", 0);

        PlayerPrefs.SetInt("PlayerCoins", currentCoins + reward);

        PlayerPrefs.Save();

        Debug.Log("Zapisano pieni¹dze! Obecny stan: " + (currentCoins + reward));

        DragInteractionObject.isAnyObjectActive = false;
        SceneManager.LoadScene("MainGame");
    }
}
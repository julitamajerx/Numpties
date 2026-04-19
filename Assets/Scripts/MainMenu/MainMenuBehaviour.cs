using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuBehaviour : MonoBehaviour
{
    public GameObject settings;
    public GameObject startMenu;
    public void LoadMainGame(string mainGameSceneName)
    {
        SceneManager.LoadScene(mainGameSceneName);
    }

    public void OpenSettings()
    {
        startMenu.SetActive(false);
        settings.SetActive(true);
    }

    public void ReturnFromSettings()
    {
        settings.SetActive(false);
        startMenu.SetActive(true);
    }
}

using UnityEngine;

public class MiniGameButton : MonoBehaviour
{
    [SerializeField] private string sceneName = "MiniGame";
    [SerializeField]
    private NeedBehaviour[] needs;

   public void StartGame()
    {
        if (GameFlowManager.Instance != null)
        {
            foreach (NeedBehaviour need in needs)
            {
                string saveKey = "Save_" + need.gameObject.name;
                PlayerPrefs.SetFloat(saveKey, need.needSlider.value);
            }
            PlayerPrefs.Save();

            GameFlowManager.Instance.LoadMiniGame(sceneName);
        }
    }
}
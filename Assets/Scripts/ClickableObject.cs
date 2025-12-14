using UnityEngine;

public class ClickableObject : MonoBehaviour
{
    [SerializeField] private GameObject darkness;
    private bool isOn = true;

    public void ToggleState()
    {
        isOn = !isOn;
        Debug.Log("Stan obiektu: " + (isOn ? "W³¹czone" : "Wy³¹czone"));
    }

    public bool IsOn()
    {
        darkness.SetActive(!isOn); 
        return isOn;
    }
}

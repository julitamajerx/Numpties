using UnityEngine;

public class ClickableObject : MonoBehaviour
{
    [SerializeField] private GameObject darkness;
    private bool isOn = true;

    public void ToggleState()
    {
        isOn = !isOn;
        darkness.SetActive(!isOn);
    }

    public bool IsOn()
    {
        return isOn;
    }
}
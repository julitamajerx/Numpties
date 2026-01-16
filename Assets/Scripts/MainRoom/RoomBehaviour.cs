using UnityEngine;

public class RoomBehaviour : MonoBehaviour
{
    [SerializeField] private SpriteRenderer background;
    [SerializeField] private Color[] roomColors;
    [SerializeField] private GameObject[] tools;
    [SerializeField] private GameObject minigame;
    [SerializeField] private ClickableObject lightControl;

    private int currentRoomIndex = 0;

    void Start()
    {
        currentRoomIndex = PlayerPrefs.GetInt("CurrentRoom", 0);
        SetCurrentRoom(currentRoomIndex);
    }

    public void ChangeRoomRight()
    {
        if (DragInteractionObject.isAnyObjectActive || (lightControl != null && !lightControl.IsOn())) return;

        currentRoomIndex = (currentRoomIndex + 1) % roomColors.Length;
        SaveRoom();
        SetCurrentRoom(currentRoomIndex);
    }

    public void ChangeRoomLeft()
    {
        if (DragInteractionObject.isAnyObjectActive || (lightControl != null && !lightControl.IsOn())) return;

        currentRoomIndex--;
        if (currentRoomIndex < 0) currentRoomIndex = roomColors.Length - 1;
        SaveRoom();
        SetCurrentRoom(currentRoomIndex);
    }

    private void SetCurrentRoom(int roomIndex)
    {
        background.color = roomColors[roomIndex];
        for (int i = 0; i < tools.Length; i++)
        {
            tools[i].SetActive(i == roomIndex);
        }

        if(roomIndex == 1)
        {
            minigame.SetActive(true);
        }
        else 
        {
            minigame.SetActive(false);
        }
    }

    private void SaveRoom()
    {
        PlayerPrefs.SetInt("CurrentRoom", currentRoomIndex);
        PlayerPrefs.Save();
    }
}
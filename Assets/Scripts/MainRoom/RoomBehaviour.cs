using UnityEngine;

public class RoomBehaviour : MonoBehaviour
{
    [SerializeField] private SpriteRenderer background;
    [SerializeField] private Color[] roomColors;
    [SerializeField] private GameObject[] tools;
    [SerializeField] private GameObject minigame;
    [SerializeField] private GameObject shop;
    [SerializeField] private ClickableObject lightControl;
    [SerializeField] private GameObject[] roomDecorations;

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

        for (int i = 0; i < roomDecorations.Length; i++)
        {
            roomDecorations[i].SetActive(i == roomIndex);
        }

        minigame.SetActive(roomIndex == 1);
        shop.SetActive(roomIndex == 0);
    }

    private void SaveRoom()
    {
        PlayerPrefs.SetInt("CurrentRoom", currentRoomIndex);
        PlayerPrefs.Save();
    }
}
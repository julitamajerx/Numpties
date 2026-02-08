using UnityEngine;

public class RoomBehaviour : MonoBehaviour
{
    [SerializeField] private SpriteRenderer background;
    [SerializeField] private Color[] roomColors;
    [SerializeField] private GameObject[] tools;
    [SerializeField] private GameObject minigame;
    [SerializeField] private ClickableObject lightControl;

    // NOWA TABLICA NA WYSTROJE (DZIECI BACKGROUNDU)
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
        // Ustawienie koloru t³a
        background.color = roomColors[roomIndex];

        // Zarz¹dzanie narzêdziami (tools)
        for (int i = 0; i < tools.Length; i++)
        {
            tools[i].SetActive(i == roomIndex);
        }

        // --- NOWA LOGIKA DLA WYSTROJU ---
        for (int i = 0; i < roomDecorations.Length; i++)
        {
            // Aktywuje tylko ten wystrój, którego indeks zgadza siê z pokojem
            roomDecorations[i].SetActive(i == roomIndex);
        }

        // Minigierka (tylko w pokoju o indeksie 1)
        minigame.SetActive(roomIndex == 1);
    }

    private void SaveRoom()
    {
        PlayerPrefs.SetInt("CurrentRoom", currentRoomIndex);
        PlayerPrefs.Save();
    }
}
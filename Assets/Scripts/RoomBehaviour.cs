using UnityEngine;

public class RoomBehaviour : MonoBehaviour
{
    [SerializeField] private SpriteRenderer background;
    [SerializeField] private Color[] roomColors;
    [SerializeField] private GameObject[] tools;

    private int currentRoomIndex = 0;

    void Start()
    {
        SetCurrentRoom(0);
    }

    public void ChangeRoomRight()
    {
        currentRoomIndex++;

        if (currentRoomIndex >= roomColors.Length)
        {
            currentRoomIndex = 0;
        }

        SetCurrentRoom(currentRoomIndex);
    }

    public void ChangeRoomLeft()
    {
        currentRoomIndex--;

        if (currentRoomIndex < 0)
        {
            currentRoomIndex = roomColors.Length - 1;
        }

        SetCurrentRoom(currentRoomIndex);
    }

    private void SetCurrentRoom(int roomIndex)
    {
        background.color = roomColors[roomIndex];

        for (int i = 0; i < tools.Length; i++)
        {
            tools[i].SetActive(i == roomIndex);
        }
    }
}

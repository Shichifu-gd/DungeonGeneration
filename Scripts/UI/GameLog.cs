using UnityEngine.UI;
using UnityEngine;

public class GameLog : MonoBehaviour
{
    public Text RoomsAvailableLog;
    public Text CloseRoomLog;
    public Text MaxRoom;

    private int NumberOfClosedRooms;

    public void ForListOfRoomsThatAppeared(string other)
    {
        RoomsAvailableLog.text = other;
    }

    public void ForSpawnPoint()
    {
        NumberOfClosedRooms++;
        CloseRoomLog.text = NumberOfClosedRooms.ToString();
    }

    public void ForMaxRoom(int value)
    {
        MaxRoom.text = value.ToString();
    }
}
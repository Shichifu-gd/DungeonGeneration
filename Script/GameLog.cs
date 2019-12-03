using UnityEngine.UI;
using UnityEngine;

public class GameLog : MonoBehaviour
{
    [SerializeField] Text RoomsAvailableLog;
    [SerializeField] Text CloseRoomLog;
    [SerializeField] Text MaxRoom;

    int NumberOfClosedRooms;

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
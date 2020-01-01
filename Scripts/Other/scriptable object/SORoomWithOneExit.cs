using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/RoomWithOneExit")]
public class SORoomWithOneExit : ScriptableObject
{
    public GameObject UpRooms;
    public GameObject DownRooms;
    public GameObject RightRooms;
    public GameObject LeftRooms;
}
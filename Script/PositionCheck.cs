using UnityEngine;

public class PositionCheck : MonoBehaviour
{
    [SerializeField] Room room;

    // PNTR - surrounds the room and has a direction ..
    // EFTR - these are the exits from the room ..
    string PositionNearTheRoom;
    string ExitsFromTheRoom;
    string SideWithAisle;

    void Start()
    {
        PositionNearTheRoom = gameObject.name.Substring(14);
        ExitsFromTheRoom = room.gameObject.name.Substring(5);
        if (ExitsFromTheRoom.Contains(PositionNearTheRoom)) SideWithAisle = "on " + PositionNearTheRoom;
        else SideWithAisle = "Off " + PositionNearTheRoom;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("SpawnPoint"))
        {
            other.gameObject.GetComponent<SpawnFiltr>().StartSpawnOptions(SideWithAisle);
            Destroy(gameObject);
        }
        if (other.CompareTag("Room")) Destroy(gameObject);
    }
}
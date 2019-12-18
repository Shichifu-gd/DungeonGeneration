using System.Collections.Generic;
using UnityEngine;

public class RoomTemplates : MonoBehaviour
{
    float waitingForTimeToExit;

    bool SpawnExite = false;

    public ListOfRoomsThatAppeared listOfRoomsThatAppeared;

    public GameObject Background;
    public GameObject CrossroadsRooms;
    public GameObject ObjectClosedRoom;
    public GameObject ObjectExite;
    public GameObject ObjectError;

    [SerializeField] GameObject UpRooms;
    [SerializeField] GameObject DownRooms;
    [SerializeField] GameObject RightRooms;
    [SerializeField] GameObject LeftRooms;

    [SerializeField] List<GameObject> RoomTemplatesForSpawnUp;
    [SerializeField] List<GameObject> RoomTemplatesForSpawnDown;
    [SerializeField] List<GameObject> RoomTemplatesForSpawnRight;
    [SerializeField] List<GameObject> RoomTemplatesForSpawnLeft;

    [SerializeField] List<GameObject> RoomTemplateWithContinuedUp;
    [SerializeField] List<GameObject> RoomTemplateWithContinuedDown;
    [SerializeField] List<GameObject> RoomTemplateWithContinuedRight;
    [SerializeField] List<GameObject> RoomTemplateWithContinuedLeft;

    private void FixedUpdate()
    {
        if (waitingForTimeToExit <= 0 && SpawnExite == false)
        {
            PassageToTheNextLevel();
            SpawnExite = true;
        }
        else waitingForTimeToExit -= Time.deltaTime;
    }

    #region assignment

    public float WaitingForTimeToExit
    {
        set
        {
            if (value > 0 && value < 10) waitingForTimeToExit = value;
            else waitingForTimeToExit = 2.5f;
        }
    }

    #endregion

    public void PassageToTheNextLevel()
    {
        GameObject spawnPosition = listOfRoomsThatAppeared.IdentifyLastRoom();
        Instantiate(ObjectExite, spawnPosition.transform.position, Quaternion.identity);
    }

    public GameObject[] SelectRoom(string spawnPoint)
    {
        GameObject[] defaultSpawnList = null;
        if (spawnPoint == "SpawnPoint U") defaultSpawnList = RoomTemplatesForSpawnDown.ToArray();
        if (spawnPoint == "SpawnPoint D") defaultSpawnList = RoomTemplatesForSpawnUp.ToArray();
        if (spawnPoint == "SpawnPoint R") defaultSpawnList = RoomTemplatesForSpawnLeft.ToArray();
        if (spawnPoint == "SpawnPoint L") defaultSpawnList = RoomTemplatesForSpawnRight.ToArray();
        return defaultSpawnList;
    }

    public GameObject[] SelectLongRoom(string spawnPoint)
    {
        GameObject[] spawnLongRoom = null;
        if (spawnPoint == "SpawnPoint U" || spawnPoint == "Long SpawnPoint D") spawnLongRoom = RoomTemplateWithContinuedUp.ToArray();
        if (spawnPoint == "SpawnPoint D" || spawnPoint == "Long SpawnPoint U") spawnLongRoom = RoomTemplateWithContinuedDown.ToArray();
        if (spawnPoint == "SpawnPoint R" || spawnPoint == "Long SpawnPoint L") spawnLongRoom = RoomTemplateWithContinuedRight.ToArray();
        if (spawnPoint == "SpawnPoint L" || spawnPoint == "Long SpawnPoint R") spawnLongRoom = RoomTemplateWithContinuedLeft.ToArray();
        return spawnLongRoom;
    }

    public GameObject SelectLastRoom(string spawnPoint)
    {
        GameObject spawnLastRoom = null;
        if (spawnPoint == "SpawnPoint U") spawnLastRoom = DownRooms;
        if (spawnPoint == "SpawnPoint D") spawnLastRoom = UpRooms;
        if (spawnPoint == "SpawnPoint R") spawnLastRoom = LeftRooms;
        if (spawnPoint == "SpawnPoint L") spawnLastRoom = RightRooms;
        return spawnLastRoom;
    }

    public GameObject SelectFirstRoom()
    {
        GameObject spawnFirstRoom = null;
        int randomRoom = Random.Range(0, 4);
        if (randomRoom == 0) spawnFirstRoom = DownRooms;
        if (randomRoom == 1) spawnFirstRoom = UpRooms;
        if (randomRoom == 2) spawnFirstRoom = LeftRooms;
        if (randomRoom == 3) spawnFirstRoom = RightRooms;
        return spawnFirstRoom;
    }

    public GameObject Crossroads()
    {
        GameObject crossroads = CrossroadsRooms;
        return crossroads;
    }
}
using System.Collections.Generic;
using UnityEngine;

public class RoomTemplates : MonoBehaviour
{
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

    [SerializeField] List<GameObject> RoomWithTwoExitsInTheDirectionUp;
    [SerializeField] List<GameObject> RoomWithTwoExitsInTheDirectionDown;
    [SerializeField] List<GameObject> RoomWithTwoExitsInTheDirectionLeft;
    [SerializeField] List<GameObject> RoomWithTwoExitsInTheDirectionRight;

    float WaitingForTimeToExit = 2.5f;

    bool SpawnExite = false;

    private void FixedUpdate()
    {
        if (WaitingForTimeToExit <= 0 && SpawnExite == false)
        {
            PassageToTheNextLevel();
            SpawnExite = true;
        }
        else WaitingForTimeToExit -= Time.deltaTime;
    }

    public void PassageToTheNextLevel()
    {
        GameObject spawnPosition = listOfRoomsThatAppeared.IdentifyLastRoom();
        Instantiate(ObjectExite, spawnPosition.transform.position, Quaternion.identity);
    }
    
    public GameObject[] TheFormationOf(string spawnPoint)
    {
        GameObject[] defaultSpawnList = null;
        if (spawnPoint == "SpawnPoint U") defaultSpawnList = RoomWithTwoExitsInTheDirectionDown.ToArray();
        if (spawnPoint == "SpawnPoint D") defaultSpawnList = RoomWithTwoExitsInTheDirectionUp.ToArray();
        if (spawnPoint == "SpawnPoint R") defaultSpawnList = RoomWithTwoExitsInTheDirectionLeft.ToArray();
        if (spawnPoint == "SpawnPoint L") defaultSpawnList = RoomWithTwoExitsInTheDirectionRight.ToArray();
        int randomNumber = Random.Range(0, 1);      
        return defaultSpawnList;
    }

    public GameObject SpawnLastRoom(string spawnPoint)
    {
        GameObject spawnLastRoom = null;
        if (spawnPoint == "SpawnPoint U") spawnLastRoom = DownRooms;
        if (spawnPoint == "SpawnPoint D") spawnLastRoom = UpRooms;
        if (spawnPoint == "SpawnPoint R") spawnLastRoom = LeftRooms;
        if (spawnPoint == "SpawnPoint L") spawnLastRoom = RightRooms;
        return spawnLastRoom;
    }   

    public GameObject Crossroads()
    {
        GameObject crossroads = CrossroadsRooms;
        return crossroads;
    }
}
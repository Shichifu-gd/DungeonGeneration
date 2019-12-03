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

    public GameObject UpRooms;
    public GameObject DownRooms;
    public GameObject RightRooms;
    public GameObject LeftRooms;

    public List<GameObject> RoomWithTwoExitsInTheDirectionUp;
    public List<GameObject> RoomWithTwoExitsInTheDirectionDown;
    public List<GameObject> RoomWithTwoExitsInTheDirectionLeft;
    public List<GameObject> RoomWithTwoExitsInTheDirectionRight;

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

    public List<GameObject> TheFormationOf( string spawnPoint)
    {
        List<GameObject> defaultSpawnList = null;
        if (spawnPoint == "SpawnPoint U") defaultSpawnList = RoomWithTwoExitsInTheDirectionDown;
        if (spawnPoint == "SpawnPoint D") defaultSpawnList = RoomWithTwoExitsInTheDirectionUp;
        if (spawnPoint == "SpawnPoint R") defaultSpawnList = RoomWithTwoExitsInTheDirectionLeft;
        if (spawnPoint == "SpawnPoint L") defaultSpawnList = RoomWithTwoExitsInTheDirectionRight;
        int randomNumber = Random.Range(0, 12);
        if (randomNumber == 2 || randomNumber == 5) defaultSpawnList.Add(CrossroadsRooms);
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
}
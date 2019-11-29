using System.Collections.Generic;
using UnityEngine;

public class RoomTemplates : MonoBehaviour
{
    public ListOfRoomsThatAppeared listOfRoomsThatAppeared;

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

    private void Update()
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
        for (int i = 0; i < listOfRoomsThatAppeared.CountList(); i++)
        {
            if (i == listOfRoomsThatAppeared.CountList() - 1)
            {
                GameObject spawnPosition = listOfRoomsThatAppeared.test();
                Instantiate(ObjectExite, spawnPosition.transform.position, Quaternion.identity);
            }
        }
    }

    public List<GameObject> TheFormationOf(List<GameObject> DefaultSpawnList, string spawnPoint)
    {
        if (spawnPoint == "SpawnPoint U") DefaultSpawnList = RoomWithTwoExitsInTheDirectionDown;
        if (spawnPoint == "SpawnPoint D") DefaultSpawnList = RoomWithTwoExitsInTheDirectionUp;
        if (spawnPoint == "SpawnPoint R") DefaultSpawnList = RoomWithTwoExitsInTheDirectionLeft;
        if (spawnPoint == "SpawnPoint L") DefaultSpawnList = RoomWithTwoExitsInTheDirectionRight;
        int randomNumber = Random.Range(0, 15);
        if (randomNumber == 2 || randomNumber == 5) DefaultSpawnList.Add(CrossroadsRooms);
        return DefaultSpawnList;        
    }
}
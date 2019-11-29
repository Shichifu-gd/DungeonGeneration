using System.Collections.Generic;
using UnityEngine;

public class SpawnFiltr : MonoBehaviour
{
    RoomTemplates roomTemplates;
    SpawnRoom spawnRoom;

    string RoomThatIsOnTheSpawnSide;
    string DropoutItem;

    bool WhetherSpawnIsAllowed = true;

    [SerializeField] List<GameObject> UnfilteredList;

    private void Awake()
    {
        roomTemplates = GameObject.FindGameObjectWithTag("RoomTemplates").GetComponent<RoomTemplates>();
        spawnRoom = GetComponent<SpawnRoom>();
    }

    #region We filter rooms for spawn

    public void StartSpawnOptions(string VerifiablePosition)
    {
        RoomThatIsOnTheSpawnSide = VerifiablePosition;
        AddToUnfilteredList();
        RecognizeTheDropoutItem();
    }

    void AddToUnfilteredList()
    {
        if (gameObject.name == "SpawnPoint U") UnfilteredList = roomTemplates.RoomWithTwoExitsInTheDirectionDown;
        if (gameObject.name == "SpawnPoint D") UnfilteredList = roomTemplates.RoomWithTwoExitsInTheDirectionUp;
        if (gameObject.name == "SpawnPoint R") UnfilteredList = roomTemplates.RoomWithTwoExitsInTheDirectionLeft;
        if (gameObject.name == "SpawnPoint L") UnfilteredList = roomTemplates.RoomWithTwoExitsInTheDirectionRight;
    }

    void RecognizeTheDropoutItem()
    {
        switch (RoomThatIsOnTheSpawnSide)
        {
            case "Off D":
                DropoutItem = "U";
                CheckOppositeDirectionUp();
                break;
            case "Off U":
                DropoutItem = "D";
                CheckOppositeDirectionDown();
                break;
            case "Off R":
                DropoutItem = "L";
                CheckOppositeDirectionLeft();
                break;
            case "Off L":
                DropoutItem = "R";
                CheckOppositeDirectionRight();
                break;
        }
        // continuation .. 
    }

    #region check for passage in the room

    void CheckOppositeDirectionUp()
    {
        if (gameObject.name == "SpawnPoint U") spawnRoom.SpawnMarkTwo(roomTemplates.DownRooms);
        else
        {
            spawnRoom.Act = "activation for U";
            MainFilter();
        }
    }

    void CheckOppositeDirectionDown()
    {
        if (gameObject.name == "SpawnPoint D") spawnRoom.SpawnMarkTwo(roomTemplates.UpRooms);
        else
        {
            spawnRoom.Act = "activation for D";
            MainFilter();
        }
    }

    void CheckOppositeDirectionLeft()
    {
        if (gameObject.name == "SpawnPoint L") spawnRoom.SpawnMarkTwo(roomTemplates.RightRooms);
        else
        {
            spawnRoom.Act = "activation for L";
            MainFilter();
        }
    }

    void CheckOppositeDirectionRight()
    {
        if (gameObject.name == "SpawnPoint R") spawnRoom.SpawnMarkTwo(roomTemplates.LeftRooms);
        else
        {
            spawnRoom.Act = "activation for R";
            MainFilter();
        }
    }

    #endregion

    void MainFilter()
    {
        int endNumber = 0;
        for (int index = 0; index < UnfilteredList.Count; index++)
        {
            endNumber = UnfilteredList[index].name.Length - 5;
            if (!UnfilteredList[index].name.Substring(5, endNumber).Contains(DropoutItem))
            {
                spawnRoom.AddToSpawnList(UnfilteredList[index]);
            }
        }
    }
    
    #endregion
}
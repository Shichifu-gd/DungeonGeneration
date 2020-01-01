using UnityEngine;

public class RoomTemplates : MonoBehaviour
{
    public ListOfRoomsThatAppeared listOfRoomsThatAppeared;

    public SORoomS OtherRooms;

    public SORoomWithOneExit RoomWithOneExit;
    public SORoomWithOneExit LongRoomWithOneExit;

    public SORoomTemplates StandardRoomTemplates;
    public SORoomTemplates LongRoomTemplates;

    private float waitingForTimeToExit;

    private bool SpawnExite = false;

    private void FixedUpdate()
    {
        if (waitingForTimeToExit <= 0 && SpawnExite == false)
        {
            PassageToTheNextLevel();
            SpawnExite = true;
        }
        else waitingForTimeToExit -= Time.deltaTime;
    }

    public void PassageToTheNextLevel()
    {
        GameObject spawnPosition = listOfRoomsThatAppeared.IdentifyLastRoom();
        Instantiate(OtherRooms.ObjectExite, spawnPosition.transform.position, Quaternion.identity);
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

    #region Standard

    public GameObject[] SelectStandardRooms(string spawnPoint)
    {
        GameObject[] defaultSpawnList = null;
        if (spawnPoint == "SpawnPoint U") defaultSpawnList = StandardRoomTemplates.RoomTemplatesForSpawnPointDown.ToArray();
        if (spawnPoint == "SpawnPoint D") defaultSpawnList = StandardRoomTemplates.RoomTemplatesForSpawnPointUp.ToArray();
        if (spawnPoint == "SpawnPoint R") defaultSpawnList = StandardRoomTemplates.RoomTemplatesForSpawnPointLeft.ToArray();
        if (spawnPoint == "SpawnPoint L") defaultSpawnList = StandardRoomTemplates.RoomTemplatesForSpawnPointRight.ToArray();
        return defaultSpawnList;
    }
    #endregion

    #region Long

    /* if the room is selected for the first time, then we simply transfer all the received rooms
     * if we select a room to continue a long room, then we filter, a large hole in the direction of the room */
    public GameObject[] SelectLongRooms(string spawnPoint, string option)
    {
        GameObject[] longRoom = null;
        string side = OppositeSide(spawnPoint.Substring(spawnPoint.Length - 1));
        if (spawnPoint == "SpawnPoint U" || spawnPoint == "Long SpawnPoint D") longRoom = SearchLongRoom(LongRoomTemplates.RoomTemplatesForSpawnPointDown.ToArray(), option, side);
        if (spawnPoint == "SpawnPoint D" || spawnPoint == "Long SpawnPoint U") longRoom = SearchLongRoom(LongRoomTemplates.RoomTemplatesForSpawnPointUp.ToArray(), option, side);
        if (spawnPoint == "SpawnPoint R" || spawnPoint == "Long SpawnPoint L") longRoom = SearchLongRoom(LongRoomTemplates.RoomTemplatesForSpawnPointLeft.ToArray(), option, side);
        if (spawnPoint == "SpawnPoint L" || spawnPoint == "Long SpawnPoint R") longRoom = SearchLongRoom(LongRoomTemplates.RoomTemplatesForSpawnPointRight.ToArray(), option, side);
        return longRoom;
    }

    private string OppositeSide(string side)
    {
        string value = null;
        if (side == "D") value = "U";
        if (side == "U") value = "D";
        if (side == "L") value = "R";
        if (side == "R") value = "L";
        return value;
    }

    private GameObject[] SearchLongRoom(GameObject[] arrayRooms, string option, string side)
    {
        GameObject[] allRooms = arrayRooms;
        GameObject[] foundRoom = null;
        int arraySize = allRooms.Length;
        bool searchSwitch = false;
        if (option == "with") searchSwitch = false;
        else searchSwitch = true;
        if (searchSwitch == false)
        {
            for (int i = 0; i < allRooms.Length; i++)
            {
                if (allRooms[i].name.Substring(allRooms[i].name.Length - 1) != side)
                {
                    allRooms[i] = null;
                    arraySize--;
                }
            }
            int localIndex = 0;
            foundRoom = new GameObject[arraySize];
            for (int i = 0; i < allRooms.Length; i++)
            {
                if (allRooms[i] != null)
                {
                    foundRoom[localIndex] = allRooms[i];
                    localIndex++;
                }
            }
            allRooms = null;
        }
        else foundRoom = allRooms;
        return foundRoom;
    }
    #endregion

    #region Other

    public GameObject SelectLastRoom(string spawnPoint, string version)
    {
        GameObject spawnLastRoom = null;
        SORoomWithOneExit localLink = null;
        if (version == "standard") localLink = RoomWithOneExit;
        else localLink = LongRoomWithOneExit;
        if (spawnPoint == "SpawnPoint U") spawnLastRoom = localLink.DownRooms;
        if (spawnPoint == "SpawnPoint D") spawnLastRoom = localLink.UpRooms;
        if (spawnPoint == "SpawnPoint R") spawnLastRoom = localLink.LeftRooms;
        if (spawnPoint == "SpawnPoint L") spawnLastRoom = localLink.RightRooms;
        return spawnLastRoom;
    }

    public GameObject SelectFirstRoom()
    {
        GameObject spawnFirstRoom = null;
        int randomRoom = Random.Range(0, 4);
        if (randomRoom == 0) spawnFirstRoom = RoomWithOneExit.DownRooms;
        if (randomRoom == 1) spawnFirstRoom = RoomWithOneExit.UpRooms;
        if (randomRoom == 2) spawnFirstRoom = RoomWithOneExit.LeftRooms;
        if (randomRoom == 3) spawnFirstRoom = RoomWithOneExit.RightRooms;
        return spawnFirstRoom;
    }

    public GameObject Crossroads()
    {
        GameObject crossroads = OtherRooms.CrossroadsRooms;
        return crossroads;
    }
    #endregion
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnRoom : MonoBehaviour
{
    ListOfRoomsThatAppeared listOfRoomsThatAppeared;
    RoomTemplates roomTemplates;

    int RandomIndex;

    public string Act { get; set; }

    bool WhetherSpawnIsAllowed = true;

    [SerializeField] List<GameObject> SpawnList;

    private void Awake()
    {
        roomTemplates = GameObject.FindGameObjectWithTag("RoomTemplates").GetComponent<RoomTemplates>();
        listOfRoomsThatAppeared = roomTemplates.listOfRoomsThatAppeared;
    }    

    public void AddToSpawnList(GameObject gameObject)
    {
        SpawnList.Add(gameObject);
    }

    #region Spawn room

    /* (Part one) At the beginning | SpawnPoint | we are waiting for a certain time for which to happen:
     *  - trigger triggering to check for suitable spawn rooms.
     *  - if the trigger does not work, then the default solution will be applied.
     * (Part two) After waiting, there is a redirect to | SpawnPoint/SpawnStart |:
     *  - and if the trigger fires, then we will spawn already filtered rooms.
     *  - or we will spawn the rooms that we have by default. */

    public IEnumerator DelayForFilter()
    {
        float extraTime = ExtraTime();
        yield return new WaitForSeconds(0.2f + extraTime);
        CheckIfFilterWasRunning();
    }

    float ExtraTime()
    {
        float time = 0;
        if (gameObject.name == "SpawnPoint U") time = 0.1f;
        if (gameObject.name == "SpawnPoint D") time = 0.2f;
        if (gameObject.name == "SpawnPoint R") time = 0.3f;
        if (gameObject.name == "SpawnPoint L") time = 0.4f;
        return time;
    }

    void CheckIfFilterWasRunning()
    {
        Act = Act ?? "default action";
        int size = listOfRoomsThatAppeared.CountList();
        int maximumNumberOfRooms = listOfRoomsThatAppeared.MaximumNumberOfRooms;
        if (WhetherSpawnIsAllowed == true && size < maximumNumberOfRooms)
        {
            if (Act == "default action") SpawnDefault();
            else SpawnWihsUsingFilter();
        }
        else ClosesOpenRooms();
        Destroy(gameObject);
    }

    void SpawnWihsUsingFilter()
    {
        RandomIndex = Random.Range(0, SpawnList.Count - 1);
        Instantiate(SpawnList[RandomIndex], transform.position, SpawnList[RandomIndex].transform.rotation);
        SpawnMarkOne();
    }

    void SpawnDefault()
    {
        List<GameObject> DefaultSpawnList = null;
        WhetherSpawnIsAllowed = false;        
        DefaultSpawnList = roomTemplates.TheFormationOf(DefaultSpawnList, gameObject.name);
        RandomIndex = Random.Range(0, DefaultSpawnList.Count);
        Instantiate(DefaultSpawnList[RandomIndex], transform.position, DefaultSpawnList[RandomIndex].transform.rotation);
    }

    public void ClosesOpenRooms()
    {
        if (WhetherSpawnIsAllowed == true)
        {
            Instantiate(roomTemplates.ObjectClosedRoom, transform.position, Quaternion.identity);
            roomTemplates.listOfRoomsThatAppeared.gameLog.ForSpawnPoint();
        }
        WhetherSpawnIsAllowed = false;
    }   

    // temporarily ->
    void SpawnMarkOne()
    {
       // Instantiate(roomTemplates.ObjectError, transform.position, roomTemplates.ObjectError.transform.rotation);
    }
    // <|

    // temporarily ->
    // spawn room 
    public void SpawnMarkTwo(GameObject room)
    {
        Instantiate(room, transform.position, room.transform.rotation);
        Instantiate(roomTemplates.ObjectError, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
    // <|

    #endregion
}
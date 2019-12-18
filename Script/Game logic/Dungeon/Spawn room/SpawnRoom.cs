using UnityEngine.SceneManagement;
using System.Collections.Generic;
using System.Collections;
using UnityEngine;

public class SpawnRoom : MonoBehaviour
{
    /* (Part one) At the beginning | SpawnPoint | we are waiting for a certain time for which to happen:
     *  - trigger triggering to check for suitable spawn rooms.
     *  - if the trigger does not work, then the default solution will be applied.
     * (Part two) After waiting, there is a redirect to | SpawnPoint/SpawnStart |:
     *  - and if the trigger fires, then we will spawn already filtered rooms.
     *  - or we will spawn the rooms that we have by default. */

    [SerializeField] ListOfRoomsThatAppeared listOfRoomsThatAppeared;
    [SerializeField] RoomTemplates roomTemplates;

    int RandomIndex;
    int GlobalIndex;
    int CurrentSizeList;
    int MaximumNumberRooms;

    float extraTime;

    string CurrentPositionStatus;
    string CurrentNameSpawnPoint;
    string CurrentRoomVersion;

    [SerializeField] bool SwitchForObscurity;
    [SerializeField]
    bool WhetherSpawnIsAllowed = true;
    bool pauseSpawnRoom;

    [SerializeField] GameObject SpawnZone;
    [SerializeField] GameObject CurrentPosition;

    [SerializeField] GameObject[] SpawnList;

    [SerializeField] List<GameObject> SpawnPointList;

    void Start()
    {
        MaximumNumberRooms = listOfRoomsThatAppeared.DetermineNumberRooms();
    }

    void FixedUpdate()
    {
        if (SpawnPointList.Count > 0 && WhetherSpawnIsAllowed == true && pauseSpawnRoom == false)
        {
            WhetherSpawnIsAllowed = false;
            StartCoroutine(DelayForFilter());
        }
        if (SpawnPointList.Count == 0 &&
            CurrentSizeList >= MaximumNumberRooms &&
            SwitchForObscurity == false && pauseSpawnRoom == false) StartCoroutine(FormBackground());
    }

    #region assignment

    public bool PauseSpawnRoom()
    {
        pauseSpawnRoom = !pauseSpawnRoom;
        return pauseSpawnRoom;
    }

    public float ExtraTime
    {
        set
        {
            if (value > 0 && value < 20) extraTime = value;
            else extraTime = .2f;
        }
    }

    #endregion

    public IEnumerator DelayForFilter()
    {
        SpawnZone.SetActive(false);
        yield return new WaitForSeconds(extraTime);
        DetermineCurrentSpawnPosition();
        CheckIfFilterWasRunning();
        Destroy(CurrentPosition);
        WhetherSpawnIsAllowed = true;
    }

    void DetermineCurrentSpawnPosition()
    {
        SpawnPointList.RemoveAll(x => x == null);
        if (SpawnPointList.Count > 0)
        {
            SpawnZone.SetActive(true);
            CurrentPositionStatus = SpawnPointList[0].GetComponent<SpawnFiltr>().PositionStatus;
            CurrentPosition = SpawnPointList[0];
            AddToSpawnList();
        }
    }

    #region Add to Lists

    void AddToSpawnList()
    {
        SpawnList = CurrentPosition.GetComponent<SpawnFiltr>().GetFilteredList();
    }

    public void AddOneSpawnPointToList(GameObject spawnPoint)
    {
        SpawnPointList.Add(spawnPoint);
    }

    #endregion

    #region Distributor

    void CheckIfFilterWasRunning()
    {
        FindOutSizeOfList();
        if (CurrentSizeList < MaximumNumberRooms)
        {
            if (CurrentPosition != null) Distributor();
        }
        else SwitchingBetweenOptions();
    }

    void FindOutSizeOfList()
    {
        CurrentSizeList = listOfRoomsThatAppeared.CountList();
    }

    void Distributor()
    {
        if (CurrentPosition != null)
        {
            CurrentNameSpawnPoint = CurrentPosition.GetComponent<SpawnPoint>().NameSpawnPoint;
            CurrentRoomVersion = CurrentPosition.GetComponent<SpawnPoint>().RoomVersion;
            if (CurrentPositionStatus == "default action" && CurrentPosition.name.Substring(11, 1) == "0") SpawnSwitch();
            if (CurrentPositionStatus == "default action" && CurrentPosition.name.Substring(11, 1) == "1") SpawnContinuationLongRoom();

            if (CurrentPositionStatus == "activation for U" && CurrentPosition.name.Substring(11, 1) == "0") SpawnWihsUsingFilter();
            if (CurrentPositionStatus == "activation for D" && CurrentPosition.name.Substring(11, 1) == "0") SpawnWihsUsingFilter();
            if (CurrentPositionStatus == "activation for R" && CurrentPosition.name.Substring(11, 1) == "0") SpawnWihsUsingFilter();
            if (CurrentPositionStatus == "activation for L" && CurrentPosition.name.Substring(11, 1) == "0") SpawnWihsUsingFilter();
            /* not available
            if (CurrentPositionStatus == "activation for U" && CurrentPosition.name.Substring(11, 1) == "1") not available ...
            if (CurrentPositionStatus == "activation for D" && CurrentPosition.name.Substring(11, 1) == "1") not available ..
            if (CurrentPositionStatus == "activation for R" && CurrentPosition.name.Substring(11, 1) == "1") not available .....
            if (CurrentPositionStatus == "activation for L" && CurrentPosition.name.Substring(11, 1) == "1") not available .
            */
            if (CurrentPositionStatus == "dead end") ClosesOpenRoomsSecondWay(CurrentPosition);
        }
    }

    #endregion

    #region Spawn room

    void SpawnSwitch()
    {
        int randomNumber = Random.Range(0, 30);
        if (randomNumber == 2 || randomNumber == 7) SpawnCrossroadsRoom();
        else
        {
            if (randomNumber == 3 || randomNumber == 8) SpawnLongRoom();
            else SpawnDefaultRoom();
        }
    }

    void SpawnCrossroadsRoom()
    {
        GameObject crossroad = roomTemplates.Crossroads();
        Spawn(crossroad, CurrentPosition.transform.position);
        crossroad = null;
    }

    void SpawnLongRoom()
    {
        GameObject[] longRoom = roomTemplates.SelectLongRoom(CurrentNameSpawnPoint);
        if (CurrentNameSpawnPoint == "SpawnPoint D" || CurrentNameSpawnPoint == "SpawnPoint U" || CurrentNameSpawnPoint == "SpawnPoint R" || CurrentNameSpawnPoint == "SpawnPoint L")
        {
            RandomIndex = Random.Range(0, longRoom.Length);
            Spawn(longRoom[0], CurrentPosition.transform.position);
            longRoom = null;
        }
        else SpawnDefaultRoom();
    }

    void SpawnDefaultRoom()
    {
        CurrentNameSpawnPoint = CurrentPosition.GetComponent<SpawnPoint>().NameSpawnPoint;
        GameObject[] defaultSpawnList = roomTemplates.SelectRoom(CurrentNameSpawnPoint);
        RandomIndex = Random.Range(0, defaultSpawnList.Length);
        Spawn(defaultSpawnList[RandomIndex], CurrentPosition.transform.position);
        defaultSpawnList = null;
    }

    void SpawnContinuationLongRoom()
    {
        GameObject[] longRoom = roomTemplates.SelectLongRoom("Long " + CurrentNameSpawnPoint);
        RandomIndex = Random.Range(0, longRoom.Length);
        Spawn(longRoom[0], CurrentPosition.transform.position);
    }

    void SpawnWihsUsingFilter()
    {
        RandomIndex = Random.Range(0, SpawnList.Length);
        Spawn(SpawnList[RandomIndex], CurrentPosition.transform.position);
    }

    void SwitchingBetweenOptions()
    {
        int randomSwitch = Random.Range(0, 10);
        if (randomSwitch == 2 || randomSwitch == 7) ClosesOpenRoomsSecondWay(CurrentPosition);
        else ClosesOpenRoomsFirstWay();
    }

    void ClosesOpenRoomsSecondWay(GameObject spawnPoint)
    {
        if (CurrentPosition != null)
        {
            GameObject lastRoom = roomTemplates.SelectLastRoom(spawnPoint.GetComponent<SpawnPoint>().NameSpawnPoint);
            Spawn(lastRoom, CurrentPosition.transform.position);
            lastRoom = null;
        }
    }

    void ClosesOpenRoomsFirstWay()
    {
        if (CurrentPosition != null) ClosesOpenRooms(CurrentPosition);
    }

    public void ClosesOpenRooms(GameObject spawnPoint)
    {
        Spawn(roomTemplates.ObjectClosedRoom, spawnPoint.transform.position);
        roomTemplates.listOfRoomsThatAppeared.gameLog.ForSpawnPoint();
        Destroy(spawnPoint);
    }

    void Spawn(GameObject room, Vector3 spawnPoint)
    {
        SpawnZone.transform.position = spawnPoint;
        Instantiate(room, spawnPoint, Quaternion.identity);
    }

    #endregion

    #region For test (delet me)

    void MarkPoint(GameObject spawnPoint)
    {
        Instantiate(roomTemplates.ObjectError, spawnPoint.transform.position, Quaternion.identity);
    }

    #endregion

    #region End spawn and hide the rooms

    IEnumerator FormBackground()
    {
        SwitchForObscurity = true;
        yield return new WaitForSeconds(.6f);
        GameObject[] appearedRooms = listOfRoomsThatAppeared.ListTransfer().ToArray();
        for (int index = 1; index < appearedRooms.Length; index++)
        {
            Instantiate(roomTemplates.Background, appearedRooms[index].transform.position, Quaternion.identity);
        }
        appearedRooms = null;
        // temporarily ->
        yield return new WaitForSeconds(.5f);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        // <| (test time (delete me))
    }

    #endregion
}
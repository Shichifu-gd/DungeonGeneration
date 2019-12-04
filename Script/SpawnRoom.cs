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

    string CurrentPositionStatus;

    [SerializeField]
    bool SwitchForObscurity;
    bool WhetherSpawnIsAllowed = true;

    [SerializeField] GameObject CurrentPosition;

    [SerializeField] GameObject[] SpawnList;
    [SerializeField] List<GameObject> SpawnPointList;

    void Start()
    {
        MaximumNumberRooms = listOfRoomsThatAppeared.DetermineNumberRooms();
    }

    void FixedUpdate()
    {
        if (SpawnPointList.Count > 0 && WhetherSpawnIsAllowed == true)
        {
            WhetherSpawnIsAllowed = false;
            StartCoroutine(DelayForFilter());
        }
        if (SpawnPointList.Count == 0 &&
            CurrentSizeList >= MaximumNumberRooms &&
            SwitchForObscurity == false) StartCoroutine(FormBackground());
    }

    public IEnumerator DelayForFilter()
    {
        float extraTime = .2f;
        yield return new WaitForSeconds(extraTime);
        DetermineCurrentSpawnPosition();
        CheckIfFilterWasRunning();
        Destroy(CurrentPosition);
    }

    void DetermineCurrentSpawnPosition()
    {
        SpawnPointList.RemoveAll(x => x == null);
        if (SpawnPointList.Count > 0)
        {
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
            else WhetherSpawnIsAllowed = true;
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
            if (CurrentPositionStatus == "default action") SpawnDefault();
            if (CurrentPositionStatus == "activation for U") SpawnWihsUsingFilter();
            if (CurrentPositionStatus == "activation for D") SpawnWihsUsingFilter();
            if (CurrentPositionStatus == "activation for R") SpawnWihsUsingFilter();
            if (CurrentPositionStatus == "activation for L") SpawnWihsUsingFilter();
            if (CurrentPositionStatus == "dead end") ClosesOpenRoomsSecondWay(CurrentPosition);
        }
    }

    #endregion

    #region Spawn room

    void SpawnDefault()
    {
        int randomNumber = Random.Range(0, 15);
        if (randomNumber == 2 || randomNumber == 7)
        {
            GameObject crossroads = roomTemplates.Crossroads();
            Instantiate(crossroads, CurrentPosition.transform.position, crossroads.transform.rotation);
            crossroads = null;
        }
        else
        {
            GameObject[] DefaultSpawnList = roomTemplates.TheFormationOf(CurrentPosition.name);
            RandomIndex = Random.Range(0, DefaultSpawnList.Length - 1);
            Instantiate(DefaultSpawnList[RandomIndex], CurrentPosition.transform.position, DefaultSpawnList[RandomIndex].transform.rotation);
            DefaultSpawnList = null;
        }
        WhetherSpawnIsAllowed = true;
    }

    void SpawnWihsUsingFilter()
    {
        RandomIndex = Random.Range(0, SpawnList.Length - 1);
        Instantiate(SpawnList[RandomIndex], CurrentPosition.transform.position, Quaternion.identity);
        WhetherSpawnIsAllowed = true;
    }

    void SwitchingBetweenOptions()
    {
        WhetherSpawnIsAllowed = true;
        int randomSwitch = Random.Range(0, 10);
        if (randomSwitch == 2 || randomSwitch == 7) ClosesOpenRoomsSecondWay(CurrentPosition);
        else ClosesOpenRoomsFirstWay();
        WhetherSpawnIsAllowed = true;
    }

    void ClosesOpenRoomsSecondWay(GameObject spawnPoint)
    {
        if (CurrentPosition != null)
        {
            GameObject lastRoom = roomTemplates.SpawnLastRoom(spawnPoint.name);
            Instantiate(lastRoom, spawnPoint.transform.position, spawnPoint.transform.rotation);
            MarkPoint(spawnPoint);
            lastRoom = null;
        }
    }

    void ClosesOpenRoomsFirstWay()
    {
        if (CurrentPosition != null) ClosesOpenRooms(CurrentPosition);
    }

    public void ClosesOpenRooms(GameObject spawnPoint)
    {
        Instantiate(roomTemplates.ObjectClosedRoom, spawnPoint.transform.position, Quaternion.identity);
        roomTemplates.listOfRoomsThatAppeared.gameLog.ForSpawnPoint();
        Destroy(spawnPoint);
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
        // <| (test time)
    }

    #endregion
}
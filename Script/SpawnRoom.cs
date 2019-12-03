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
    int CurrentIndex = 0;

    string CurrentPositionStatus;

    bool WhetherSpawnIsAllowed = true;
    bool SwitchForObscurity;

    GameObject CurrentPosition;

    [SerializeField] List<GameObject> SpawnList;
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
        float extraTime = .3f;
        yield return new WaitForSeconds(extraTime);
        DetermineCurrentSpawnPosition();
        CheckIfFilterWasRunning();
        Destroy(CurrentPosition.gameObject);
    }

    void DetermineCurrentSpawnPosition()
    {
        SpawnPointList.RemoveAll(x => x == null);
        if (SpawnPointList.Count > 0)
        {
            CurrentPosition = SpawnPointList[0];
            CurrentPositionStatus = CurrentPosition.GetComponent<SpawnFiltr>().PositionStatus;
            AddToSpawnList();
        }
    }

    #region Add to Lists

    void AddToSpawnList()
    {
        SpawnList = CurrentPosition.GetComponent<SpawnFiltr>().FilteredList();
    }

    public void AddOneSpawnPointToList(GameObject spawnPoint)
    {
        SpawnPointList.Add(spawnPoint);
    }

    #endregion

    void CheckIfFilterWasRunning()
    {
        CheckList();
        if (CurrentSizeList < MaximumNumberRooms)
        {
            if (CurrentPosition != null)
            {
                if (CurrentPositionStatus == "default action") SpawnDefault();
                else SpawnWihsUsingFilter();
            }
            else WhetherSpawnIsAllowed = true;
            return;
        }
        else SwitchingBetweenOptions();
    }

    void CheckList()
    {
        CurrentSizeList = listOfRoomsThatAppeared.CountList();
    }

    #region Spawn room

    void SpawnDefault()
    {
        List<GameObject> DefaultSpawnList = roomTemplates.TheFormationOf(CurrentPosition.name);
        RandomIndex = Random.Range(0, DefaultSpawnList.Count - 1);
        Instantiate(DefaultSpawnList[RandomIndex], CurrentPosition.transform.position, DefaultSpawnList[RandomIndex].transform.rotation);
        WhetherSpawnIsAllowed = true;
    }

    void SpawnWihsUsingFilter()
    {
        if (SpawnList.Count > 1) RandomIndex = Random.Range(0, SpawnList.Count - 1);
        else RandomIndex = 0;
        // FixMe -> (in rare cases, no value is assigned in (SpawnList))
        if (SpawnList.Count > 0) Instantiate(SpawnList[RandomIndex], CurrentPosition.transform.position, Quaternion.identity);
        else
        {
            Debug.Log("error");
            ClosesOpenRoomsSecondWay();
        }
        // <|
        WhetherSpawnIsAllowed = true;
    }

    void SwitchingBetweenOptions()
    {
        int randomSwitch = Random.Range(0, 10);
        if (randomSwitch == 2 || randomSwitch == 7) ClosesOpenRoomsSecondWay();
        else ClosesOpenRoomsFirstWay();
    }

    void ClosesOpenRoomsFirstWay()
    {
        if (CurrentPosition != null)
        {
            Instantiate(roomTemplates.ObjectClosedRoom, CurrentPosition.transform.position, Quaternion.identity);
            roomTemplates.listOfRoomsThatAppeared.gameLog.ForSpawnPoint();
            WhetherSpawnIsAllowed = true;
        }
        else return;
    }

    public void ClosesOpenRoomsSecondWay()
    {
        if (CurrentPosition != null)
        {
            GameObject lastRoom = roomTemplates.SpawnLastRoom(CurrentPosition.name);
            Instantiate(lastRoom, CurrentPosition.transform.position, CurrentPosition.transform.rotation);
            Instantiate(roomTemplates.ObjectError, CurrentPosition.transform.position, Quaternion.identity); // DeleteMe
            WhetherSpawnIsAllowed = true;
        }
        else return;
    }

    public void ClosesOpenRoomsThirdWay(GameObject spawnPoint)
    {
        Instantiate(roomTemplates.ObjectClosedRoom, spawnPoint.transform.position, Quaternion.identity);
        Destroy(spawnPoint);
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
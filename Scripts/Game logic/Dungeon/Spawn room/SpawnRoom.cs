using UnityEngine.SceneManagement;
using System.Collections.Generic;
using System.Collections;
using UnityEngine;

public class SpawnRoom : MonoBehaviour
{
    public ListOfRoomsThatAppeared listOfRoomsThatAppeared;
    public RoomTemplates roomTemplates;

    private float extraTime;

    private int RandomIndex;
    private int GlobalIndex;
    private int CurrentSizeList;
    private int MaximumNumberRooms;

    private string CurrentPositionStatus;
    private string CurrentNameSpawnPoint;
    private string CurrentRoomVersion;

    [SerializeField] private bool SwitchForObscurity;
    [SerializeField]
    private bool WhetherSpawnIsAllowed = true;
    private bool pauseSpawnRoom;

    [SerializeField] private GameObject SpawnZone;
    [SerializeField] private GameObject CurrentPosition;

    [SerializeField] private GameObject[] SpawnList;

    [SerializeField] private List<GameObject> SpawnPointList;

    /* (Part one) At the beginning | SpawnPoint | we are waiting for a certain time for which to happen:
     *  - trigger triggering to check for suitable spawn rooms.
     *  - if the trigger does not work, then the default solution will be applied.
     * (Part two) After waiting, there is a redirect to | SpawnPoint/SpawnStart |:
     *  - and if the trigger fires, then we will spawn already filtered rooms.
     *  - or we will spawn the rooms that we have by default. */

    private void Start()
    {
        MaximumNumberRooms = listOfRoomsThatAppeared.DetermineNumberRooms();
    }

    private void FixedUpdate()
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

    public IEnumerator DelayForFilter()
    {
        SpawnZone.SetActive(false);
        yield return new WaitForSeconds(extraTime);
        DetermineCurrentSpawnPosition();
        CheckIfFilterWasRunning();
        Destroy(CurrentPosition);
        WhetherSpawnIsAllowed = true;
    }

    private void DetermineCurrentSpawnPosition()
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

    #region Add to Lists

    private void AddToSpawnList()
    {
        SpawnList = CurrentPosition.GetComponent<SpawnFiltr>().GetFilteredList();
    }

    public void AddOneSpawnPointToList(GameObject spawnPoint)
    {
        SpawnPointList.Add(spawnPoint);
    }
    #endregion


    #region Distributor

    private void CheckIfFilterWasRunning()
    {
        FindOutSizeOfList();
        if (CurrentSizeList < MaximumNumberRooms && CurrentPosition != null) Distributor();
        else SwitchingBetweenOptions();
    }

    private void FindOutSizeOfList()
    {
        CurrentSizeList = listOfRoomsThatAppeared.CountList();
    }

    private void Distributor()
    {
        if (CurrentPosition != null)
        {
            CurrentNameSpawnPoint = CurrentPosition.GetComponent<SpawnPoint>().NameSpawnPoint;
            CurrentRoomVersion = CurrentPosition.GetComponent<SpawnPoint>().RoomVersion;
            if (CurrentPositionStatus == "default action" && CurrentPosition.name.Substring(11, 1) == "0") SpawnSwitch();
            if (CurrentPositionStatus == "default action" && CurrentPosition.name.Substring(11, 1) == "1") SpawnContinuationLongRoom();
            if (CurrentPositionStatus == "activation action" && CurrentPosition.name.Substring(11, 1) == "0") SpawnWihsUsingFilter();
            if (CurrentPositionStatus == "activation action" && CurrentPosition.name.Substring(11, 1) == "1") SpawnWihsUsingFilter();
            if (CurrentPositionStatus == "dead end first way") ClosesOpenRoomsSecondWay(CurrentPosition);
            if (CurrentPositionStatus == "dead end second way") ClosesOpenRoomsFirstWay();
        }
    }
    #endregion

    #region Spawn room

    /* --SpawnSwitch--
     * crossroad
     * long
     * default */
    private void SpawnSwitch()
    {
        int randomNumber = Random.Range(0, 30);
        if (randomNumber == 2 || randomNumber == 7) SpawnCrossroadsRoom();
        else
        {
            if (randomNumber == 3 || randomNumber == 5 || randomNumber == 8 || randomNumber == 15) SpawnLongRoom();
            else SpawnDefaultRoom();
        }
    }

    private void SpawnCrossroadsRoom()
    {
        GameObject crossroad = roomTemplates.Crossroads();
        Spawn(crossroad, CurrentPosition.transform.position);
        crossroad = null;
    }

    private void SpawnLongRoom()
    {
        SpawnList = roomTemplates.SelectLongRooms(CurrentNameSpawnPoint, "without");
        if (CurrentNameSpawnPoint == "SpawnPoint D" || CurrentNameSpawnPoint == "SpawnPoint U" || CurrentNameSpawnPoint == "SpawnPoint R" || CurrentNameSpawnPoint == "SpawnPoint L")
        {
            RandomIndex = Random.Range(0, SpawnList.Length);
            Spawn(SpawnList[RandomIndex], CurrentPosition.transform.position);
        }
        else SpawnDefaultRoom();
    }

    private void SpawnDefaultRoom()
    {
        CurrentNameSpawnPoint = CurrentPosition.GetComponent<SpawnPoint>().NameSpawnPoint;
        GameObject[] defaultSpawnList = roomTemplates.SelectStandardRooms(CurrentNameSpawnPoint);
        RandomIndex = Random.Range(0, defaultSpawnList.Length);
        Spawn(defaultSpawnList[RandomIndex], CurrentPosition.transform.position);
        defaultSpawnList = null;
    }

    private void SpawnContinuationLongRoom()
    {
        SpawnList = roomTemplates.SelectLongRooms("Long " + CurrentNameSpawnPoint, "with");
        RandomIndex = Random.Range(0, SpawnList.Length);
        Spawn(SpawnList[RandomIndex], CurrentPosition.transform.position);
    }

    private void SpawnWihsUsingFilter()
    {
        RandomIndex = Random.Range(0, SpawnList.Length);
        if (SpawnList[RandomIndex] != null) Spawn(SpawnList[RandomIndex], CurrentPosition.transform.position);
        else ClosesOpenRoomsFirstWay();
    }

    private void SwitchingBetweenOptions()
    {
        int randomSwitch = Random.Range(0, 10);
        if (CurrentPosition) CurrentRoomVersion = CurrentPosition.GetComponent<SpawnPoint>().RoomVersion;
        if (CurrentRoomVersion.Contains("standard"))
        {
            if (randomSwitch == 2 || randomSwitch == 7) ClosesOpenRoomsSecondWay(CurrentPosition);
            else ClosesOpenRoomsFirstWay();
        }
        else if (CurrentPosition) Spawn(roomTemplates.OtherRooms.ObjectClosedLongRoom, CurrentPosition.transform.position);
    }

    private void ClosesOpenRoomsSecondWay(GameObject spawnPoint)
    {
        if (CurrentPosition != null)
        {
            GameObject lastRoom = roomTemplates.SelectLastRoom(spawnPoint.GetComponent<SpawnPoint>().NameSpawnPoint, spawnPoint.GetComponent<SpawnPoint>().RoomVersion);
            Spawn(lastRoom, CurrentPosition.transform.position);
            lastRoom = null;
        }
    }

    private void ClosesOpenRoomsFirstWay()
    {
        if (CurrentPosition != null && CurrentPosition.GetComponent<SpawnPoint>().RoomVersion == "standard") ClosesOpenRooms(CurrentPosition);
        else if (CurrentPosition != null && CurrentPosition.GetComponent<SpawnPoint>().RoomVersion == "long") ClosesOpenRoomsf(CurrentPosition);
    }

    public void ClosesOpenRooms(GameObject spawnPoint)
    {
        Spawn(roomTemplates.OtherRooms.ObjectClosedRoom, spawnPoint.transform.position);
        roomTemplates.listOfRoomsThatAppeared.gameLog.ForSpawnPoint();
        Destroy(spawnPoint);
    }

    public void ClosesOpenRoomsf(GameObject spawnPoint)
    {
        Spawn(roomTemplates.OtherRooms.ObjectClosedLongRoom, spawnPoint.transform.position);
        roomTemplates.listOfRoomsThatAppeared.gameLog.ForSpawnPoint();
        Destroy(spawnPoint);
    }

    private void Spawn(GameObject room, Vector3 spawnPoint)
    {
        SpawnZone.transform.position = spawnPoint;
        Instantiate(room, spawnPoint, Quaternion.identity);
    }
    #endregion

    #region End spawn and hide the rooms

    private IEnumerator FormBackground()
    {
        SwitchForObscurity = true;
        yield return new WaitForSeconds(.6f);
        GameObject[] appearedRooms = listOfRoomsThatAppeared.ListTransfer().ToArray();
        for (int index = 1; index < appearedRooms.Length; index++)
        {
            Instantiate(roomTemplates.OtherRooms.Background, appearedRooms[index].transform.position, Quaternion.identity);
        }
        appearedRooms = null;
        // temporarily ->
        yield return new WaitForSeconds(.5f);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        // <| (test time (delete me))
    }
    #endregion

    #region For test (delet me)

    private void MarkPoint(GameObject spawnPoint)
    {
        Instantiate(roomTemplates.OtherRooms.ObjectError, spawnPoint.transform.position, Quaternion.identity);
    }
    #endregion
}
using UnityEngine;

public class SpawnFiltr : MonoBehaviour
{
    private RoomTemplates roomTemplates;

    private int sizeUL;

    public string PositionStatus { get; set; }

    private string RoomThatIsOnTheSpawnSideSecond;
    private string RoomThatIsOnTheSpawnSideFirst;
    private string Option = "Standart ";
    private string SpawnDirectionName;
    private string DropoutItem;

    [SerializeField] private GameObject[] UnfilteredList;
    [SerializeField] private GameObject[] FilteredList;

    private void Awake()
    {
        roomTemplates = GameObject.FindGameObjectWithTag("RoomTemplates").GetComponent<RoomTemplates>();
    }

    private void Start()
    {
        PositionStatus = PositionStatus ?? "default action";
        SpawnDirectionName = GetComponent<SpawnPoint>().NameSpawnPoint;
    }

    #region We filter rooms for spawn

    public void StartSpawnOptions(string verifiablePositionFirst, string verifiablePositionSecond)
    {
        RoomThatIsOnTheSpawnSideFirst = verifiablePositionFirst;
        RoomThatIsOnTheSpawnSideSecond = verifiablePositionSecond;
        AddToUnfilteredList();
        RecognizeTheDropoutItem();
    }

    private void AddToUnfilteredList()
    {
        if (FilteredList.Length < 1)
        {
            if (GetComponent<SpawnPoint>().RoomVersion == "standard") UnfilteredList = roomTemplates.SelectStandardRooms(SpawnDirectionName);
            if (GetComponent<SpawnPoint>().RoomVersion == "long")
            {
                Option = "Long ";
                UnfilteredList = roomTemplates.SelectLongRooms(Option + SpawnDirectionName, "with");
            }
        }
        else UnfilteredList = FilteredList;
    }

    private void RecognizeTheDropoutItem()
    {
        switch (RoomThatIsOnTheSpawnSideFirst)
        {
            case "Off D":
                DropoutItem = "U";
                CheckOppositeDirection();
                break;
            case "Off U":
                DropoutItem = "D";
                CheckOppositeDirection();
                break;
            case "Off R":
                DropoutItem = "L";
                CheckOppositeDirection();
                break;
            case "Off L":
                DropoutItem = "R";
                CheckOppositeDirection();
                break;
        }
    }

    #region check for passage in the room

    private void CheckOppositeDirection()
    {
        if (SpawnDirectionName == "SpawnPoint " + DropoutItem && RoomThatIsOnTheSpawnSideSecond == null) PositionStatus = "dead end first way";
        else
        {
            PositionStatus = "activation action";
            MainFilter();
        }
    }

    public void statusTest()
    {
        PositionStatus = "dead end second way";
    }

    #endregion

    private void MainFilter()
    {
        PartOne();
        if (RoomThatIsOnTheSpawnSideSecond != null) PartTwo();
        Filter(sizeUL);
    }

    private void PartOne()
    {
        int endNumber = 0;
        sizeUL = UnfilteredList.Length;
        for (int index = 0; index < UnfilteredList.Length; index++)
        {
            endNumber = UnfilteredList[index].name.Length - 5;
            if (UnfilteredList[index].name.Substring(5, endNumber).Contains(DropoutItem))
            {
                sizeUL--;
                UnfilteredList[index] = null;
            }
        }
    }

    private void PartTwo()
    {
        int endNumber = 0;
        for (int index = 0; index < UnfilteredList.Length; index++)
        {
            if (UnfilteredList[index] != null)
            {
                endNumber = UnfilteredList[index].name.Length - 5;
                if (UnfilteredList[index].name.Substring(5, endNumber).Contains(RoomThatIsOnTheSpawnSideSecond))
                {
                    sizeUL--;
                    UnfilteredList[index] = null;
                }
            }
        }
    }

    private void Filter(int size)
    {
        int indexFL = 0;
        FilteredList = new GameObject[size];
        for (int index = 0; index < UnfilteredList.Length; index++)
        {
            if (UnfilteredList[index] != null)
            {
                FilteredList[indexFL] = UnfilteredList[index];
                indexFL++;
            }
        }
        UnfilteredList = null;
    }

    public GameObject[] GetFilteredList()
    {
        return FilteredList;
    }

    #endregion
}
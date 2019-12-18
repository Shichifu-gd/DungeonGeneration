using UnityEngine;

public class SpawnFiltr : MonoBehaviour
{
    RoomTemplates roomTemplates;

    public string PositionStatus { get; set; }

    string RoomThatIsOnTheSpawnSide;
    string DropoutItem;
    string SpawnDirectionName;

    [SerializeField] GameObject[] UnfilteredList;
    [SerializeField] GameObject[] FilteredList;

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

    public void StartSpawnOptions(string VerifiablePosition)
    {
        RoomThatIsOnTheSpawnSide = VerifiablePosition;
        AddToUnfilteredList();
        RecognizeTheDropoutItem();
    }

    void AddToUnfilteredList()
    {
        UnfilteredList = roomTemplates.SelectRoom(SpawnDirectionName);
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
        if (SpawnDirectionName == "SpawnPoint U") PositionStatus = "dead end";
        else
        {
            PositionStatus = "activation for U";
            MainFilter();
        }
    }

    void CheckOppositeDirectionDown()
    {
        if (SpawnDirectionName == "SpawnPoint D") PositionStatus = "dead end";
        else
        {
            PositionStatus = "activation for D";
            MainFilter();
        }
    }

    void CheckOppositeDirectionLeft()
    {
        if (SpawnDirectionName == "SpawnPoint L") PositionStatus = "dead end";
        else
        {
            PositionStatus = "activation for L";
            MainFilter();
        }
    }

    void CheckOppositeDirectionRight()
    {
        if (SpawnDirectionName == "SpawnPoint R") PositionStatus = "dead end";
        else
        {
            PositionStatus = "activation for R";
            MainFilter();
        }
    }

    #endregion

    void MainFilter()
    {
        int endNumber = 0;
        int sizeUL = UnfilteredList.Length - 1;
        for (int index = 0; index < UnfilteredList.Length - 1; index++)
        {
            endNumber = UnfilteredList[index].name.Length - 5;
            if (UnfilteredList[index].name.Substring(5, endNumber).Contains(DropoutItem))
            {
                sizeUL--;
                UnfilteredList[index] = null;
            }
        }
        Filter(sizeUL);
    }

    void Filter(int size)
    {
        int indexFL = 0;
        FilteredList = new GameObject[size];
        for (int index = 0; index < UnfilteredList.Length - 1; index++)
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
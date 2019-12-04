using UnityEngine;

public class SpawnFiltr : MonoBehaviour
{
    RoomTemplates roomTemplates;
    SpawnRoom spawnRoom;

    public string PositionStatus { get; set; }

    string RoomThatIsOnTheSpawnSide;
    string DropoutItem;

    [SerializeField] GameObject[] UnfilteredList;
    [SerializeField] GameObject[] FilteredList;

    private void Awake()
    {
        roomTemplates = GameObject.FindGameObjectWithTag("RoomTemplates").GetComponent<RoomTemplates>();
        spawnRoom = GameObject.FindGameObjectWithTag("SpawnRoom").GetComponent<SpawnRoom>();
    }

    private void Start()
    {
        PositionStatus = PositionStatus ?? "default action";
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
        UnfilteredList = roomTemplates.TheFormationOf(gameObject.name);
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
        if (gameObject.name == "SpawnPoint U") PositionStatus = "dead end";
        else
        {
            PositionStatus = "activation for U";
            MainFilter();
        }
    }

    void CheckOppositeDirectionDown()
    {
        if (gameObject.name == "SpawnPoint D") PositionStatus = "dead end";
        else
        {
            PositionStatus = "activation for D";
            MainFilter();
        }
    }

    void CheckOppositeDirectionLeft()
    {
        if (gameObject.name == "SpawnPoint L") PositionStatus = "dead end";
        else
        {
            PositionStatus = "activation for L";
            MainFilter();
        }
    }

    void CheckOppositeDirectionRight()
    {
        if (gameObject.name == "SpawnPoint R") PositionStatus = "dead end";
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
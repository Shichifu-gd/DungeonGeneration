using UnityEngine;

public class PositionCheck : MonoBehaviour
{
    public Room room;

    public int Prioritize { get; set; }

    // PNTR - surrounds the room and has a direction ..
    // EFTR - these are the exits from the room ..
    private string PositionNearTheRoom { get; set; }
    private string ExitsFromTheRoom;
    private string SideWithAisleF;
    private string SideWithAisleS;
    private string OptionsPositionCheck; // rename

    private void Start()
    {
        FigureOutPriorities();
        if (room) FormSideWithAisle();
    }

    private void FigureOutPriorities()
    {
        if (gameObject.name.Substring(14) == "U") Prioritize = 1;
        if (gameObject.name.Substring(14) == "D") Prioritize = 2;
        if (gameObject.name.Substring(14) == "R") Prioritize = 3;
        if (gameObject.name.Substring(14) == "L") Prioritize = 4;
    }

    private void FormSideWithAisle()
    {
        PositionNearTheRoom = gameObject.name.Substring(14);
        ExitsFromTheRoom = room.gameObject.name.Substring(5, 1);
        if (ExitsFromTheRoom.Contains(PositionNearTheRoom)) SideWithAisleF = "on " + PositionNearTheRoom;
        else SideWithAisleF = "Off " + PositionNearTheRoom;
    }

    /* option 1) If PositionCheck collides with SpawnPoint then SpawnPoint starts standard filtering.
     * option 2) If PositionCheck collides with the second PositionCheck then additional values ​​are assigned to one of the PositionCheck, and the second is destroyed. 
     * Then, if PositionCheck collides with SpawnPoint, then SpawnPoint first performs standard filtering after additional filtering.
     * option 3) And if PositionCheck collides with the second PositionCheck and then with the third PositionCheck then in this place there is a dead end for the spawn.  */
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("PositionCheck"))
        {
            if (SideWithAisleS == null)
            {
                if (Prioritize > other.gameObject.GetComponent<PositionCheck>().Prioritize) NeighborhoodRules(other.gameObject.GetComponent<PositionCheck>().PositionNearTheRoom);
                else Destroy(gameObject);
            }
            else
            {
                SideWithAisleS = null;
                SideWithAisleF = "dead end second way";
                Destroy(other.gameObject);
            }
        }

        if (other.CompareTag("SpawnPoint"))
        {
            if (SideWithAisleF != "dead end second way") other.gameObject.GetComponent<SpawnFiltr>().StartSpawnOptions(SideWithAisleF, SideWithAisleS);
            else other.gameObject.GetComponent<SpawnFiltr>().statusTest();
            Destroy(gameObject);
        }
        if (other.CompareTag("Room")) Destroy(gameObject);
    }

    private void NeighborhoodRules(string neighbour)
    {
        if (neighbour == "U") SideWithAisleS = "D";
        if (neighbour == "D") SideWithAisleS = "U";
        if (neighbour == "R") SideWithAisleS = "L";
        if (neighbour == "L") SideWithAisleS = "R";
    }
}
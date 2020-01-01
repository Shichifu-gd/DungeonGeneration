using UnityEngine;

public class SpawnPoint : MonoBehaviour
{
    private SpawnRoom spawnRoom;

    private int SpawnPriority { get; set; }

    public string NameSpawnPoint { get; set; }
    public string RoomVersion { get; set; }

    private void Awake()
    {
        spawnRoom = GameObject.FindGameObjectWithTag("SpawnRoom").GetComponent<SpawnRoom>();
        FormNameObject();
    }

    private void Start()
    {
        DetermineVersion();
        spawnRoom.AddOneSpawnPointToList(gameObject);
        Prioritize();
    }

    /*  --------- usage example --------
     *  SpawnPoin |version||spawn_side|
     *  SpawnPoin 01 - "standard" "Up"
     *            04 - "standard" "Left"
     *            12 - "long" "down"
     *            11 - "long" "up"
     *            14 - "long" "left"        
     * --------- version ---------------
     * standard 0
     * long 1     
     * --------- spawn_side ------------
     * 1 - up  
     * 2 - down
     * 3 - right
     * 4 - left
     * --------- working version ------- 
     * SpawnPoin 001 = "Spawn poin Up"   
     * --------- length ---------------- 
     * SpawnPoint = 10 + 1               
     * |version||spawn_side| = 2        
     * general = 13                     
     * ------------- end --------------- */
    private void DetermineVersion()
    {
        if (gameObject.name.Substring(11, 1) == "0") RoomVersion = "standard";
        if (gameObject.name.Substring(11, 1) == "1") RoomVersion = "long";
    }

    private string DefineSpawnDirection(string numberSide)
    {
        string side = null;
        if (numberSide == "1") side = "U";
        if (numberSide == "2") side = "D";
        if (numberSide == "3") side = "R";
        if (numberSide == "4") side = "L";
        return side;
    }

    private void Prioritize()
    {
        SpawnPriority = int.Parse(gameObject.name.Substring(12, 1));
    }

    private void FormNameObject()
    {
        NameSpawnPoint = "SpawnPoint " + DefineSpawnDirection(gameObject.name.Substring(12, 1));
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("SpawnPoint"))
        {
            if (SpawnPriority > other.gameObject.GetComponent<SpawnPoint>().SpawnPriority) Destroy(other.gameObject);
            CloseTheRemainingExits();
        }
    }

    private void ExcessRemoval()
    {
        Invoke("CloseTheRemainingExits", .1f);
    }

    private void CloseTheRemainingExits()
    {
        spawnRoom.ClosesOpenRooms(gameObject);
    }
}
using UnityEngine;

public class SpawnPoint : MonoBehaviour
{
    SpawnRoom spawnRoom;

    public int SpawnPriority { get; set; }

    private void Awake()
    {
        spawnRoom = GameObject.FindGameObjectWithTag("SpawnRoom").GetComponent<SpawnRoom>();
    }

    private void Start()
    {
        spawnRoom.AddOneSpawnPointToList(gameObject);
        Prioritize();
    }

    void Prioritize()
    {
        if (gameObject.name == "SpawnPoint U") SpawnPriority = 0;
        if (gameObject.name == "SpawnPoint D") SpawnPriority = 1;
        if (gameObject.name == "SpawnPoint R") SpawnPriority = 2;
        if (gameObject.name == "SpawnPoint L") SpawnPriority = 3;
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
        spawnRoom.ClosesOpenRoomsThirdWay(gameObject);
    }
}
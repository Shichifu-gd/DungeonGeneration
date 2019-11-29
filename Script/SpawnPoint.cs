using UnityEngine;

public class SpawnPoint : MonoBehaviour
{
    SpawnRoom spawnRoom;

    private void Awake()
    {
        spawnRoom = GetComponent<SpawnRoom>();
    }

    private void Start()
    {
        spawnRoom.StartCoroutine(spawnRoom.DelayForFilter());
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("SpawnPoint"))
        {
            if (gameObject.transform.position.z == 1 && other.gameObject.transform.position.z == 4) ExcessRemoval(1);
            else ExcessRemoval(2);
        }
    }   

    private void ExcessRemoval(int version)
    {
        if (version == 1) if (gameObject.transform.position.z == 0 || gameObject.transform.position.z == 2) Destroy(gameObject);
        if (version == 2) if (gameObject.transform.position.z == 1 || gameObject.transform.position.z == 4) Destroy(gameObject);
        Invoke("CloseTheRemainingExits", .1f);
    }

    private void CloseTheRemainingExits()
    {
        spawnRoom.ClosesOpenRooms();
        Destroy(gameObject);
    }
}
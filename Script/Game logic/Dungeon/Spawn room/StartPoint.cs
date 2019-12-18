using UnityEngine;

public class StartPoint : MonoBehaviour
{
    [SerializeField] GameObject roomTemplates;

    void Start()
    {
        SpawnRoom();
    }

    void SpawnRoom()
    {
        GameObject spawnFirstRoom = roomTemplates.GetComponent<RoomTemplates>().SelectFirstRoom();
        Instantiate(spawnFirstRoom, gameObject.transform.position, spawnFirstRoom.transform.rotation);
        Destroy(gameObject);
    }
}
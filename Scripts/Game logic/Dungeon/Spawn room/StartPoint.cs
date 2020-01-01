using UnityEngine;

public class StartPoint : MonoBehaviour
{
    public GameObject roomTemplates;

    private void Start()
    {
        SpawnRoom();
    }

    private void SpawnRoom()
    {
        GameObject spawnFirstRoom = roomTemplates.GetComponent<RoomTemplates>().SelectFirstRoom();
        Instantiate(spawnFirstRoom, gameObject.transform.position, spawnFirstRoom.transform.rotation);
        Destroy(gameObject);
    }
}
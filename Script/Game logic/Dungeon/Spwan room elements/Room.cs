using UnityEngine;

public class Room : MonoBehaviour
{
    ListOfRoomsThatAppeared listOfRoomsThatAppeared;

    private void Awake()
    {
        listOfRoomsThatAppeared = GameObject.FindGameObjectWithTag("ListOfRoomsThatAppeared").GetComponent<ListOfRoomsThatAppeared>();
    }

    private void Start()
    {
        listOfRoomsThatAppeared.AddToLRA(gameObject);
    }     
}
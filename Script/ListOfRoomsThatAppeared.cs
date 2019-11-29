using System.Collections.Generic;
using UnityEngine;

public class ListOfRoomsThatAppeared : MonoBehaviour
{
    public GameLog gameLog;

    public int MaximumNumberOfRooms { get; set; }

    [SerializeField] List<GameObject> RoomsAvailable;

    private void Start()
    {
        MaximumNumberOfRooms = Random.Range(7, 15);
        gameLog.ForMaxRoom(MaximumNumberOfRooms);
    }

    private void Update()
    {
        gameLog.ForListOfRoomsThatAppeared(RoomsAvailable.Count.ToString());
    }

    public void AddToLRA(GameObject objectExite)
    {
        RoomsAvailable.Add(objectExite);
    }

    public int CountList()
    {
        int result = RoomsAvailable.Count;
        return result;
    }
   
    public GameObject test()
    {       
        GameObject lastRoom = RoomsAvailable[RoomsAvailable.Count -1];
        return lastRoom;
    }
}
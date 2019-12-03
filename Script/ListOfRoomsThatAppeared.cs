using System.Collections.Generic;
using UnityEngine;

public class ListOfRoomsThatAppeared : MonoBehaviour
{
    public GameLog gameLog;

    [SerializeField] List<GameObject> RoomsAvailable;   

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

    public int DetermineNumberRooms()
    {
        int maximumNumberOfRooms = Random.Range(7, 15);
        gameLog.ForMaxRoom(maximumNumberOfRooms);
        return maximumNumberOfRooms;
    }
   
    public GameObject IdentifyLastRoom()
    {       
        GameObject lastRoom = RoomsAvailable[RoomsAvailable.Count -1];
        return lastRoom;
    }

    public List<GameObject> ListTransfer()
    {
        return RoomsAvailable;
    }
}
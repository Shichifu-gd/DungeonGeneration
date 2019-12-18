using System.Collections.Generic;
using UnityEngine;

public class ListOfRoomsThatAppeared : MonoBehaviour
{
    public GameLog gameLog;

    int maximumNumberOfRooms;
    int minimumNumberOfRooms;

    [SerializeField] List<GameObject> RoomsAvailable;

    private void Update()
    {
        gameLog.ForListOfRoomsThatAppeared(RoomsAvailable.Count.ToString());
    }

    #region assignment

    public int MaximumNumberOfRooms
    {
        set
        {
            if (value > 0) maximumNumberOfRooms = value;
            else maximumNumberOfRooms = 10;
        }
    }

    public int MinimumNumberOfRooms
    {
        set
        {
            if (value > 0 && value < maximumNumberOfRooms - 1) minimumNumberOfRooms = value;
            else minimumNumberOfRooms = 0;
        }
    }

    #endregion

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
        maximumNumberOfRooms = Random.Range(minimumNumberOfRooms, maximumNumberOfRooms);
        gameLog.ForMaxRoom(maximumNumberOfRooms);
        return maximumNumberOfRooms;
    }

    public GameObject IdentifyLastRoom()
    {
        GameObject lastRoom = RoomsAvailable[RoomsAvailable.Count - 1];
        return lastRoom;
    }

    public List<GameObject> ListTransfer()
    {
        return RoomsAvailable;
    }
}
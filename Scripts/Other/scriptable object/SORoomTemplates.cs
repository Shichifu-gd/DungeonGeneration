using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/RoomTemplates")]
public class SORoomTemplates : ScriptableObject
{
   public List<GameObject> RoomTemplatesForSpawnPointUp;
   public List<GameObject> RoomTemplatesForSpawnPointDown;
   public List<GameObject> RoomTemplatesForSpawnPointRight;
   public List<GameObject> RoomTemplatesForSpawnPointLeft;
}
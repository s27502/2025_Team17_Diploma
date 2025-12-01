using System.Collections.Generic;
using UnityEngine;

namespace DefaultNamespace
{
    [CreateAssetMenu(menuName = "Game/Floor Data")]
    public class FloorData : ScriptableObject
    {
        public string floorName;
        public List<GameObject> possibleRooms;
        public int shopRoomNumber;
        public GameObject startRoom;
        public GameObject bossRoom;
        public GameObject shopRoom;

        public int roomsToGenerate = 10;
    }

}
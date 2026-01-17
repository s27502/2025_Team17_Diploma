using System.Collections.Generic;
using UnityEngine;

namespace DefaultNamespace
{
    [CreateAssetMenu(menuName = "Game/Floor Data")]
    public class FloorData : ScriptableObject
    {
        public string floorName;
        public List<GameObject> possibleRooms;
        
        public int shopRoomNumberMin;
        public int shopRoomNumberMax;

        public int talismanRoomNumber;

        public int shrineRoomNumberMin;
        public int shrineRoomNumberMax;
        
        
        public GameObject startRoom;
        public GameObject bossRoom;
        public GameObject shopRoom;
        public GameObject shrineRoom;
        public GameObject talismanRoom;

        public int roomsToGenerate = 10;
    }

}
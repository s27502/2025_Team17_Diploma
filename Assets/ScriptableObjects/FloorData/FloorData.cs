using System.Collections.Generic;
using UnityEngine;

namespace DefaultNamespace
{
    [CreateAssetMenu(menuName = "Game/Floor Data")]
    public class FloorData : ScriptableObject
    {
        public string floorName;
        public List<GameObject> possibleRooms;
        public GameObject startRoom;
        public GameObject bossRoom;

        public int roomsToGenerate = 10;
    }

}
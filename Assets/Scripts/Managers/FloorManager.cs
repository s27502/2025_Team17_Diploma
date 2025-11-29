using System.Collections;
using System.Collections.Generic;
using DefaultNamespace;
using UnityEngine;

public class FloorManager : SingletonDoNotDestroy<FloorManager>
{
    [SerializeField] private GameObject _player;
    [SerializeField] private int _floorsToGenerate;
    private int _currentFloor = 0;

    [SerializeField] private List<FloorData> _floorDatas;
    private FloorData _currentFloorData;

    private GameObject _currentRoom;
    private int _roomCounter = 0;

    protected override void Awake()
    {
        base.Awake();
        if (Instance != this) return;
        
        ServiceLocator.Instance.Register(this);
        _currentFloorData = _floorDatas[0];
        
        _currentRoom = Instantiate(_currentFloorData.startRoom, Vector3.zero, Quaternion.identity);
        _player.transform.position = _currentRoom.GetComponent<Room>()._spawnPos.transform.position;
        _roomCounter++;

    }

    public void GoToNextRoom()
    {
        Debug.Log("should TP");


        if (_roomCounter == _currentFloorData.roomsToGenerate)
        {
            GoToNextFloor();
        }
        else
        {
            Destroy(_currentRoom);
            _currentRoom = Instantiate(PickRandomRoom(), Vector3.zero, Quaternion.identity);
            _player.transform.position = _currentRoom.GetComponent<Room>()._spawnPos.transform.position;
            _roomCounter++;
        }
    }

    private void GoToNextFloor()
    {
        throw new System.NotImplementedException();
    }

    private GameObject PickRandomRoom()
    {
        return _currentFloorData.possibleRooms[
            Random.Range(0, _currentFloorData.possibleRooms.Count)
        ];
    }

    public GameObject GetCurrentRoom()
    {
        return _currentRoom;
    }
}

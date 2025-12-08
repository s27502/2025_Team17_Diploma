using System.Collections;
using System.Collections.Generic;
using DefaultNamespace;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FloorManager : SingletonDoNotDestroy<FloorManager>
{
    [SerializeField] private GameObject _thankScreen;
    private GameObject _thankScreenInstance;
    
    [SerializeField] private GameObject _player;
    [SerializeField] private int _floorsToGenerate;
    private int _currentFloor = 0;

    [SerializeField] private List<FloorData> _floorDatas;
    private FloorData _currentFloorData;

    private GameObject _currentRoom;
    private int _shopNumber;
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
        if (_roomCounter == _currentFloorData.roomsToGenerate)
        {
            GoToNextFloor();
        }
        else
        {
            Destroy(_currentRoom);
            _roomCounter++;

            if (_roomCounter == _currentFloorData.shopRoomNumber)
            {
                _currentRoom = Instantiate(_currentFloorData.shopRoom, Vector3.zero, Quaternion.identity);
            }
            else if (_roomCounter == _currentFloorData.roomsToGenerate)
            {
                _currentRoom = Instantiate(_currentFloorData.bossRoom, Vector3.zero, Quaternion.identity);
            }
            else
            {
                _currentRoom = Instantiate(PickRandomRoom(), Vector3.zero, Quaternion.identity);
            }
            
            _player.transform.position = _currentRoom.GetComponent<Room>()._spawnPos.transform.position;
            
        }
    }
    
    private IEnumerator DeathCoroutine()
    {
        Time.timeScale = 0;
        Destroy(ServiceLocator.Instance.GetService<FloorManager>().GetCurrentRoom());
            
        _thankScreenInstance = Instantiate(_thankScreen);
            
        yield return new WaitForSecondsRealtime(5f);

        Destroy(gameObject);
        ServiceLocator.Instance.Erase();
        SceneManager.LoadScene("MainMenu");

        Time.timeScale = 1;
    }
    

    private void GoToNextFloor()
    {
        StartCoroutine(DeathCoroutine());
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

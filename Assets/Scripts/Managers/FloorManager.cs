using System.Collections;
using System.Collections.Generic;
using DefaultNamespace;
using Managers;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FloorManager : SingletonDoNotDestroy<FloorManager>
{
    [SerializeField] private AudioClip defaultMusic;
    [SerializeField] private AudioClip bossMusic;
    [SerializeField] private AudioClip shopMusic;
    
    [SerializeField] private Animator transition;
    [SerializeField] private GameObject _thankScreen;
    private GameObject _thankScreenInstance;
    
    [SerializeField] private FloorInfo floorInfo;
    
    [SerializeField] private GameObject _player;
    [SerializeField] private int _floorsToGenerate;
    private int _currentFloor = 0;
    private bool _newFloor = false;
    
    [SerializeField] private List<FloorData> _floorDatas;
    private FloorData _currentFloorData;

    private GameObject _currentRoom;
    
    private int _shopNumber;
    private int _shrineNumber;
    private int _roomCounter = 0;

    protected override void Awake()
    {
        base.Awake();
        if (Instance != this) return;
        
        ServiceLocator.Instance.Register(this);
        _currentFloorData = _floorDatas[_currentFloor];
        AudioManager.Instance.PlayMusic(defaultMusic);

        _shopNumber = RollShopRoomNumber();
        _shrineNumber = RollShrineRoomNumber();
        _currentRoom = Instantiate(_currentFloorData.startRoom, Vector3.zero, Quaternion.identity);
        _player.transform.position = _currentRoom.GetComponent<Room>()._spawnPos.transform.position;
        _roomCounter++;
        floorInfo.SetText(_currentFloorData.floorName);
        floorInfo.FlyInNOut();
    }

    private int RollShrineRoomNumber()
    {
        return Random.Range(_currentFloorData.shrineRoomNumberMin, _currentFloorData.shrineRoomNumberMax+1);
    }

    private int RollShopRoomNumber()
    {
        return Random.Range(_currentFloorData.shopRoomNumberMin, _currentFloorData.shopRoomNumberMax+1);
    }

    public IEnumerator GoToNextRoomCoroutine()
    {
        _player.GetComponent<PlayerAttack>().RebuildProjectilePool();
        if (_roomCounter == _currentFloorData.roomsToGenerate)
        {
            GoToNextFloor();
            floorInfo.SetText(_currentFloorData.floorName);
            floorInfo.FlyInNOut();
        }
        else
        {
            transition.SetTrigger("Start");
            yield return new WaitForSeconds(0.5f);
            Time.timeScale = 0;

            Destroy(_currentRoom);
            _roomCounter++;

            if (_roomCounter == _shopNumber)
            {
                AudioManager.Instance.PlayMusic(shopMusic);
                _currentRoom = Instantiate(_currentFloorData.shopRoom, Vector3.zero, Quaternion.identity);
            }
            else if (_currentFloor == _floorDatas.Count-1 && _roomCounter == _currentFloorData.nahebekuNumber)
            {
                AudioManager.Instance.PlayMusic(bossMusic);
                _currentRoom = Instantiate(_currentFloorData.nahebekuRoom, Vector3.zero, Quaternion.identity);
            }
            else if (_roomCounter == _shrineNumber)
            {
                AudioManager.Instance.PlayMusic(defaultMusic);
                _currentRoom = Instantiate(_currentFloorData.shrineRoom, Vector3.zero, Quaternion.identity);
            }
            else if (_roomCounter == _currentFloorData.roomsToGenerate)
            {
                AudioManager.Instance.PlayMusic(bossMusic);
                _currentRoom = Instantiate(_currentFloorData.bossRoom, Vector3.zero, Quaternion.identity);
            }
            else if (_roomCounter == _currentFloorData.talismanRoomNumber)
            {
                AudioManager.Instance.PlayMusic(defaultMusic);
                _currentRoom = Instantiate(_currentFloorData.talismanRoom);
            }
            else if (_newFloor)
            {
                _currentRoom = Instantiate(_currentFloorData.startRoom, Vector3.zero, Quaternion.identity);
                AudioManager.Instance.PlayMusic(defaultMusic);
                _newFloor = false;
            }
            else
            {
                AudioManager.Instance.PlayMusic(defaultMusic);
                _currentRoom = Instantiate(PickRandomRoom(), Vector3.zero, Quaternion.identity);
            }
            
            _player.transform.position = _currentRoom.GetComponent<Room>()._spawnPos.transform.position;
            
            transition.SetTrigger("End");
            Time.timeScale = 1;
            yield return new WaitForSeconds(0.5f);
        }
    }
    public void GoToNextRoom()
    {
        StartCoroutine(GoToNextRoomCoroutine());
    }
    
    private IEnumerator FinishCoroutine()
    {

        transition.SetTrigger("Start");
        yield return new WaitForSecondsRealtime(0.5f);

        Destroy(_currentRoom);
        Destroy(gameObject);
        ServiceLocator.Instance.Erase();
        
        SceneManager.LoadScene("EndingCutscene");


    }
    

    public void GoToNextFloor()
    {
        //END
        //StartCoroutine(DeathCoroutine());
        
        //ENDLESS
        _newFloor = true;
        _currentFloor ++;
        if (_currentFloor == _floorDatas.Count)
        {
            StartCoroutine(FinishCoroutine());
        }
        else
        {
            _roomCounter = 0;
            _currentFloorData = _floorDatas[_currentFloor];
            _shopNumber = RollShopRoomNumber();
            _shrineNumber = RollShrineRoomNumber();
            StartCoroutine(GoToNextRoomCoroutine());
        }
                                                    



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

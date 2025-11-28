using System.Collections;
using System.Collections.Generic;
using DefaultNamespace;
using UnityEngine;

public class Room : MonoBehaviour
{
    private FloorManager _floorManager;

    public GameObject _spawnPos;
    // Start is called before the first frame update
    void Start()
    {
        _floorManager = ServiceLocator.Instance.GetService<FloorManager>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void GoToNextRoom()
    {
        _floorManager.GoToNextRoom();
    }
}

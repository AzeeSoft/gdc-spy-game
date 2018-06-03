using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class GameManagerOld : MonoBehaviour
{
    public static GameManagerOld Instance;

    public GameObject WinScreen;
    public GameObject LoseScreen;

    private bool _isPlaying;
    private GameObject _playerGameObject;

    private PlayerControllable curPlayerControllable;

    public GameManagerOld()
    {
        Instance = this;
        _isPlaying = true;
    }

    void Awake()
    {
        _playerGameObject = GameObject.FindWithTag("Player");
    }

    void Start()
    {
        switchPlayerControlToFirstPerson();
    }

    public GameObject GetPlayerGameObject()
    {
        return _playerGameObject;
    }

    public void LogMessage(string msg)
    {
        Debug.Log(msg);
    }

    public void switchPlayerControl(PlayerControllable playerControllable)
    {
        if (curPlayerControllable)
        {
            curPlayerControllable.ReleaseControl();
        }

        curPlayerControllable = playerControllable;
        curPlayerControllable.TakeControl();
    }

    public void switchPlayerControlToFirstPerson()
    {
        PlayerControllable playerControllable = _playerGameObject.GetComponent<PlayerControllable>();
        switchPlayerControl(playerControllable);
    }
}

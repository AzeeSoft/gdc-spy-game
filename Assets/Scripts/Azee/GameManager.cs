using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public GameObject WinScreen;
    public GameObject LoseScreen;

    private bool _isPlaying;
    private GameObject _playerGameObject;

    public GameManager()
    {
        Instance = this;
        _isPlaying = true;
    }

    void Awake()
    {
        _playerGameObject = GameObject.FindWithTag("Player");
    }

    public GameObject GetPlayerGameObject()
    {
        return _playerGameObject;
    }

    public void LogMessage(string msg)
    {
        Debug.Log(msg);
    }
}

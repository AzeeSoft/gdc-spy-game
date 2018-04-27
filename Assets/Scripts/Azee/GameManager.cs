using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    private GameObject _playerGameObject;

    public GameManager()
    {
        Instance = this;
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

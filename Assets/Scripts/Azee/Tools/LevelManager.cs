using System.Collections;
using System.Collections.Generic;
using EasyButtons;
using UnityEngine;

/// <summary>
/// To be placed on every game level.
///    
/// Not implemented as a Singleton because Unity allows multiple scenes to be loaded simultaneously.
/// A singleton implementation could become catastrophic!
/// </summary>
public class LevelManager : MonoBehaviour
{
    /// <summary>
    /// A static reference to the last loaded LevelManager
    /// </summary>
    [HideInInspector] public static LevelManager Instance;

    /// <summary>
    /// Although not needed, a private reference to the GameManager, just to display the GameManager's data in the inspector.
    /// </summary>
    [SerializeField]
    private GameManager _gameManager = GameManager.Instance;

    public GameObject WinScreen;
    public GameObject LoseScreen;

    private bool _isPlaying;
    private GameObject _playerGameObject;

    private PlayerControllable curPlayerControllable;

    void OnValidate()
    {
        _gameManager.GetProfileList();  // Just to force the profile list to be loaded in the inspector.
    }

    void Awake()
    {
        Instance = this;
        
        LoadGameData();
        _gameManager.GetProfileList();  // Just to force the profile list to be loaded in the inspector.

        _isPlaying = true;

        _playerGameObject = GameObject.FindWithTag("Player");
    }

    // Use this for initialization
    void Start ()
    {
        switchPlayerControlToFirstPerson();
    }
	
	// Update is called once per frame
	void Update () {
		
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

    [Button]
    void SaveGameData()
    {
        _gameManager.SaveGameData();
    }

    [Button]
    void LoadGameData()
    {
        _gameManager.LoadGameData();
    }

    [Button]
    void ClearGameData()
    {
        _gameManager.ClearGameData(true);
    }
}

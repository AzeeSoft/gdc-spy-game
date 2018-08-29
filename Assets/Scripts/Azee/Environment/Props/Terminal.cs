using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Terminal : MonoBehaviour
{
    public string MazeLevelKey;

    public UnityEvent OnHacked;
    public UnityEvent OnFailed;

    private SceneSwitcher _sceneSwitcher;
    private InteractiveObject _interactiveObject;

    void Awake()
    {
        _sceneSwitcher = FindObjectOfType<SceneSwitcher>();
        _interactiveObject = GetComponent<InteractiveObject>();
    }

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void AttemptHacking()
    {
        MazeSceneController.MazeSceneData mazeSceneData = new MazeSceneController.MazeSceneData()
        {
            MazeLevelKey = MazeLevelKey,
            Terminal = this
        };
        _sceneSwitcher.SwitchScene("MazeScene Copy", true, mazeSceneData);
    }

    public void OnHackAttemptResultReceived(MazeSceneController.MazeSceneData mazeSceneData)
    {
        if (mazeSceneData.Result)
        {
            _interactiveObject.enabled = false;
            OnHacked.Invoke();
            Debug.Log("Hacked");
        }
        else
        {
            Debug.Log("Couldn't hack");
            OnFailed.Invoke();
        }
    }
}

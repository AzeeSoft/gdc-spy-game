using System;
using System.Collections;
using System.Collections.Generic;
using BasicTools.ButtonInspector;
using UnityEngine;

public class MazeSceneController : MonoBehaviour {

    public class MazeSceneData
    {
        public string MazeLevelKey;
        public Terminal Terminal;
        public bool Result = false;
    }

    public List<MazeLevel> MazeLevels = new List<MazeLevel>();

    [SerializeField]
    [Button("Find maze level objects in children", "FindMazeLevelsInChildren")]
    private bool _buttonFindMazeLevelsInChildren;

    private MazeSceneData _mazeSceneData;

    // Use this for initialization
    void Start () {

	}
	
	// Update is called once per frame
	void Update () {
	   
    }

    public void FindMazeLevelsInChildren()
    {
        MazeLevels.Clear();

        foreach (MazeLevel mazeLevel in GetComponentsInChildren<MazeLevel>(true))
        {
            MazeLevels.Add(mazeLevel);
        }
    }

    public void OnSceneDataReceived(object data)
    {
        MazeSceneData sceneData = data as MazeSceneData;
        if (sceneData != null)
        {
            _mazeSceneData = sceneData;

            foreach (MazeLevel mazeLevel in MazeLevels)
            {
                if (mazeLevel.Key.Equals(_mazeSceneData.MazeLevelKey))
                {
                    mazeLevel.gameObject.SetActive(true);
                    return;
                }
            }
        }
    }

    public void MazeSceneResult(bool Success)
    {
        _mazeSceneData.Result = Success;
        FindObjectOfType<SceneSwitcher>().ShowLastSavedScene(_mazeSceneData);
    }

}

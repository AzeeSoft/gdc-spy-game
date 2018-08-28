using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialLevelManager : LevelManager {

	// Use this for initialization
	new void Start ()
	{
	    base.Start();

	    TutorialManager.Instance.ShowTutorial("Initializing");
	}
	
	// Update is called once per frame
    new void Update () {
        base.Update();

	    if (Input.GetButtonDown("Submit"))
	    {
            TutorialManager.Instance.BroadcastTutorialAction("ok");
	    }
	}

    public void OnSceneResultReceived(object data)
    {
        if (data is MazeSceneController.MazeSceneData)
        {
            MazeSceneController.MazeSceneData mazeSceneData = (MazeSceneController.MazeSceneData) data;
            mazeSceneData.Terminal.OnHackAttemptResultReceived(mazeSceneData);
        }
    }
}

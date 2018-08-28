using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialLevelManager : LevelManager {

	// Use this for initialization
	void Start () {
		TutorialManager.Instance.ShowTutorial("Initializing");
	}
	
	// Update is called once per frame
	void Update () {
	    if (Input.GetButtonDown("Submit"))
	    {
            TutorialManager.Instance.BroadcastTutorialAction("ok");
	    }
	}
}

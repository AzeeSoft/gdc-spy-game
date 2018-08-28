using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class BroadcastOnTrigger : MonoBehaviour
{
    public string OtherTag = "Player";
    public string BroadcastAction = "Triggered";

    public UnityEvent OnBroadcast;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void OnTriggerEnter(Collider collider)
    {
        if (collider.CompareTag(OtherTag))
        {
            TutorialManager.Instance.BroadcastTutorialAction(BroadcastAction);
            if (OnBroadcast != null)
            {
                OnBroadcast.Invoke();
            }
        }
    }
}

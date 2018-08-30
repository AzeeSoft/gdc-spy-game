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

    void OnTriggerStay(Collider collider)
    {
        if (collider.CompareTag(OtherTag))
        {
            bool broadcastReceived = TutorialManager.Instance.BroadcastTutorialActionWithResult(BroadcastAction);
            if (broadcastReceived && OnBroadcast != null)
            {
                OnBroadcast.Invoke();
            }
        }
    }
}

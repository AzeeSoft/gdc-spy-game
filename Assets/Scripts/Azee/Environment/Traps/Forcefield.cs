using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Characters.FirstPerson;

public class Forcefield : MonoBehaviour
{

    public float SlowDownFactor = 0.5f;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void OnTriggerEnter(Collider collider)
    {
        if (collider.CompareTag("Player"))
        {
            FirstPersonController fpsController = collider.gameObject.GetComponent<FirstPersonController>();

            fpsController.ModifySpeed(SlowDownFactor);
        }
    }

    void OnTriggerExit(Collider collider)
    {
        if (collider.CompareTag("Player"))
        {
            FirstPersonController fpsController = collider.gameObject.GetComponent<FirstPersonController>();

            fpsController.ModifySpeed(1/SlowDownFactor);
        }
    }
}

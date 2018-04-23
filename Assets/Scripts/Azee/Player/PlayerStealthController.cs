using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Azee.Interfaces;
using UnityEngine;
using UnityStandardAssets.Characters.FirstPerson;

public class PlayerStealthController : MonoBehaviour, AudibleObject
{
    public float maxNoiseRadius;
    public GameObject noiseRadiusGameObject;

    CharacterController characterController;
    FirstPersonController firstPersonController;

	// Use this for initialization
	void Start ()
	{
	    characterController = GetComponent<CharacterController>();
	    firstPersonController = GetComponent<FirstPersonController>();
	}
	
	// Update is called once per frame
	void Update ()
	{
	    float audibleRadius = 0;

	    if (characterController.velocity.sqrMagnitude > 0)
	    {
	        audibleRadius = maxNoiseRadius * firstPersonController.getCurrentFootstepVolume();
	    }

        noiseRadiusGameObject.transform.localScale = Vector3.Lerp(noiseRadiusGameObject.transform.localScale, Vector3.one * audibleRadius, Time.deltaTime * 1.5f); 
	}

    public Vector3? LocateFromNoise(GameObject target)
    {
        float curNoiseRadius = noiseRadiusGameObject.GetComponent<SphereCollider>().bounds.extents.x;

        if (Vector3.Distance(transform.position, target.transform.position) <= curNoiseRadius)
        {
            return transform.position;
        }

        return null;
    }
}

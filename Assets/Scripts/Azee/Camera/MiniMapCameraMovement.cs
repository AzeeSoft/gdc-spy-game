using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniMapCameraMovement : MonoBehaviour
{

    GameObject playerGameObject;

	// Use this for initialization
	void Start () {
		playerGameObject = GameObject.FindGameObjectWithTag("Player");
	}
	
	// Update is called once per frame
	void LateUpdate () {
		focusOnPlayer();
	}

    void focusOnPlayer()
    {
        transform.position = new Vector3(playerGameObject.transform.position.x, transform.position.y, playerGameObject.transform.position.z);

        Vector3 moddedForward = playerGameObject.transform.forward;
        moddedForward.y = 0;

        float playerForwardAngle = Vector3.SignedAngle(Vector3.forward, moddedForward, Vector3.up);

        Vector3 newEulerAngle = new Vector3(transform.eulerAngles.x, playerForwardAngle, transform.eulerAngles.z);
        transform.eulerAngles = newEulerAngle;
    }
}

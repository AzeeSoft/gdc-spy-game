using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lean : MonoBehaviour {

    public Transform cameraPivot;

    public float speed = 100f;
    public float maxAngle = 20f;

    float currentAngle = 0f;

    private void Awake()
    {
        if(cameraPivot == null && transform.parent != null)
        {
            cameraPivot = transform.parent;
        }
    }


    void Start () {
		
	}
	
	void Update ()
    {
		// lean left
        if (Input.GetKey(KeyCode.Q))
        {
            currentAngle = Mathf.MoveTowardsAngle(currentAngle, maxAngle, speed * Time.deltaTime);
        }

        // lean right
        else if (Input.GetKey(KeyCode.E))
        {
            currentAngle = Mathf.MoveTowardsAngle(currentAngle, -maxAngle, speed * Time.deltaTime);
        }

        // reset lean

        else
        {
            currentAngle = Mathf.MoveTowardsAngle(currentAngle, 0f, speed * Time.deltaTime);
        }

        cameraPivot.transform.localRotation = Quaternion.AngleAxis(currentAngle, Vector3.forward);
	}
}

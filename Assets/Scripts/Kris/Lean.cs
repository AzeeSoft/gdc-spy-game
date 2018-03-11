using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lean : MonoBehaviour {

    public Transform cameraPivot;

    public float rotationSpeed = 100f;
    private float speed = 1f;
    public float maxAngle = 20f;
    public float maxMove = 20f;

    public Vector3? unleanedPos;

    float currentAngle = 0f;

    private void Awake()
    {
        if(cameraPivot == null && transform.parent != null)
        {
            cameraPivot = transform.parent;
        }
    }


    void Start ()
    {
  
	}
	
	void Update ()
    {
		// lean left
        if (Input.GetKey(KeyCode.Q))
        {
            /*
            if(unleanedPos==null)
                unleanedPos = cameraPivot.transform.position;

            Vector3 direction = (cameraPivot.transform.right * -1 * speed);
            float oldY = cameraPivot.transform.position.y;
            cameraPivot.transform.Translate(direction);

            
            Vector3 newPos = cameraPivot.transform.position;
            newPos.y = oldY;
            cameraPivot.transform.position = newPos;
           */
            currentAngle = Mathf.MoveTowardsAngle(currentAngle, maxAngle, rotationSpeed * Time.deltaTime);
        }
        // lean right
        else if (Input.GetKey(KeyCode.E))
        {
            /*
            if (unleanedPos == null)
                unleanedPos = cameraPivot.transform.position;

            Vector3 direction = (cameraPivot.transform.right * speed);
            float oldY = cameraPivot.transform.position.y;
            cameraPivot.transform.Translate(direction);


            Vector3 newPos = cameraPivot.transform.position;
            newPos.y = oldY;
            cameraPivot.transform.position = newPos;
            */
            currentAngle = Mathf.MoveTowardsAngle(currentAngle, -maxAngle, rotationSpeed * Time.deltaTime);
        }

        // reset lean

        else
        {
            /*
            if (unleanedPos != null)
            {
                cameraPivot.transform.position = (Vector3)unleanedPos;
                unleanedPos = null;
            }
           */
            currentAngle = Mathf.MoveTowardsAngle(currentAngle, 0f, rotationSpeed * Time.deltaTime);
        }

        cameraPivot.transform.localRotation = Quaternion.AngleAxis(currentAngle, Vector3.forward);
	}
}

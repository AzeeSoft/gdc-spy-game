using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Press : MonoBehaviour
{
    GameObject objToActivate;
    bool canActivate = false;

    void OnTriggerEnter(Collider other)
    {
        objToActivate = other.gameObject;
        canActivate = true;
    }

    void Update()
    {
        if(Input.GetKeyDown("f"))
        {
            canActivate = false;
        }
    }

}

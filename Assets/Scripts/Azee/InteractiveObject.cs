using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class InteractiveObject : MonoBehaviour
{
    [Serializable]
    public class Interaction
    {
        public string description = "";
        public float minRange = 100f;
        public UnityEvent onInteractionEvent;
    }

    [Tooltip("Can have upto 2 interactions right now in this order: Primary, Secondary")]
    [SerializeField] public List<Interaction> interactions;


    // Use this for initialization
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
    }
}
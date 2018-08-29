using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;

public class InteractiveObject : MonoBehaviour
{
    [Serializable]
    public class InteractionEvent : UnityEvent<ActionController>
    {

    }

    [Serializable]
    public class Interaction
    {
        public bool enabled = true;
        public bool showGizmo = true;
        public Color gizmoColor = Color.blue;
        public bool showPrefix = true;
        public string description = "";
        public float maxRange = 30f;

        public ActionController.SpecialActionController requiresSpecialActionController =
            ActionController.SpecialActionController.None; 
        public InteractionEvent onInteractionEvent;
    }

    private const int MaxInteractions = 2;

    [Tooltip("Can have upto 2 interactions right now in this order: Primary, Secondary")]
    [SerializeField] public Interaction[] interactions = new Interaction[2];

    void OnValidate()
    {
        if (interactions.Length > MaxInteractions)
        {
            Debug.LogWarning("You can only have at most "+ MaxInteractions + " interactions!");
            Array.Resize(ref interactions, MaxInteractions);
        }
    }

    void OnDrawGizmos()
    {
        foreach (Interaction interaction in interactions)
        {
            if (interaction.enabled && interaction.showGizmo)
            {
                DebugExtension.DrawCircle(transform.position, Vector3.up, interaction.gizmoColor, interaction.maxRange);
                DebugExtension.DrawCircle(transform.position, Vector3.right, interaction.gizmoColor, interaction.maxRange);
                DebugExtension.DrawCircle(transform.position, Vector3.forward, interaction.gizmoColor, interaction.maxRange);
            }
        }
    }

    // Use this for initialization
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
    }

    public Interaction GetInteraction(int index)
    {
        if (index >= interactions.Length)
        {
            return null;
        }

        return interactions[index];
    }

    public void ToggleInteraction(int index, bool enable)
    {
        if (index < interactions.Length)
        {
            interactions[index].enabled = enable;
        }
    }
}
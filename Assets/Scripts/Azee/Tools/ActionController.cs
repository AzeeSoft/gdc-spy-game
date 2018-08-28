using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Outline = cakeslice.Outline;

public class ActionController : MonoBehaviour
{
    public float InteractionRadius = 0.5f;
    public float HighlightRadius = 10f;

    private const int MaxInteractions = 2;

    private readonly string[] InteractionInputButtons = new string[MaxInteractions]
    {
        "Primary Interaction",
        "Secondary Interaction"
    };

    private readonly string[] InteractionDescriptionPrefixes = new string[MaxInteractions]
    {
        "Left Mouse: ",
        "Right Mouse: "
    };

    [SerializeField] private float _maxDistance = 100f;

    [SerializeField] private Text _interactionDescriptionText;

    private Camera _camera;

    private bool[] _interactionInputs = new bool[MaxInteractions];

    private Stack<Outline> _activeOutlines = new Stack<Outline>();

    // Use this for initialization
    void Start()
    {
        _camera = GetComponentInChildren<Camera>();

        if (!_interactionDescriptionText)
        {
            Debug.LogWarning("Interaction Description Text is not assigned!!!");
        }
    }

    void LateUpdate()
    {
        CheckHighlights();
        DetectInteractionInputs();
        CheckInteraction();
    }

    private void DetectInteractionInputs()
    {
        for (int i = 0; i < MaxInteractions; i++)
        {
            _interactionInputs[i] = false;
        }

        for (int i = 0; i < MaxInteractions; i++)
        {
            if (Input.GetButtonDown(InteractionInputButtons[i]))
            {
                _interactionInputs[i] = true;
                return; // Ensures that at most only one interaction input is ever true
            }
        }
    }

    private void CheckInteraction()
    {
        string actionDescription = "";

        int layerMask = -5; //All layers
        
        RaycastHit raycastHit;

        bool hitDetected = Physics.SphereCast(_camera.transform.position, InteractionRadius,
            _camera.transform.forward,
            out raycastHit,
            _maxDistance,
            layerMask, QueryTriggerInteraction.Ignore);

        if (hitDetected)
        {
            if (raycastHit.distance > 0)
            {
                InteractiveObject interactiveObject = raycastHit.transform.GetComponent<InteractiveObject>();
                if (interactiveObject != null && interactiveObject.enabled)
                {
                    int interactionCount = Mathf.Min(MaxInteractions, interactiveObject.interactions.Length);

                    for (int i = 0; i < interactionCount; i++)
                    {
                        InteractiveObject.Interaction interaction = interactiveObject.interactions[i];

                        if (interaction.enabled &&
                            Vector3.Distance(transform.position, interactiveObject.transform.position) <=
                            interaction.maxRange)
                        {
                            actionDescription += (interaction.showPrefix ? InteractionDescriptionPrefixes[i] : "") + interaction.description + "\n";

                            if (_interactionInputs[i])
                            {
                                interaction.onInteractionEvent.Invoke();
                            }
                        }
                    }
                }
            }
        }

        if (_interactionDescriptionText)
        {
            _interactionDescriptionText.text = actionDescription;
            _interactionDescriptionText.gameObject.SetActive(!actionDescription.Equals(""));
        }
    }

    void CheckHighlights()
    {
        foreach (Outline outline in _activeOutlines)
        {
            outline.enabled = false;
        }
        _activeOutlines.Clear();


        int layerMask = -5; //All layers

        RaycastHit[] raycastHits = Physics.SphereCastAll(_camera.transform.position, HighlightRadius,
            _camera.transform.forward,
            _maxDistance,
            layerMask, QueryTriggerInteraction.Ignore);

        foreach (RaycastHit raycastHit in raycastHits)
        {
            Outline[] outlines = raycastHit.collider.gameObject.GetComponentsInChildren<Outline>();

            if (outlines.Length > 0)
            {
//                Debug.Log("Pointing at: " + raycastHit.transform.gameObject);
                InteractiveObject interactiveObject = raycastHit.transform.GetComponent<InteractiveObject>();
                if (interactiveObject != null)
                {
                    int interactionCount = Mathf.Min(MaxInteractions, interactiveObject.interactions.Length);

                    bool highlightable = false;
                    for (int i = 0; i < interactionCount; i++)
                    {
                        InteractiveObject.Interaction interaction = interactiveObject.interactions[i];

                        if (interaction.enabled &&
                            Vector3.Distance(transform.position, interactiveObject.transform.position) <=
                            interaction.maxRange)
                        {
                            highlightable = true;
                            break;
                        }
                    }

                    if (highlightable)
                    {
                        foreach (Outline outline in outlines)
                        {
                            outline.enabled = true;
                            _activeOutlines.Push(outline);
                        }
                    }
                }
            }
        }
    }
}
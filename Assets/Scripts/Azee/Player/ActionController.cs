using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ActionController : MonoBehaviour
{
    private const int MaxInteractions = 2;
    private readonly string[] InteractionInputButtons = new string[MaxInteractions]
    {
        "Primary Interaction",
        "Secondary Interaction"
    };
    private readonly string[] InteractionDescriptionPrefixes = new string[MaxInteractions]
    {
        "Primary: ",
        "Secondary: "
    };

    [SerializeField] private float maxDistance = 100f;
    [SerializeField] private List<string> interactiveLayerNames = new List<string>();

    [SerializeField] private Text interactionDescriptionText;

    private Camera fpsCamera;
    private XRayVision xRayVision;

    private bool[] interactionInputs = new bool[MaxInteractions];

	// Use this for initialization
	void Start ()
	{
	    fpsCamera = GetComponentInChildren<Camera>();
	    xRayVision = fpsCamera.GetComponent<XRayVision>();

	    if (!interactionDescriptionText)
	    {
            Debug.LogWarning("Interaction Description Text is not assigned!!!");
        }
	}
	
	void LateUpdate ()
	{
        DetectInteractionInputs();
	    CheckInteraction();
        CheckVisionToggle();
	}

    private void DetectInteractionInputs()
    {
        for (int i = 0; i < MaxInteractions; i++)
        {
            interactionInputs[i] = false;
        }

        for (int i = 0; i < MaxInteractions; i++)
        {
            if (Input.GetButtonDown(InteractionInputButtons[i]))
            {
                interactionInputs[i] = true;
                return;     // Ensures that at most only one interaction input is ever true
            }
        }
    }

    private void CheckInteraction()
    {
        string actionDescription = "";

        RaycastHit raycastHit = new RaycastHit();
        if (Physics.Raycast(fpsCamera.transform.position, fpsCamera.transform.forward, out raycastHit, maxDistance))
        {
//            Debug.Log("Pointing at: " + raycastHit.transform.gameObject);

            InteractiveObject interactiveObject = raycastHit.transform.GetComponent<InteractiveObject>();
            if (interactiveObject != null)
            {
                int interactionCount = Mathf.Min(MaxInteractions, interactiveObject.interactions.Length);

                for (int i = 0; i < interactionCount; i++)
                {
                    InteractiveObject.Interaction interaction = interactiveObject.interactions[i];

                    if (interaction.enabled && Vector3.Distance(transform.position, interactiveObject.transform.position) <=
                        interaction.maxRange)
                    {
                        actionDescription += InteractionDescriptionPrefixes[i] + interaction.description + "\n";

                        if (interactionInputs[i])
                        {
                            interaction.onInteractionEvent.Invoke();
                        }
                    }
                }
            }
        }

        if (interactionDescriptionText)
        {
            interactionDescriptionText.text = actionDescription;
            interactionDescriptionText.gameObject.SetActive(!actionDescription.Equals(""));
        }
    }

    void CheckVisionToggle()
    {
        if (Input.GetButtonDown("Toggle Vision") && xRayVision)
        {
            xRayVision.enabled = !xRayVision.enabled;
        }
    }
}

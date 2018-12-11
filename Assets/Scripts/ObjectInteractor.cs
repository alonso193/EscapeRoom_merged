using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ObjectInteractor : MonoBehaviour {
    public float interactRange;
    public GameObject carryObject;
    public HashSet<GameObject> excludeHitObjects;
    private GameObject hitObject;

    public GameObject actionsUICanvas;
    public Vector2 actionsUIBasePos;
    public float actionsUIHeightDelta;

    private Dictionary<string, string> actions;
    private List<string> buttons;
    private StorageUI storageUI;
    private bool interaction;

	void Start ()
    {
        interaction = true;
        buttons = new List<string> {
            "Button_X",
            "Button_Circle",
            "Button_Square",
            "Button_Triangle"
        };
        excludeHitObjects = new HashSet<GameObject>();
        storageUI = GameObject.FindWithTag("Scripts").GetComponent<StorageUI>();
	}
	
	void FixedUpdate ()
    {
        bool validHitActions = false;

        // Hide actions UI
        HideActionsUI();

        // Do nothing if interaction is disabled
        if (!interaction) return;

        // Check if some object was hit
        if (hitObject != null && !excludeHitObjects.Contains(hitObject)) {
            // Check if hit object is interactive
            IInteractive hitInteract = hitObject.GetComponent<IInteractive>();

            if (hitInteract != null) {
                actions = hitInteract.GetHitActions(gameObject, carryObject);

                if (actions != null) {
                    validHitActions = true;

                    // Show possible actions in the UI
                    ShowActionsUI(hitInteract, actions);

                    // Execute player action on the hitInteract (if any)
                    HandleHitActions(hitInteract, actions);
                }
            }
        }
        // Return if object was hit and actions were detected
        if (validHitActions) return;

        // Check if carry object is interactive
        if (carryObject != null) {
            IInteractive carryInteract = carryObject.GetComponent<IInteractive>();
            if (carryInteract != null) {
                actions = carryInteract.GetCarryActions(gameObject);

                if (actions != null) {
                    // Show possible actions in the UI
                    ShowActionsUI(carryInteract, actions);

                    // Execute player action on the carryInteract (if any)
                    HandleCarryActions(carryInteract, actions);
                }
            }
        }
	}

    public void SetInteraction(bool state)
    {
        interaction = state;
    }
    
    public bool GetInteraction()
    {
        return interaction;
    }

    public void SetHitObject(GameObject hit)
    {
        hitObject = hit;
    }

    public void UnsetHitObject()
    {
        hitObject = null;
    }

    bool HandleHitActions(IInteractive interact, Dictionary<string, string> actions)
    {
        bool executed = false;
        string actionKey = FindActionKeyDown(actions);

        if (actionKey != null) {
            interact.ExecuteHitAction(actions[actionKey], gameObject, carryObject);
            executed = true;
        }

        return executed;
    }

    bool HandleCarryActions(IInteractive interact, Dictionary<string, string> actions)
    {
        bool executed = false;
        string actionKey = FindActionKeyDown(actions);

        if (actionKey != null) {
            interact.ExecuteCarryAction(actions[actionKey], gameObject);
            executed = true;
        }

        return executed;
    }

    string FindActionKeyDown(Dictionary<string, string> actions)
    {
        foreach (string button in buttons) {
            if (actions.ContainsKey(button) && 
                Input.GetButtonDown(button))
                return button;
        }
        return null;
    }

    void HideActionsUI()
    {
        RectTransform canvasTransform = actionsUICanvas.GetComponent<RectTransform>();

        // Hide object name
        RectTransform objNameTransform = (RectTransform)canvasTransform.Find("ObjectName");
        if (objNameTransform != null) {
           objNameTransform.gameObject.SetActive(false);
        }

        // Hide actions
        foreach (var button in buttons) {
            RectTransform actionTransform = (RectTransform)canvasTransform.Find(button);
            if (actionTransform != null) {
               actionTransform.gameObject.SetActive(false);
            }
        }
    }

    void ShowActionsUI(IInteractive objInteract, Dictionary<string, string> actions)
    {
        int i = 0;
        RectTransform canvasTransform = actionsUICanvas.GetComponent<RectTransform>();

        // Show object name
        RectTransform objNameTransform = (RectTransform)canvasTransform.Find("ObjectName");
        if (objNameTransform != null) {
            // Set object name text
            TextMeshProUGUI objNameText = 
                objNameTransform.GetComponent<TextMeshProUGUI>();
            objNameText.text = objInteract.GetInteractiveName();

            // Show object name in UI
            objNameTransform.gameObject.SetActive(true);
        }

        // Show actions
        foreach (var button in buttons) {
            RectTransform actionTransform = (RectTransform)canvasTransform.Find(button);
            if (actionTransform != null && actions.ContainsKey(button)) {
                // Set action text
                TextMeshProUGUI actionText = 
                    actionTransform.Find("ActionText").GetComponent<TextMeshProUGUI>();
                actionText.text = actions[button];

                // Set action position
                actionTransform.localPosition =
                    new Vector3(actionsUIBasePos.x,
                                actionsUIBasePos.y + (i++)*actionsUIHeightDelta, 
                                0);

                // Show action in UI
                actionTransform.gameObject.SetActive(true);
            }
        }
    }
}

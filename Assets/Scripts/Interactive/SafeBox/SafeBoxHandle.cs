using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SafeBoxHandle : MonoBehaviour, IInteractive {
    public string interactiveName;
    private Dictionary<string, string> hitActions;
    private GameObject safeManager;

	void Start () {
        // Setup hitActions
        hitActions = new Dictionary<string, string>
        {
            { "Button_X", "Open" },
        };
        // Reference to safe box manager
        safeManager = GameObject.FindWithTag("SafeBox");
	}

    public string GetInteractiveName()
    {
        return interactiveName;
    }

    public Dictionary<string, string> GetHitActions(GameObject interactor, GameObject other)
    {
        return hitActions;
    }

    public void ExecuteHitAction(string actionName, GameObject interactor, GameObject other)
    {
        switch(actionName) {
            case "Open":
                OpenAction(interactor);
                break;
            default:
                Debug.Log("Invalid HitAction");
                break;
        }
    }

    void OpenAction(GameObject interactor)
    {
        Debug.Log("Open Action");
    }

    public Dictionary<string, string> GetCarryActions(GameObject interactor)
    {
        return null;
    }

    public void ExecuteCarryAction(string actionName, GameObject interactor)
    {
    }
}

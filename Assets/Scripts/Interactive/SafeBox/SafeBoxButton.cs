using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SafeBoxButton : MonoBehaviour, IInteractive {
    public string interactiveName;
    public int buttonNumber;
    private Dictionary<string, string> hitActions;
    private SafeBoxManager safeManager;

	void Start () {
        // Setup hitActions
        hitActions = new Dictionary<string, string>
        {
            { "Button_X", "Press" },
        };
        // Reference to safe box manager
        safeManager = GameObject.FindWithTag("SafeBox").GetComponent<SafeBoxManager>();
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
            case "Press":
                PressAction(interactor);
                break;
            default:
                Debug.Log("Invalid HitAction");
                break;
        }
    }

    void PressAction(GameObject interactor)
    {
        interactor.GetComponent<ObjectInteractor>().UnsetHitObject();
        safeManager.AddNumber(buttonNumber);
    }

    public Dictionary<string, string> GetCarryActions(GameObject interactor)
    {
        return null;
    }

    public void ExecuteCarryAction(string actionName, GameObject interactor)
    {
    }
}

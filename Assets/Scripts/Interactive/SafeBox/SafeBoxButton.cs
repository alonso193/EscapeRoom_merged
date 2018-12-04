using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SafeBoxButton : MonoBehaviour {
    public string interactiveName;
    public int buttonNumber;
    private Dictionary<string, string> hitActions;

	void Start () {
        // Setup hitActions
        hitActions = new Dictionary<string, string>
        {
            { "Button_X", "Press" },
        };
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
        Debug.Log("Press Action");
    }

    public Dictionary<string, string> GetCarryActions(GameObject interactor)
    {
        return null;
    }

    public void ExecuteCarryAction(string actionName, GameObject interactor)
    {
    }
}

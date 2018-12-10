using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElectricalBoxInteractive : MonoBehaviour, IInteractive {
    public string interactiveName;
    private bool isOpen;
    private Animator anim;

	void Start () {
        isOpen = false;
        anim = GetComponent<Animator>();
	}

    public string GetInteractiveName()
    {
        return interactiveName;
    }

    public Dictionary<string, string> GetHitActions(GameObject interactor, GameObject other)
    {
        var hitActions = new Dictionary<string, string>();
        if (!isOpen) {
            hitActions["Button_X"] = "Open";
        }

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
        isOpen = true;
        anim.SetBool("ElectricalBoxOpen", true);
        GetComponent<BoxCollider>().enabled = false;
    }

    public Dictionary<string, string> GetCarryActions(GameObject interactor)
    {
        return null;
    }

    public void ExecuteCarryAction(string actionName, GameObject interactor)
    {
    }
}

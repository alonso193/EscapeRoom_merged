using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Switchable : MonoBehaviour, IInteractive {
    public string interactiveName;
    public Switchable enableSwitch;
    private bool isOn;
    private bool lastState;
    public GameObject[] onObjects;
    public GameObject[] offObjects;

	void Start () {
        isOn = false;
        lastState = false;
	}

    public string GetInteractiveName()
    {
        return interactiveName;
    }

    void Update()
    {
        bool onState = isOn && ((enableSwitch == null) ||
                                (enableSwitch != null && enableSwitch.GetOnState()));
            
        if (onState != lastState)
            Switch(onState);

        lastState = onState;
    }

    public Dictionary<string, string> GetHitActions(GameObject interactor, GameObject other)
    {
        var hitActions = new Dictionary<string, string>();

        if (!isOn) {
            hitActions["Button_X"] = "Turn On";
        } else {
            hitActions["Button_Circle"] = "Turn Off";
        }

        return hitActions;
    }

    public void ExecuteHitAction(string actionName, GameObject interactor, GameObject other)
    {
        switch(actionName) {
            case "Turn On":
            case "Turn Off":
                SwitchAction();
                break;
            default:
                Debug.Log("Invalid HitAction");
                break;
        }
    }

    public bool GetOnState()
    {
        return isOn;
    }

    void Switch(bool onState)
    {
        foreach (var offObject in offObjects)
            offObject.SetActive(!onState);

        foreach (var onObject in onObjects)
            onObject.SetActive(onState);
    }

    void SwitchAction()
    {
        isOn = !isOn;
    }

    public Dictionary<string, string> GetCarryActions(GameObject interactor)
    {
        return null;
    }

    public void ExecuteCarryAction(string actionName, GameObject interactor)
    {
    }
}

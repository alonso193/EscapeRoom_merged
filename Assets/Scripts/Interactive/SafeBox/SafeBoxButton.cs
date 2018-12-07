using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SafeBoxButton : MonoBehaviour, IInteractive {
    public string interactiveName;
    public int buttonNumber;
    private Dictionary<string, string> hitActions;
    private GameObject safeBox;
    private SafeBoxManager safeManager;
    private SafeBoxInteractive safeInteract;
    private AudioSource buttonSound;

	void Start () {
        // Setup hitActions
        hitActions = new Dictionary<string, string>
        {
            { "Button_X", "Press" },
        };
        // Reference to safe box
        safeBox = GameObject.FindWithTag("SafeBox");
        safeManager = safeBox.GetComponent<SafeBoxManager>();
        buttonSound = safeBox.GetComponents<AudioSource>()[0];
        safeInteract = safeBox.GetComponent<SafeBoxInteractive>();
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
        bool leave = false;
        interactor.GetComponent<ObjectInteractor>().UnsetHitObject();

        // Add number to safe box screen
        leave = safeManager.AddNumber(buttonNumber);
        // Play button sound
        buttonSound.Play();
        // Leave safebox interaction if required
        if (leave)
            safeInteract.LeaveAction();
    }

    public Dictionary<string, string> GetCarryActions(GameObject interactor)
    {
        return null;
    }

    public void ExecuteCarryAction(string actionName, GameObject interactor)
    {
    }
}

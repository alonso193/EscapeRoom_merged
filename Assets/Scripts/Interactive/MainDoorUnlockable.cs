using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainDoorUnlockable : MonoBehaviour, IInteractive {
    public string interactiveName;
    public GameObject unlockObject;
    private Dictionary<string, string> hitActions;
    private Animator mainDoorAnim;
    private bool lockedState;
    private bool openState;

	void Start () {
        mainDoorAnim = GetComponentInParent<Animator>();
        lockedState = true;
        openState = false;
	}

    public string GetInteractiveName()
    {
        return interactiveName;
    }

    public Dictionary<string, string> GetHitActions(GameObject interactor, GameObject other)
    {
        var hitActions = new Dictionary<string, string>();
        if (lockedState) {
            if (other == unlockObject) {
                hitActions.Add("Button_X", "Unlock");
            }
        } else {
            if (openState) {
                hitActions.Add("Button_Circle", "Close");
            } else {
                hitActions.Add("Button_X", "Open");
            }
        }

        return hitActions;
    }

    public void ExecuteHitAction(string actionName, GameObject interactor, GameObject other)
    {
        switch(actionName) {
            case "Unlock":
                UnlockAction(other);
                break;
            case "Open":
                OpenStateAction(true);
                break;
            case "Close":
                OpenStateAction(false);
                break;
            default:
                Debug.Log("Invalid HitAction");
                break;
        }
    }

    void UnlockAction(GameObject other)
    {
        // Unlock
        lockedState = false;

        // Play unlock sound
        AudioSource unlockSound = GetComponent<AudioSource>();
        if (unlockSound != null) unlockSound.Play();

        // Destroy unlock object
        Destroy(other);
    }

    void OpenStateAction(bool open)
    {
        openState = open;
        if (open) {
            mainDoorAnim.SetBool("mainDoorOpen", true);
            mainDoorAnim.SetBool("mainDoorClose", false);
        } else {
            mainDoorAnim.SetBool("mainDoorClose", true);
            mainDoorAnim.SetBool("mainDoorOpen", false);
        }
    }

    public Dictionary<string, string> GetCarryActions(GameObject interactor)
    {
        return null;
    }

    public void ExecuteCarryAction(string actionName, GameObject interactor)
    {
    }
}

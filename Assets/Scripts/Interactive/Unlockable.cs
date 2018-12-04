using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unlockable : MonoBehaviour, IInteractive {
    public string interactiveName;
    public GameObject unlockObject;
    public float animationTime;
    public bool animateRotation;
    public Vector3 openPosition, closedPosition;

    private bool lockedState;
    private bool openState;

    void Start()
    {
        lockedState = true;
        openState = false;
    }

    public string GetInteractiveName()
    {
        return interactiveName;
    }

    /**
     * ====================
     * H I T  A C T I O N S
     * ====================
     */
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
        GetComponent<AudioSource>().Play();

        // Destroy unlock object
        Destroy(other);
    }

    void OpenStateAction(bool open)
    {
        var iTweenArgs = iTween.Hash();
        var movementType = animateRotation ? "rotation" : "position";
        var movementPosition = open ? openPosition : closedPosition;

        iTweenArgs.Add("time", animationTime);
        iTweenArgs.Add("islocal", true);
        iTweenArgs.Add(movementType, movementPosition);

        if (animateRotation)
            iTween.RotateTo(gameObject, iTweenArgs);
        else
            iTween.MoveTo(gameObject, iTweenArgs);
        openState = open;
    }

    /**
     * ========================
     * C A R R Y  A C T I O N S
     * ========================
     */
    public Dictionary<string, string> GetCarryActions(GameObject interactor)
    {
        return null;
    }

    public void ExecuteCarryAction(string actionName, GameObject interactor)
    {
    }
}

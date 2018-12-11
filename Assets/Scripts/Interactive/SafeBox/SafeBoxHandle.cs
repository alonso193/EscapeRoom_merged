using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class SafeBoxHandle : MonoBehaviour, IInteractive {
    public string interactiveName;
    private GameObject safeBox;
    private GameObject door;
    private GameObject handle;
    private Dictionary<string, string> hitActions;
    private SafeBoxManager safeManager;
    private SafeBoxInteractive safeInteract;
    private Animator doorAnim;
    private Animator handleAnim, handleCanvasAnim;

	void Start () {
        // Reference to safe box components
        safeBox = GameObject.FindWithTag("SafeBox");
        door = safeBox.transform.Find("Door").gameObject;
        handle = door.transform.Find("Handle").gameObject;

        safeManager = safeBox.GetComponent<SafeBoxManager>();
        safeInteract = safeBox.GetComponent<SafeBoxInteractive>();

        doorAnim = door.GetComponent<Animator>();
        handleAnim = handle.GetComponent<Animator>();
        handleCanvasAnim = GetComponent<Animator>();
	}

    public string GetInteractiveName()
    {
        return interactiveName;
    }

    public Dictionary<string, string> GetHitActions(GameObject interactor, GameObject other)
    {
        var hitActions = new Dictionary<string, string>();

        if (!safeManager.LockState) {
            hitActions["Button_X"] = "Open";
        }

        return hitActions;
    }

    public void ExecuteHitAction(string actionName, GameObject interactor, GameObject other)
    {
        switch(actionName) {
            case "Open":
                StartCoroutine(OpenAction(interactor));
                break;
            default:
                Debug.Log("Invalid HitAction");
                break;
        }
    }

    IEnumerator OpenAction(GameObject interactor)
    {
        // Trigger handle open animation
        handleAnim.SetBool("safeBoxHandleOpen", true);
        handleCanvasAnim.SetBool("safeBoxHandleOpen", true);
        yield return new WaitForEndOfFrame();
        yield return new WaitForSeconds(handleAnim.GetCurrentAnimatorStateInfo(0).length+0.5f);

        // Leave safe box interaction
        safeInteract.LeaveAction();

        // Set safe box state to open
        safeManager.IsOpen = true;
        safeBox.GetComponent<BoxCollider>().enabled = false;

        // Open safe box door
        doorAnim.SetBool("safeBoxDoorOpen", true);
    }

    public Dictionary<string, string> GetCarryActions(GameObject interactor)
    {
        return null;
    }

    public void ExecuteCarryAction(string actionName, GameObject interactor)
    {
    }
}

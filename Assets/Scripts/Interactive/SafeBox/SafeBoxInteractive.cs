using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Characters.FirstPerson;

public class SafeBoxInteractive : MonoBehaviour {
    public string interactiveName;

    private Dictionary<string, string> hitActions;
    private GameObject readCanvas;

    private bool examineInteraction;

    private GameObject player;
    private float originalPlayerSpeed;

	void Start () {
        // Setup hitActions
        hitActions = new Dictionary<string, string>
        {
            { "Button_X", "Examine" },
        };
		
        // Get readCanvas
        Transform readCanvasTransform = Camera.main.transform.Find("ReadCanvas");
        if (readCanvasTransform != null) {
            readCanvas = readCanvasTransform.gameObject;
        }
	}

    public void Update()
    {
        // If leave button pressed, restore object to original state
		if (examineInteraction && Input.GetButtonDown("Button_Circle")) {
            Leave();
        }
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
            case "Examine":
                ExamineAction(interactor);
                break;
            default:
                Debug.Log("Invalid HitAction");
                break;
        }
    }

    void ExamineAction(GameObject interactor)
    {
        // Set reference to player
        player = interactor;

        // Disable First Person Controller movement
        originalPlayerSpeed = player.GetComponent<FirstPersonController>().GetWalkSpeed();
        player.GetComponent<FirstPersonController>().SetWalkSpeed(0f);

        // Unset hit object
        player.GetComponent<ObjectInteractor>().UnsetHitObject();

        // Ignore reticle raycast
        gameObject.layer = LayerMask.NameToLayer("Ignore Raycast");

        // Setup safe box door canvas

        // Set examine interaction flag
        examineInteraction = true;
    }

    void Leave()
    {

    }

    public Dictionary<string, string> GetCarryActions(GameObject interactor)
    {
        return null;
    }

    public void ExecuteCarryAction(string actionName, GameObject interactor)
    {
    }
}

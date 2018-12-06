using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Characters.FirstPerson;

public class SafeBoxInteractive : MonoBehaviour, IInteractive {
    public string interactiveName;
    public float doorCanvasDistance;

    private Dictionary<string, string> hitActions;
    private GameObject readCanvas;
    private GameObject doorCanvas;

    private bool examineInteraction;

    private GameObject player;
    private float originalPlayerSpeed;

    private SafeBoxManager safeManager;

	void Start () {
        // Setup hitActions
        hitActions = new Dictionary<string, string>
        {
            { "Button_X", "Examine" },
        };
		
        //// Get readCanvas
        //Transform readCanvasTransform = Camera.main.transform.Find("ReadCanvas");
        //if (readCanvasTransform != null) {
        //    readCanvas = readCanvasTransform.gameObject;
        //}
        safeManager = GetComponent<SafeBoxManager>();
	}

    public void Update()
    {
  //      // If leave button pressed, restore object to original state
		//if (examineInteraction && Input.GetButtonDown("Button_Circle")) {
  //          Leave();
  //      }
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

        // Set safe box as carry
        player.GetComponent<ObjectInteractor>().carryObject = gameObject;

        // Initialize safe manager
        safeManager.ExamineInitialize();

        // Show safe box door canvas
        Transform doorCanvasTransform = transform.Find("doorCanvas");
        if (doorCanvasTransform != null) {
            doorCanvas = doorCanvasTransform.gameObject;
            Transform camTransform = Camera.main.transform;

            // Dettach door canvas from the safe box
            doorCanvasTransform.SetParent(null);

            // Place door canvas in front of the player
            //Vector3 forward = Vector3.Normalize(Vector3.ProjectOnPlane(camTransform.forward, Vector3.up));
            Vector3 forward = camTransform.forward;
            doorCanvasTransform.position = player.transform.position + forward * doorCanvasDistance;
            doorCanvasTransform.rotation = Quaternion.LookRotation(forward);
            doorCanvas.SetActive(true);
        }

        // Set examine interaction flag
        examineInteraction = true;
    }

    public Dictionary<string, string> GetCarryActions(GameObject interactor)
    {
        var carryActions = new Dictionary<string, string>();
        if (examineInteraction) {
            carryActions["Button_Circle"] = "Leave";
        }

        return carryActions;
    }

    public void ExecuteCarryAction(string actionName, GameObject interactor)
    {
        switch(actionName) {
            case "Leave":
                LeaveAction();
                break;
            default:
                Debug.Log("Invalid CarryAction");
                break;
        }
    }

    public void LeaveAction()
    {
        // Set examine interaction flag
        examineInteraction = false;

        // Hide safe box door canvas
        doorCanvas.transform.SetParent(transform);
        doorCanvas.SetActive(false);

        // Unset carry object
        player.GetComponent<ObjectInteractor>().carryObject = null;

        // Enable reticle raycast
        gameObject.layer = LayerMask.NameToLayer("Default");

        // Enable First Person Controller movement
        player.GetComponent<FirstPersonController>().SetWalkSpeed(originalPlayerSpeed);
    }

}

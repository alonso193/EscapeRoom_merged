using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickupable : MonoBehaviour, IInteractive {
    public string interactiveName;
    public Vector3 CarryAngles;
    public Vector3 CarryPosition;
    public float time = 1.0f;

    Dictionary<string, string> hitActions, carryActions;
    Transform camTransform;

    private bool beingCarried;

    private StorageUI storageUI;

    public void Start()
    {
        // Setup hitActions
        hitActions = new Dictionary<string, string>
        {
            { "Button_X", "Pick Up" },
        };

        // Setup carryActions
        carryActions = new Dictionary<string, string>
        {
            { "Button_Circle", "Drop" },
        };

        // Setup Rigidbody
        GetComponent<Rigidbody>().useGravity = true;

        // Reference to storage
        storageUI = GameObject.FindWithTag("Scripts").GetComponent<StorageUI>();
    }

    public void FixedUpdate()
    {
        // If being carried, update object position to follow camera
        if (beingCarried) {
            transform.position = Vector3.Lerp(transform.position, 
                camTransform.position 
                + camTransform.up * CarryPosition.y
                + camTransform.forward * CarryPosition.z
                + camTransform.right * CarryPosition.x,
                time);
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
            case "Pick Up":
                PickUpAction(interactor, other);
                break;
            default:
                Debug.Log("Invalid HitAction");
                break;
        }
    }

    void PickUpAction(GameObject interactor, GameObject carry)
    {
        //// Return if no space left in storage and carry
        //if (!storageUI.GetEmptySlots() && (carry != null)) return;

        // Adjust object physics for pick up
        GetComponent<Rigidbody>().useGravity = false;
        GetComponent<Rigidbody>().isKinematic = true;

        // Set camera transform
        camTransform = Camera.main.transform;

        // Set orientation
        transform.SetParent(camTransform);
        transform.localEulerAngles = CarryAngles;

        // Set flag to carry
        beingCarried = true;

        // Unset hit object just in case the reticle didn't exit in time
        interactor.GetComponent<ObjectInteractor>().UnsetHitObject();

        gameObject.layer = LayerMask.NameToLayer("Ignore Raycast");
        if (carry == null) {
            // Set current object as carry reference
            interactor.GetComponent<ObjectInteractor>().carryObject = gameObject;
        } else if (storageUI.GetEmptySlots()) {
            // Store object in inventory
            storageUI.UpdateStorage(gameObject);
            gameObject.SetActive(false);
        } else {
            // If no space left, swap current with carry object
            carry.GetComponent<IInteractive>().ExecuteCarryAction("Drop", interactor);
            interactor.GetComponent<ObjectInteractor>().carryObject = gameObject;
        }
    }

    public Dictionary<string, string> GetCarryActions(GameObject interactor)
    {
        return carryActions;
    }

    public void ExecuteCarryAction(string actionName, GameObject interactor)
    {
        switch(actionName) {
            case "Drop":
                DropAction(interactor);
                break;
            default:
                Debug.Log("Invalid CarryAction");
                break;
        }
    }

    void DropAction(GameObject interactor)
    {
        // If storage UI is not active
        if (!storageUI.GetStorageState()) {
            // Remove current object from carry reference
            interactor.GetComponent<ObjectInteractor>().carryObject = null;
        }

        // Set flag to don't carry
        beingCarried = false;

        // Drop object
        GetComponent<Rigidbody>().isKinematic = false;
        GetComponent<Rigidbody>().useGravity = true;
        transform.SetParent(null);

        // Make object hitable again
        gameObject.layer = LayerMask.NameToLayer("Default");

        // Unset camera transform
        camTransform = null;
    }

}

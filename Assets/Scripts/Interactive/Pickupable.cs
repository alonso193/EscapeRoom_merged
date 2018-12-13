using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickupable : MonoBehaviour, IInteractive {
    public string interactiveName;
    public Vector3 carryPosition;
    public Vector3 carryAngles;
    public float lerpTime = 1.0f;
    public bool allowStore = true;

    private Dictionary<string, string> carryActions;
    private PickupableBase pickupBase;

    private Transform camTransform;
    private bool beingCarried;
    private StorageUI storageUI;

    public void Start()
    {
        // Setup carryActions
        carryActions = new Dictionary<string, string>
        {
            { "Button_Circle", "Drop" },
        };

        // Pickupable base
        pickupBase = new PickupableBase(gameObject,
                                        carryPosition,
                                        carryAngles,
                                        allowStore,
                                        lerpTime);
    }

    public void FixedUpdate()
    {
        pickupBase.FixedUpdate();
    }

    public string GetInteractiveName()
    {
        return interactiveName;
    }

    public Dictionary<string, string> GetHitActions(GameObject interactor, GameObject other)
    {
        var hitActions = new Dictionary<string, string>();

        hitActions["Button_X"] = "Pick Up";

        if (pickupBase.StoreActionValid()) {
            hitActions["Button_Square"] = "Store";
        }

        return hitActions;
    }

    public void ExecuteHitAction(string actionName, GameObject interactor, GameObject other)
    {
        switch(actionName) {
            case "Pick Up":
                PickUpAction(interactor, other);
                break;
            case "Store":
                StoreAction(interactor, other);
                break;
            default:
                Debug.Log("Invalid HitAction");
                break;
        }
    }

    void PickUpAction(GameObject interactor, GameObject carry)
    {
        // Pickup object
        pickupBase.PickUp(interactor);

        // Grab object
        pickupBase.Grab(interactor, carry);
    }

    void StoreAction(GameObject interactor, GameObject carry)
    {
        // Pickup object
        pickupBase.PickUp(interactor);

        // Store object
        pickupBase.Store();
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
        pickupBase.Drop(interactor);
    }
}


public class PickupableBase {
    private GameObject pickupObject;
    private Vector3 carryPosition;
    private Vector3 carryAngles;
    private float lerpTime;
    private bool allowStore;

    public bool beingCarried;
    private Transform camTransform;
    private StorageUI storageUI;
    private AudioSource storeSound;

    public PickupableBase(GameObject pickupObject,
                          Vector3 carryPosition,
                          Vector3 carryAngles,
                          bool allowStore,
                          float lerpTime)
    {
        this.pickupObject = pickupObject;
        this.carryPosition = carryPosition;
        this.carryAngles = carryAngles;
        this.allowStore = allowStore;
        this.lerpTime = lerpTime;
        
        // Setup Rigidbody
        pickupObject.GetComponent<Rigidbody>().useGravity = true;

        // Reference to storage
        storageUI = GameObject.FindWithTag("Scripts").GetComponent<StorageUI>();

        // Reference to store sound
        storeSound = GameObject.FindWithTag("Scripts").GetComponent<AudioSource>();
    }

    public void FixedUpdate() {
        // If being carried, update object position to follow camera
        if (beingCarried) {
            pickupObject.transform.position = Vector3.Lerp(
                pickupObject.transform.position, 
                camTransform.position 
                + camTransform.up * carryPosition.y
                + camTransform.forward * carryPosition.z
                + camTransform.right * carryPosition.x,
                lerpTime);

            pickupObject.transform.localEulerAngles = carryAngles;
        }
    }

    public void PickUp(GameObject interactor)
    {
        // Adjust object physics for pick up
        pickupObject.GetComponent<Rigidbody>().useGravity = false;
        pickupObject.GetComponent<Rigidbody>().isKinematic = true;

        // Set camera transform
        camTransform = Camera.main.transform;

        // Set orientation
        pickupObject.transform.SetParent(camTransform);
        pickupObject.transform.localEulerAngles = carryAngles;

        // Set flag to carry
        beingCarried = true;

        // Unset hit object just in case the reticle didn't exit in time
        interactor.GetComponent<ObjectInteractor>().UnsetHitObject();

        // Ignore reticle raycast
        pickupObject.layer = LayerMask.NameToLayer("Ignore Raycast");
    }

    public void Drop(GameObject interactor)
    {
        // If storage UI is not active
        if (!storageUI.GetStorageState()) {
            // Remove current object from carry reference
            interactor.GetComponent<ObjectInteractor>().carryObject = null;
        }

        // Set flag to don't carry
        beingCarried = false;

        // Drop object
        //pickupObject.GetComponent<Rigidbody>().isKinematic = false;
        pickupObject.GetComponent<Rigidbody>().useGravity = true;
        pickupObject.transform.SetParent(null);

        // Make object hitable again
        pickupObject.layer = LayerMask.NameToLayer("Default");

        // Unset camera transform
        camTransform = null;
    }

    public void Grab(GameObject interactor, GameObject carry)
    {
        if (carry != null) {
            // Drop carry object
            carry.GetComponent<IInteractive>().ExecuteCarryAction("Drop", interactor);
        }
        // Set current object as carry
        interactor.GetComponent<ObjectInteractor>().carryObject = pickupObject;
    }

    public bool StoreActionValid()
    {
        return allowStore && storageUI.GetEmptySlots();
    }

    public void Store()
    {
        // Play store sound
        storeSound.Play();
        
        // Store in inventory
        if (storageUI.GetEmptySlots()) {
            storageUI.UpdateStorage(pickupObject);
            pickupObject.SetActive(false);
        }
    }
}


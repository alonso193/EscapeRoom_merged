using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReadPickupable : MonoBehaviour, IInteractive {
    public string interactiveName;

    // Readable attributes
    public float rotateSpeed = 2.5f;
    public Vector3 readCanvasPosition;
    public Vector3 readCanvasScale;
    public Vector3 readCanvasAngles;
    private ReadableBase readBase;

    // Pickupable attributes
    public Vector3 carryPosition;
    public Vector3 carryAngles;
    public float lerpTime = 1.0f;
    public bool allowStore = true;
    private PickupableBase pickupBase;

    private Dictionary<string, string> carryActions;

	void Start () {
        // Setup carryActions
        carryActions = new Dictionary<string, string>
        {
            { "Button_X", "Read" },
            { "Button_Circle", "Drop" },
        };

        // Readable base
        readBase = new ReadableBase(gameObject,
                                    true,
                                    rotateSpeed,
                                    readCanvasPosition,
                                    readCanvasScale,
                                    readCanvasAngles);

        // Pickupable base
        pickupBase = new PickupableBase(gameObject,
                                        carryPosition,
                                        carryAngles,
                                        allowStore,
                                        lerpTime);
	}
	
	void Update() {
        readBase.Update();

        if (readBase.readInteraction && Input.GetButtonDown("Button_Circle")) {
            pickupBase.beingCarried = true;
        }
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
            case "Read":
                ReadAction(interactor);
                break;
            case "Drop":
                DropAction(interactor);
                break;
            default:
                Debug.Log("Invalid CarryAction");
                break;
        }
    }

    void ReadAction(GameObject interactor)
    {
        pickupBase.beingCarried = false;
        readBase.Read(interactor);
    }

    void DropAction(GameObject interactor)
    {
        pickupBase.Drop(interactor);
    }
}

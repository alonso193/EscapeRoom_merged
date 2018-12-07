using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Characters.FirstPerson;

public class Readable : MonoBehaviour, IInteractive {
    public string interactiveName;
    Dictionary<string, string> hitActions;

    public float rotateSpeed = 2.5f;
    public Vector3 readCanvasPosition;
    public Vector3 readCanvasScale;
    public Vector3 readCanvasAngles;

    private ReadableBase readBase;

    void Start() {
        // Setup hitActions
        hitActions = new Dictionary<string, string>
        {
            { "Button_X", "Read" },
        };

        // Readable base
        readBase = new ReadableBase(gameObject,
                                    false,
                                    rotateSpeed,
                                    readCanvasPosition,
                                    readCanvasScale,
                                    readCanvasAngles);
	}
	
	void Update() {
        readBase.Update();
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
            case "Read":
                ReadAction(interactor);
                break;
            default:
                Debug.Log("Invalid HitAction");
                break;
        }
    }

    void ReadAction(GameObject interactor)
    {
        readBase.Read(interactor);
    }

    public Dictionary<string, string> GetCarryActions(GameObject interactor)
    {
        return null;
    }

    public void ExecuteCarryAction(string actionName, GameObject interactor)
    {
    }
}

public class ReadableBase {
    public bool readInteraction;

    private bool isPickupable;
    private float rotateSpeed;
    private GameObject readObject;
    private GameObject readCanvas;

    private GameObject player;
    private float originalPlayerSpeed;

    private Vector3 readCanvasPosition;
    private Vector3 readCanvasScale;
    private Vector3 readCanvasAngles;

    private Transform originalParent;
    private Vector3 originalPosition;
    private Vector3 originalScale;
    private Vector3 originalAngles;

    public ReadableBase(GameObject readObject,
                        bool isPickupable,
                        float rotateSpeed,
                        Vector3 readCanvasPosition,
                        Vector3 readCanvasScale,
                        Vector3 readCanvasAngles)
    {
        this.readObject = readObject;
        this.isPickupable = isPickupable;
        this.rotateSpeed = rotateSpeed;
        this.readCanvasPosition = readCanvasPosition;
        this.readCanvasScale = readCanvasScale;
        this.readCanvasAngles = readCanvasAngles;

        // Get readCanvas
        Transform readCanvasTransform = Camera.main.transform.Find("ReadCanvas");
        if (readCanvasTransform != null) {
            readCanvas = readCanvasTransform.gameObject;
        }

        readInteraction = false;
    }

    public void Read(GameObject player)
    {
        // Set reference to player
        this.player = player;

        // Disable First Person Controller movement
        originalPlayerSpeed = player.GetComponent<FirstPersonController>().GetWalkSpeed();
        player.GetComponent<FirstPersonController>().SetWalkSpeed(0f);

        // Disable interaction while reading
        player.GetComponent<ObjectInteractor>().SetInteraction(false);

        // Unset hit object
        player.GetComponent<ObjectInteractor>().UnsetHitObject();

        // Ignore reticle raycast
        readObject.layer = LayerMask.NameToLayer("Ignore Raycast");

        // Store original transform
        originalParent = readObject.transform.parent;
        originalPosition = readObject.transform.localPosition;
        originalScale = readObject.transform.localScale;
        originalAngles = readObject.transform.localEulerAngles;

        // Set object transform
        readObject.transform.SetParent(readCanvas.transform);
        readObject.transform.localPosition = readCanvasPosition;
        readObject.transform.localScale = readCanvasScale;
        readObject.transform.localEulerAngles = readCanvasAngles;

        if (!isPickupable) {
            // Enable object kinematic
            readObject.GetComponent<Rigidbody>().useGravity = false;
        }
        readObject.GetComponent<Rigidbody>().isKinematic = true;

        // Enable ReadCanvas interaction
        Transform leaveButton = readCanvas.transform.Find("LeaveButton");
        if (leaveButton != null) {
            leaveButton.gameObject.SetActive(true);
        }
        readInteraction = true;
    }

    public void Leave() {
        // Disable ReadCanvas interaction
        readInteraction = false;
        Transform leaveButton = readCanvas.transform.Find("LeaveButton");
        if (leaveButton != null) {
            leaveButton.gameObject.SetActive(false);
        }

        // Restore original transform
        readObject.transform.SetParent(originalParent);
        readObject.transform.localPosition = originalPosition;
        readObject.transform.localScale = originalScale;
        readObject.transform.localEulerAngles = originalAngles;

        if (player != null) {
            // Enable object interaction
            player.GetComponent<ObjectInteractor>().SetInteraction(true);

            // Enable First Person Controller movement
            player.GetComponent<FirstPersonController>().SetWalkSpeed(originalPlayerSpeed);
        }

        readObject.GetComponent<Rigidbody>().isKinematic = false;
        if (!isPickupable) {
            // Disable object kinematic
            readObject.GetComponent<Rigidbody>().useGravity = true;

            // Enable reticle raycast
            readObject.layer = LayerMask.NameToLayer("Default");
        }
    }

    public void Update()
    {
        // If leave button pressed, restore object to original state
		if (readInteraction) {
            if (Input.GetButtonDown("Button_Circle")) {
                Leave();
            } else {
                if (Input.GetAxis("Horizontal") > 0) {
                    readObject.transform.localRotation *= Quaternion.AngleAxis(rotateSpeed, Vector3.forward);
                }
                if (Input.GetAxis("Horizontal") < 0) {
                    readObject.transform.localRotation *= Quaternion.AngleAxis(-rotateSpeed, Vector3.forward);
                }
            }
        }
    }
}
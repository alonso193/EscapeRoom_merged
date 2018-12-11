using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LanternInteractive : MonoBehaviour, IInteractive
{
    public string interactiveName;

    private GameObject lights;
    private GameObject maincamera;
    private GameObject mainDoor;
    private AudioSource closeDoorSound;
    private GameObject lantern;

    private StorageUI storageUI;
    private PickupableBase pickupBase;

    public Vector3 carryPosition;
    public Vector3 carryAngles;
    public float lerpTime = 1.0f;
    public bool allowStore = true;

    private Dictionary<string, string> carryActions;

    public void Start()
    {
        lights = GameObject.FindWithTag("Lighting");
        maincamera = GameObject.FindWithTag("MainCamera");
        mainDoor = GameObject.FindWithTag("MainDoor");
        lantern = GameObject.FindWithTag("Lantern");
        closeDoorSound = lantern.GetComponents<AudioSource>()[0];
        storageUI = GameObject.FindWithTag("Scripts").GetComponent<StorageUI>();

        // Pickupable base
        pickupBase = new PickupableBase(gameObject,
                                        carryPosition,
                                        carryAngles,
                                        allowStore,
                                        lerpTime);

    }

    public string GetInteractiveName()
    {
        return interactiveName;
    }

    public Dictionary<string, string> GetHitActions(GameObject interactor, GameObject other)
    {
        var hitActions = new Dictionary<string, string>();

        hitActions["Button_X"] = "Pick Up";

        return hitActions;
    }

    public void ExecuteHitAction(string actionName, GameObject interactor, GameObject other)
    {
        switch (actionName)
        {
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
        maincamera.GetComponent<Light>().intensity = 10;
        //storageUI.SetActiveLantern();
        //closeDoorSound.Play();

        for (int i = 0; i < 84; i++)
        {
            mainDoor.transform.Rotate(Vector3.up, 50f * Time.deltaTime);
        }

        for (int i = 0; i < 41; i++)
        {
            mainDoor.transform.Translate(Vector3.left * 10f * Time.deltaTime);
        }

        for (int i = 0; i < 19; i++)
        {
            mainDoor.transform.Translate(Vector3.forward * 10f * Time.deltaTime);
        }

        Destroy(gameObject);
    }


    public Dictionary<string, string> GetCarryActions(GameObject interactor)
    {
        return carryActions;
    }

    public void ExecuteCarryAction(string actionName, GameObject interactor)
    {
        switch (actionName)
        {
            default:
                Debug.Log("Invalid CarryAction");
                break;
        }
    }
}

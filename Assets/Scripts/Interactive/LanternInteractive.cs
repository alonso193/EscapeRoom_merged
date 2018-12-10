using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LanternInteractive : MonoBehaviour, IInteractive
{
    public string interactiveName;

    private GameObject lights, lightsT;
    private GameObject maincamera;
    private GameObject mainDoor;
    private AudioSource closeDoorSound;
    private GameObject lantern;

    private GameObject storage;
    private PickupableBase pickupBase;

    public Vector3 carryPosition;
    public Vector3 carryAngles;
    public float lerpTime = 1.0f;
    public bool allowStore = true;

    private Dictionary<string, string> carryActions;

    public void Start()
    {
        lights = GameObject.FindWithTag("Lighting");
        lightsT = GameObject.FindWithTag("LightingTerrain");
        maincamera = GameObject.FindWithTag("MainCamera");
        mainDoor = GameObject.FindWithTag("MainDoor");
        lantern = GameObject.FindWithTag("Lantern");
        closeDoorSound = lantern.GetComponents<AudioSource>()[0];
        storage = GameObject.FindWithTag("Scripts");

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
        
        //lights.SetActive(false);
        //lightsT.SetActive(false);
        maincamera.GetComponent<Light>().intensity = 10;
        //storage.GetComponent<StorageUI>().SetActiveLantern();
        //closeDoorSound.Play();

        for (int i = 0; i < 83; i++)
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

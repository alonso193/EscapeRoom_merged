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
    private Animator mainDoorAnim;
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
        maincamera = Camera.main.gameObject;
        mainDoor = GameObject.FindWithTag("MainDoor");
        mainDoorAnim = mainDoor.GetComponent<Animator>();
        closeDoorSound = mainDoor.GetComponent<AudioSource>();
        lantern = GameObject.FindWithTag("Lantern");

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
                StartCoroutine(PickUpAction(interactor, other));
                break;
            default:
                Debug.Log("Invalid HitAction");
                break;
        }
    }

    IEnumerator PickUpAction(GameObject interactor, GameObject carry)
    {
        StorageUI storage = interactor.transform.Find("Scripts").GetComponent<StorageUI>();

        maincamera.GetComponent<Light>().intensity = 7;
        storage.SetActiveLantern();
        foreach (var renderer in GetComponentsInChildren<MeshRenderer>()) {
            renderer.enabled = false;
        }

        mainDoorAnim.SetBool("mainDoorClose", true);
        yield return new WaitForEndOfFrame();
        yield return new WaitForSeconds(mainDoorAnim.GetCurrentAnimatorStateInfo(0).length);

        closeDoorSound.Play();
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

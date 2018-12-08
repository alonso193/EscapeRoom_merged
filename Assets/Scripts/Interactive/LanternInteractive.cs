using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LanternInteractive : MonoBehaviour
{
    public string interactiveName;

    private GameObject lights;
    private GameObject maincamera;
    private StorageUI storageUI;
    private Dictionary<string, string> carryActions;

    public void Start()
    {
        lights = GameObject.FindWithTag("Lighting");
        maincamera = GameObject.FindWithTag("MainCamera");

        carryActions = new Dictionary<string, string>
        {
            { "Button_X", "Pick Up" },
        };
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
                PickUpAction(interactor);
                break;
                Debug.Log("Invalid HitAction");
                break;
        }
    }

    void PickUpAction(GameObject interactor)
    {
        Destroy(interactor);
        lights.SetActive(false);
        maincamera.GetComponent<Light>().intensity = 10;
    }

}

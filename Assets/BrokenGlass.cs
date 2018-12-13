using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrokenGlass : MonoBehaviour, IInteractive {
    public string interactiveName;
    public GameObject unlockObject;

    private bool brokenState;
    private Dictionary<string, string> carryActions;

    void Start()
    {
        brokenState = false;
    }

    public string GetInteractiveName()
    {
        return interactiveName;
    }

    /**
     * ====================
     * H I T  A C T I O N S
     * ====================
     */
    public Dictionary<string, string> GetHitActions(GameObject interactor, GameObject other)
    {
        var hitActions = new Dictionary<string, string>();
        if (!   brokenState)
        {
            if (other == unlockObject)
            {
                hitActions["Button_X"] = "Break";
            }
        }
        return hitActions;
    }

    public void ExecuteHitAction(string actionName, GameObject interactor, GameObject other)
    {
        switch (actionName)
        {
            case "Break":
                StartCoroutine(BreakAction(interactor));
                break;
            default:
                Debug.Log("Invalid HitAction");
                break;
        }
    }


    IEnumerator BreakAction(GameObject interactor)
    {
        brokenState = true;
        GetComponent<AudioSource>().Play();
        yield return new WaitForSeconds(1);
        Destroy(gameObject);
    }
   
    /**
     * ========================
     * C A R R Y  A C T I O N S
     * ========================
     */
    public Dictionary<string, string> GetCarryActions(GameObject interactor)
    {
        return carryActions;
    }

    public void ExecuteCarryAction(string actionName, GameObject interactor)
    {
    }
}

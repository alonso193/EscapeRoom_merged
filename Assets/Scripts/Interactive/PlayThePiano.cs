using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayThePiano : MonoBehaviour, IInteractive {
    public string interactiveName;

    private bool playState;
    private Dictionary<string, string> carryActions;

    void Start()
    {
        playState = false;
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
        if (!GetComponent<AudioSource>().isPlaying)
        {
            hitActions["Button_X"] = "Play";
        }

        return hitActions;
    }

    public void ExecuteHitAction(string actionName, GameObject interactor, GameObject other)
    {
        switch (actionName)
        {
            case "Play":
                PlayAction();
                break;
            default:
                Debug.Log("Invalid HitAction");
                break;
        }
    }

    void PlayAction()
    {
        GetComponent<AudioSource>().Play();
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

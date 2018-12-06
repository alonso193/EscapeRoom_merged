using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SafeBoxManager : MonoBehaviour {
    public List<int> unlockValues;
    public List<int> inputValues;

    private SafeBoxInteractive safeInteract;
    private bool lockState;

	void Start () {
        // Set values list
        inputValues = new List<int>();

        safeInteract = GameObject.FindWithTag("SafeBox").GetComponent<SafeBoxInteractive>();
        lockState = true;
	}

    public void ExamineInitialize()
    {
        inputValues = new List<int>();
    }

    public void AddNumber(int num)
    {
        if (inputValues.Count < unlockValues.Count) {
            inputValues.Add(num);
            if (inputValues.SequenceEqual(unlockValues)) {
                lockState = false;
                Debug.Log("Unlock safe box!");
            } else {
                if (inputValues.Count == unlockValues.Count) {
                    safeInteract.LeaveAction();
                }
            }
        }
    }
}

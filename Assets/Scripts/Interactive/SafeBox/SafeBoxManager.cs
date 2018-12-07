using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class SafeBoxManager : MonoBehaviour {
    public List<int> unlockValues;
    public List<int> inputValues;

    public bool LockState { set; get; }
    public bool IsOpen { set; get; }
    public GameObject numberDisplay;
    private AudioSource successSound;
    private AudioSource failSound;

	void Start () {
        ExamineInitialize();
        LockState = true;
        IsOpen = false;

        failSound = GetComponents<AudioSource>()[1];
        successSound = GetComponents<AudioSource>()[2];
	}

    public void ExamineInitialize()
    {
        inputValues = new List<int>();
        numberDisplay.GetComponent<TextMeshPro>().text = "";
    }

    public bool AddNumber(int num)
    {
        bool leave = false;
        if (inputValues.Count < unlockValues.Count) {
            inputValues.Add(num);
            numberDisplay.GetComponent<TextMeshPro>().text += num.ToString();

            if (inputValues.SequenceEqual(unlockValues)) {
                // Success combination
                LockState = false;
                successSound.Play();
            } else if (inputValues.Count == unlockValues.Count) {
                // Fail combination
                leave = true;
                failSound.Play();
            }
        }
        return leave;
    }
}

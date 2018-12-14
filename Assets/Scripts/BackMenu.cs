using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackMenu : MonoBehaviour {

    public GameObject[] scenes;

	void Update ()
    {
        if (Input.GetButtonDown("Button_Options"))
        {
            scenes[0].SetActive(true);
            scenes[1].SetActive(false);
        }
    }
}

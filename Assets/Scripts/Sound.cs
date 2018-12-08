using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Sound : MonoBehaviour {

    public void Start()
    {
        Button b = GetComponent<Button>();
        AudioSource audio = GetComponent<AudioSource>();
    }
}

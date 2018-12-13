using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenCloseDoor : MonoBehaviour {
    Animator anim;
	// Use this for initialization
	void Start () {
        anim = GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnTriggerEnter(Collider other)
    {
        anim.enabled = true;
        anim.SetTrigger("OpenCloseDoor");
    }

    private void OnTriggerExit(Collider other)
    {
        anim.enabled = true;
        anim.SetTrigger("CloseDoor");
    }

    private void OnTriggerStay(Collider other)
    {
        anim.enabled = false;
    }
}

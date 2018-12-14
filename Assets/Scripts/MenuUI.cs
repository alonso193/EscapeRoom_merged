using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuUI : MonoBehaviour {

    public int currentButton = 0;
    public Button[] MainButtons;
    public Color[] colorButtons;

    private float dPadYPrev = 0;
    private bool dPadYReady = true;

    void Start () {
		
	}
	

	void Update ()
    {
        UpdateCurrenButton();
        UpdateColorButton();
        CheckSelectButton();
	}

    void CheckSelectButton()
    {
        if (Input.GetButtonDown("Button_X"))
        {
            MainButtons[currentButton].onClick.Invoke();
        }
    }

    void UpdateCurrenButton()
    {
        float dPadY = Input.GetAxisRaw("DPad_Y");

        if (dPadYPrev != 0 && dPadY == 0)
            dPadYReady = true;

        dPadYPrev = dPadY;

        // Move DPad up
        if ( (dPadYReady && dPadY < 0) || Input.GetAxisRaw("Mouse ScrollWheel") > 0 )
        {
            dPadYReady = false;
            if(currentButton != 0)
            {
                currentButton--;
            }
        }

        // Move DPad down
        if ( (dPadYReady && dPadY > 0) || Input.GetAxisRaw("Mouse ScrollWheel") < 0 )
        {
            dPadYReady = false;
            if (currentButton < MainButtons.Length - 1)
            {
                currentButton++;
            }
        }
    }

    void UpdateColorButton()
    {
        for(int i = 0; i < MainButtons.Length; i++)
        {
            if(i == currentButton)
            {
                MainButtons[currentButton].GetComponent<Image>().color = colorButtons[1];
            }
            else
            {
                MainButtons[i].GetComponent<Image>().color = colorButtons[0];
            }
        }
    }
}

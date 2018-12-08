using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BackToMenu : MonoBehaviour {

	void Update ()
    {
        if (Input.GetKeyDown(KeyCode.M))
        {
            LoadByIndex(0);
        }
    }

    public void LoadByIndex(int sceneIndex)
    {
        //Time.timeScale = 0;
        GameObject Test = GameObject.FindGameObjectWithTag("test");
        Test.SetActive(false);
        SceneManager.LoadScene(sceneIndex, LoadSceneMode.Additive);
        //GameObject Test = GameObject.FindGameObjectWithTag("test");
        //Test.SetActive(false);
    }

}

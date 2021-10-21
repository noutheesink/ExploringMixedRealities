using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ChooseMode : MonoBehaviour
{
	
    // Start is called before the first frame update
    void Start()
    {
	    
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void GoToSeekerScene()
    {
	    SceneManager.LoadScene("SeekerScene",LoadSceneMode.Single);
    }
    
    public void GoToHiderScene()
    {
	    SceneManager.LoadScene("HiderScene",LoadSceneMode.Single);
    }

    // private void OnGUI()
    // {
	   //  if (GUI.Button(new Rect(new Vector2(Screen.width/2, 100),new Vector2(100, 200)), "Seeker")) GoToSeekerScene();
	   //  if (GUI.Button(new Rect(new Vector2(Screen.width/2, 200),new Vector2(100, 200)), "Hider")) GoToHiderScene();
    // }
}

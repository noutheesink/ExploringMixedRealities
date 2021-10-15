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
    [SerializeField] private Button seekerButton;
    [SerializeField] private Button hiderButton;
    
    // Start is called before the first frame update
    void Start()
    {
	    
    }

    // Update is called once per frame
    void Update()
    {
	    seekerButton.onClick.AddListener(GoToSeekerScene);
	    hiderButton.onClick.AddListener(GoToHiderScene);
    }

    private void GoToSeekerScene()
    {
	    Destroy(this);
	    SceneManager.LoadScene("SeekerScene",LoadSceneMode.Single);
    }
    
    private void GoToHiderScene()
    {
	    SceneManager.LoadScene("HiderScene",LoadSceneMode.Single);
    }

    // private void OnGUI()
    // {
	   //  if (GUI.Button(new Rect(new Vector2(Screen.width/2, 100),new Vector2(100, 200)), "Seeker")) GoToSeekerScene();
	   //  if (GUI.Button(new Rect(new Vector2(Screen.width/2, 200),new Vector2(100, 200)), "Hider")) GoToHiderScene();
    // }
}

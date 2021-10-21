using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class JamButton : MonoBehaviour
{
    public float timeOutTime = 60;
    private float timeSinceLastPress = 60;

    private Button _button;
    
    // Start is called before the first frame update
    void Start()
    {
        _button = GetComponent<Button>();
    }

    // Update is called once per frame
    void Update()
    {
        _button.interactable = timeSinceLastPress > timeOutTime;
        timeSinceLastPress += Time.deltaTime;
        Debug.Log(timeSinceLastPress);
    }

    public void OnClick()
    {
        timeSinceLastPress = 0;
        ClientSend.Button("jamButton");
    }
}

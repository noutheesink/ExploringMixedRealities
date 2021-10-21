using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;

    public bool seeker;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Debug.Log("Instance already exists, destroying object!");
            Destroy(this);
        }
    }

    public void Start()
    {
        StartCoroutine(WaitForConnecting());
    }

    private IEnumerator WaitForConnecting()
    {
        yield return new WaitForSeconds(1);
        Client.instance.ConnectToServer();
    }
}

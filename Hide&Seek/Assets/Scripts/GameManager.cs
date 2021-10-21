using System.Collections;
using System.Collections.Generic;
using System.Device.Location;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public static Dictionary<int, GeoCoordinate> players = new Dictionary<int, GeoCoordinate>();

    public GameObject localPlayerPrefab;
    public GameObject playerPrefab;

    public GameObject radar;

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

    public void SpawnPlayer(int _id, GeoCoordinate _coordinate)
    {
        players.Add(_id, _coordinate);
    }

    public void HandleButton(string buttonFunction)
    {
        Debug.Log("well if this doesnt work than idk anymore   " + buttonFunction);
        switch (buttonFunction)
        {
            case "jamButton":
                HandleJamButton();
                break;
            default:
                break;
        }
    }

    private void HandleJamButton()
    {
        if (!UIManager.instance.seeker) return;
        StartCoroutine(radar.GetComponent<RadarScript>().JamRadar());
    }
}

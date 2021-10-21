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
}

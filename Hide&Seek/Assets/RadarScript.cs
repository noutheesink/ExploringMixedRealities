using System;
using System.Collections;
using System.Collections.Generic;
using System.Device.Location;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR;

public class RadarScript : MonoBehaviour
{
    public float sweepSpeed = 180;
    public RawImage enemyImage;
    
    private static GPS gps;

    public float testLong;
    public float testLat;

    private Transform sweepTransform;

    private GeoCoordinate myCoordinates;
    public double radarScale;
    public double maxDistance;
    public GameObject motherDot;

    public List<GeoCoordinate> coordinatesList = new List<GeoCoordinate>();
    private List<GameObject> radarDots = new List<GameObject>();

    public GameObject radarJamming;

    private void Awake()
    {
        sweepTransform = transform.Find("Sweep");
        
        //coordinatesList.Add(mcDonaldsCoord);
        XRSettings.LoadDeviceByName("");
        XRSettings.enabled = false;
    }

    // Start is called before the first frame update
    void Start()
    {
        gps = GPS.Instance;
    }

    // Update is called once per frame
    void Update()
    {
        coordinatesList.Clear();

        foreach (var player in GameManager.players)
        {
            if (player.Key != Client.instance.myId) coordinatesList.Add(player.Value);
        } 
        coordinatesList.Add(new GeoCoordinate(testLat,testLong));
        
        radarDots.ForEach(Destroy);
        foreach(GeoCoordinate coordinates in coordinatesList)
        {
            PlaceCoordinates(coordinates);
        }

        Sweep();
    }

    private void Sweep()
    {
        sweepTransform.eulerAngles -= new Vector3(0, 0, sweepSpeed * Time.deltaTime);
    }
    
    public Vector3 GetRadarPosition()
    {
        return transform.position;
    }

    private void PlaceCoordinates(GeoCoordinate coordinates)
    {
        double distance = gps.gpsCoordinate.GetDistanceTo(coordinates);
        
        if (distance > maxDistance)
            return;

        double myX = gps.gpsCoordinate.Latitude;
        double myY = gps.gpsCoordinate.Longitude;

        double otherX = coordinates.Latitude;
        double otherY = coordinates.Longitude;

        Vector3 deltaDir = new Vector3((float)(myX - otherX), (float)(myY - otherY), 0).normalized;
        deltaDir *= (float)(radarScale * distance / maxDistance);

        Vector3 radarOrigin = transform.position;
        
        
        var newDot = Instantiate(motherDot, transform);
        newDot.GetComponent<RectTransform>().transform.position = radarOrigin + deltaDir;
        
        newDot.SetActive(true);
        radarDots.Add(newDot);
    }

    public IEnumerator JamRadar()
    {
        float timeUntilJamStop = 20;

        while (timeUntilJamStop > 0)
        {
            radarJamming.SetActive(true);
            timeUntilJamStop -= Time.deltaTime;
            yield break;
        }

        radarJamming.SetActive(false);
    }
}



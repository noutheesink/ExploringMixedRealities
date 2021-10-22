using System;
using System.Collections;
using System.Collections.Generic;
using System.Device.Location;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR;
using Random = UnityEngine.Random;

public class RadarScript : MonoBehaviour
{
    public float sweepSpeed = 180;
    public RawImage enemyImage;
    
    private static GPS gps;

    public float testLong;
    public float testLat;

    public Transform sweepTransform;

    private GeoCoordinate myCoordinates;
    public double radarScale;
    public double maxDistance;
    public GameObject motherDot;
    public Transform radarBackground;
    public float jamRotationSpeed = 20;
    
    public List<GeoCoordinate> coordinatesList = new List<GeoCoordinate>();
    private List<GameObject> radarDots = new List<GameObject>();

    public GameObject radarJamming;
    public bool isJamming = false;

    private void Awake()
    {
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
        //rotate radar to north
        Input.location.Start();
        Input.compass.enabled = true;
        Quaternion magneticRotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(0, 0, -Input.compass.magneticHeading), Time.deltaTime * 2);
        transform.rotation = magneticRotation;

        //rotate all dots according to magnetic radar rotation
        foreach (GameObject dot in radarDots)
        {
            dot.transform.RotateAround(gameObject.transform.position, new Vector3(0, 0, 1), magneticRotation.eulerAngles.z);
        }
        
        coordinatesList.Clear();

        foreach (var player in GameManager.players)
        {
            if (player.Key != Client.instance.myId) coordinatesList.Add(player.Value);
        } 
        coordinatesList.Add(new GeoCoordinate(testLat,testLong));
        
        radarDots.ForEach(Destroy);
        radarDots.Clear();
        foreach(GeoCoordinate coordinates in coordinatesList)
        {
            PlaceCoordinates(coordinates);
        }
        
        //rotate all dots according to magnetic radar rotation
        foreach (GameObject dot in radarDots)
        {
            dot.transform.RotateAround(gameObject.transform.position, new Vector3(0, 0, 1), magneticRotation.eulerAngles.z);
        }
        //Sweep();
    }

    private void Sweep()
    {
        float currentSweepSpeed = sweepSpeed;
        if (isJamming) currentSweepSpeed *= 10;
        sweepTransform.eulerAngles -= new Vector3(0, 0, currentSweepSpeed * Time.deltaTime);
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
        
        
        var newDot = Instantiate(motherDot, radarBackground);

        if (isJamming)
        {
            float range = 600;
            newDot.GetComponent<RectTransform>().transform.position = radarOrigin + new Vector3(Random.Range(-range, range),Random.Range(-range, range),0);
        }
        else newDot.GetComponent<RectTransform>().transform.position = radarOrigin + deltaDir;
        
        
        
        newDot.SetActive(true);
        radarDots.Add(newDot);
    }

    public IEnumerator JamRadar()
    {
        float timeUntilJamStop = 20;

        while (timeUntilJamStop > 0)
        {
            isJamming = true;
            Debug.Log(timeUntilJamStop);
            radarJamming.SetActive(true);
            timeUntilJamStop -= Time.deltaTime;
            radarJamming.transform.GetChild(0).rotation *= Quaternion.Euler(0,0,jamRotationSpeed * Time.deltaTime);
            yield return null;
        }
        
        radarJamming.transform.GetChild(0).rotation = Quaternion.Euler(0,0,0);

        radarJamming.SetActive(false);
        isJamming = false;
    }
}



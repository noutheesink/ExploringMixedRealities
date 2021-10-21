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
    public RawImage enemyImage;
    
    private static GPS gps;

    public float testLong;
    public float testLat;

    public TextMeshProUGUI distanceText;

    private Transform sweepTransform;

    private GeoCoordinate mcDonaldsCoord;

    private GeoCoordinate myCoordinates;
    public double radarScale;
    public double maxDistance;
    public GameObject motherDot;
    
    public List<GeoCoordinate> coordinatesList = new List<GeoCoordinate>();
    private List<GameObject> radarDots = new List<GameObject>();

    private void Awake()
    {
        sweepTransform = transform.Find("Sweep");
        
        mcDonaldsCoord = new GeoCoordinate(testLat, testLong);
        
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

        distanceText.text = "Angle to north: " + (-Input.compass.magneticHeading).ToString();

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
    }

    
    public Vector3 GetRadarPosition()
    {
        return transform.position;
    }

    private void PlaceCoordinates(GeoCoordinate coordinates)
    {
        double distance = gps.gpsCoordinate.GetDistanceTo(coordinates);
        //Debug.Log("has distance: " + distance);
        if (distance > maxDistance)
            return;

        double myX = gps.gpsCoordinate.Latitude;
        double myY = gps.gpsCoordinate.Longitude;

        double otherX = coordinates.Latitude;
        double otherY = coordinates.Longitude;

        Vector3 deltaDir = new Vector3((float)(myX - otherX), (float)(myY - otherY), 0).normalized;
        deltaDir *= (float)(radarScale * distance / maxDistance);

        Vector3 radarOrigin = transform.position;
        
        //var newDot = Instantiate(motherDot, radarOrigin + deltaDir, Quaternion.identity);
        var newDot = Instantiate(motherDot, transform);
        newDot.GetComponent<RectTransform>().transform.position = radarOrigin + deltaDir;
        //newDot.transform.position = radarOrigin + deltaDir;
        newDot.SetActive(true);
        radarDots.Add(newDot);
    }
}



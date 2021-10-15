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

    public TextMeshProUGUI distanceText;

    private Transform sweepTransform;

    private GeoCoordinate mcDonaldsCoord;

    private GeoCoordinate myCoordinates;
    public double maxDistance;
    public GameObject motherDot;
    
    private List<GeoCoordinate> coordinatesList = new List<GeoCoordinate>();
    

    private void Awake()
    {
        sweepTransform = transform.Find("Sweep");
        
        mcDonaldsCoord = new GeoCoordinate(testLat, testLong);
        
        coordinatesList.Add(mcDonaldsCoord);
        XRSettings.LoadDeviceByName("");
        XRSettings.enabled = false;
    }

    // Start is called before the first frame update
    void Start()
    {
        gps = GPS.Instance;
        
        //for (int i = 0; i < coordinatesList.Count; i++)
        //{
        //    RawImage newImage = Instantiate(enemyImage, transform);
        //    newImage.gameObject.SetActive(true);
        //    coordinatesList[i] = (coordinatesList[i].Item1, newImage);
        //}
    }

    // Update is called once per frame
    void Update()
    {
        float distance = (float)gps.gpsCoordinate.GetDistanceTo(mcDonaldsCoord);

        distanceText.text = "Distance to McDonalds: " + distance;
        myCoordinates = gps.gpsCoordinate;
        foreach(GeoCoordinate coordinates in coordinatesList)
        {
            PlaceCoordinates(coordinates);
        }

        //ShowOnRadar();
        Sweep();
    }

    //private void ShowOnRadar()
    //{
    //    Vector2 gpsVector2 = new Vector2(gps.latitude, gps.longitude);
    //    foreach (var (coordinate,image) in coordinatesList)
    //    {
    //        float distance = (float)gps.gpsCoordinate.GetDistanceTo(coordinate);

    //        Vector2 direction = (new Vector2((float) coordinate.Latitude, (float) coordinate.Longitude) - gpsVector2)
    //            .normalized;

    //        Vector2 position = direction * distance;

    //        image.rectTransform.position = position;
    //    }
    //}

    private void Sweep()
    {
        sweepTransform.eulerAngles -= new Vector3(0, 0, sweepSpeed * Time.deltaTime);
    }
    
    public Vector3 getRadarPosition()
    {
        return gameObject.transform.position;
    }

    private void PlaceCoordinates(GeoCoordinate coordinates)
    {
        double distance = myCoordinates.GetDistanceTo(coordinates);
        if (distance > maxDistance)
            return;

        double myX = myCoordinates.Latitude;
        double myY = myCoordinates.Longitude;

        double otherX = coordinates.Latitude;
        double otherY = coordinates.Longitude;

        Vector3 deltaDir = new Vector3((float)(myX - otherX), (float)(myY - otherY), gameObject.transform.position.z - 0.5f).normalized;
        deltaDir *= (float)(222 * distance / maxDistance);

        Vector3 radarOrigin = gameObject.transform.position;
        Instantiate(motherDot, radarOrigin + deltaDir, new Quaternion());
    }
}


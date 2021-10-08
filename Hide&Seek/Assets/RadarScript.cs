using System.Collections;
using System.Collections.Generic;
using System.Device.Location;
using TMPro;
using UnityEngine;

public class RadarScript : MonoBehaviour
{
    private static GPS gps;

    public float testLong;
    public float testLat;

    public TextMeshProUGUI distanceText;

    // Start is called before the first frame update
    void Start()
    {
        gps = GPS.Instance;
    }

    // Update is called once per frame
    void Update()
    {
        var testCoord = new GeoCoordinate(testLat, testLong);

        float distance = (float)gps.gpsCoordinate.GetDistanceTo(testCoord);

        distanceText.text = "Distance to McDonalds: " + distance;
    }
}

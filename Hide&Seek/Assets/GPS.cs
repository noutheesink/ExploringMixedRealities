using System.Collections;
using System.Collections.Generic;
using System.Device.Location;
using TMPro;
using UnityEngine;
using UnityEngine.XR.ARFoundation;

//using UnityEngine.XR.ARFoundation;

public class GPS : MonoBehaviour
{
    public static GPS Instance;

    public GeoCoordinate gpsCoordinate;
    
    public float latitude;
    public float longitude;

    public TextMeshProUGUI logText;
    public TextMeshProUGUI longText;
    public TextMeshProUGUI latText;
    
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else if (Instance != this)
        {
            Debug.Log("Instance already exists, destroying object!");
            Destroy(this);
        }
    }
    
    
    // Start is called before the first frame update
    void Start()
    {
        DontDestroyOnLoad(gameObject);
        StartCoroutine(StartLocationService());
    }

    private IEnumerator StartLocationService()
    {
        longText.enabled = false;
        latText.enabled = false;
        if (!Input.location.isEnabledByUser)
        {
            logText.text = "User has not enabled GPS";
            longText.enabled = false;
            latText.enabled = false;
            yield break;
        }

        Input.location.Start();
        int maxWait = 20;
        while (Input.location.status == LocationServiceStatus.Initializing && maxWait > 0)
        {
            yield return new WaitForSeconds(1);
            maxWait--;
        }

        if (maxWait <= 0)
        {
            logText.text = "Timed out";
            yield break;
        }

        while (Input.location.status != LocationServiceStatus.Failed)
        {
            logText.text = "Coordinates:";
            longText.enabled = true;
            latText.enabled = true;
            latitude = Input.location.lastData.latitude;
            longitude = Input.location.lastData.longitude;
            yield return new WaitForSeconds(1);
        }

        logText.text = "Unable to determine device location";
        yield break;
    }

    // Update is called once per frame
    void Update()
    {
        longText.text = "Longitude: " + longitude.ToString();
        latText.text = "Latitude: " + latitude.ToString();

        gpsCoordinate = new GeoCoordinate(latitude, longitude);
        if (latitude != 0 && longitude != 0)
        {
            SendCoordinates();
        }
    }
    
    void SendCoordinates()
    {
        ClientSend.ClientCoordinates(gpsCoordinate);
    }

}

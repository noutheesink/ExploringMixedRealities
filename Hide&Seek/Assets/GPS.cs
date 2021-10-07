using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.XR.ARFoundation;

//using UnityEngine.XR.ARFoundation;

public class GPS : MonoBehaviour
{
    public static GPS Instance;
    
    public float latitude;
    public float longitude;

    public TextMeshProUGUI logText;
    public TextMeshProUGUI longText;
    public TextMeshProUGUI latText;
    
    // Start is called before the first frame update
    void Start()
    {
        Instance = this;
        DontDestroyOnLoad(gameObject);
        StartCoroutine(StartLocationService());
    }

    private IEnumerator StartLocationService()
    {
        if (!Input.location.isEnabledByUser)
        {
            logText.text = "User has not enabled GPS";
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
        longText.text = longitude.ToString();
        latText.text = latitude.ToString();
    }
    

}

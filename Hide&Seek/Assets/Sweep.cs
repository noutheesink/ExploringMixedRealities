using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = System.Random;

public class Sweep : MonoBehaviour
{
    public float sweepSpeed;

    public RadarScript RadarScript;
    
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log(RadarScript.isJamming);
        
        float currentSweepSpeed = sweepSpeed;
        if (RadarScript.isJamming) currentSweepSpeed *= 5;
        transform.eulerAngles -= new Vector3(0, 0, currentSweepSpeed * Time.deltaTime);
    }
}

using System;
using UnityEngine;
using System.Collections;


public class InitialCalibration : MonoBehaviour {

    double lastUpdated;
    float lastAngle;

    IEnumerator Start()
    {
        // First, check if user has location service enabled
        if (!Input.location.isEnabledByUser)
            yield break;

        // Start service before querying location
        Input.location.Start();
        // Wait until service initializes
        int maxWait = 20;
        while (Input.location.status == LocationServiceStatus.Initializing && maxWait > 0)
        {
            yield return new WaitForSeconds(1);
            maxWait--;
        }

        // Service didn't initialize in 20 seconds
        if (maxWait < 1)
        {
            print("Timed out");
            yield break;
        }

        // Connection has failed
        if (Input.location.status == LocationServiceStatus.Failed)
        {
            print("Unable to determine device location");
            yield break;
        }
        else
        {
            // Access granted and location value could be retrieved
            print("Location: " + Input.location.lastData.latitude + " " + Input.location.lastData.longitude + " " + Input.location.lastData.altitude + " " + Input.location.lastData.horizontalAccuracy + " " + Input.location.lastData.timestamp);
        }

        // Stop service if there is no need to query location updates continuously
        //Input.location.Stop();
        lastUpdated = Input.compass.timestamp;
        Input.compass.enabled = true;
        //Screen.orientation = ScreenOrientation.Portrait;
        
    }
    /*
    void OnGUI()
    {
        GUIStyle guiStyle = new GUIStyle();
        guiStyle.fontSize = 100;
        GUI.Label(new Rect(0, 0, 500, 500), "Value: " + lastAngle, guiStyle);
    }
    */
    bool started = false;
	void Update () {
        
        if (!started &&  Input.compass.trueHeading != 0) 
        {
            transform.localEulerAngles = new Vector3(0, Input.compass.trueHeading, 0);
            this.lastAngle = Input.compass.trueHeading;
            started = true;
        }
	}
}

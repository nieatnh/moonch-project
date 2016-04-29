using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
public class CelestialCameraHelper : MonoBehaviour
{
    RawImage moonTargetCardboard;
    List<ICelestialPosition> celestialManagers;
    IEnumerator Start() 
    {
        moonTargetCardboard = GameObject.Find("/MainCamera/Head/Main Camera/Canvas/MoonTarget").GetComponent<RawImage>();
        //Debug.Log(moonTargetCardboard);
        celestialManagers = new List<ICelestialPosition>();
        celestialManagers.Add(CelestialDispatcher.CreateInstance(CelestialDispatcher.ECelestials.Moon));

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
	}
	
	void Update () 
    {
        double latitude = Input.location.lastData.latitude;
        double longitude = Input.location.lastData.longitude;

        foreach (var iManager in this.celestialManagers)
        {
            double deltaTheta, deltaPhi;
            var position = iManager.CalculatePosition(DateTime.UtcNow, latitude, longitude);
            double distance = CelestialScale.MoonScale(0.04f).AproxDistance;
            Vector3 v = Spherical.SphericalToCartesian(Math.PI / 2 - position.altitude, position.azimuth, distance /* moonPosition.distance / 405629.76174948126*/);

            Vector3 cameraDirection = Camera.main.transform.rotation * Vector3.forward;

            CalculateHelper(cameraDirection, v, out deltaPhi, out deltaTheta);
            double angle = Math.Atan2(deltaTheta, -deltaPhi) + Math.PI;
            double carboardRadius = 25f;
            double module = Math.Sqrt(deltaTheta * deltaTheta + deltaPhi * deltaPhi);

            if (module < (4 * Math.PI) / 180)
            {
                moonTargetCardboard.rectTransform.localPosition = new Vector3(200f, 200f, 0f);
                Application.LoadLevel("GoToTheMoonScene");
            }
            else
            {
                moonTargetCardboard.rectTransform.localPosition =
                    new Vector3((float)(1.5f * carboardRadius * Math.Cos(angle)), (float)(carboardRadius * Math.Sin(angle)), 0f);
            }
        }
	}

    void CalculateHelper(Vector3 camera, Vector3 target, out double deltaPhi, out double deltaTheta)
    {
        Spherical sCamera = (Spherical)camera;
        Spherical sTarget = (Spherical)target;

        deltaPhi = sTarget.phi - sCamera.phi;
        deltaTheta = sTarget.theta - sCamera.theta;

        if (deltaPhi > Math.PI) deltaPhi = -2 * Math.PI + deltaPhi;
        else if (deltaPhi < -Math.PI) deltaPhi = 2 * Math.PI + deltaPhi;
    }
}

using System;
using UnityEngine;
using System.Collections;

public class SunPosition : ICelestialPosition {
    string status = "";
    public string Name { get { return "Sun"; } }
    public CelestialPositionResult CalculatePosition(System.DateTime date, double latitude, double longitude, double alture = 0)
    {
        var res = SunCalc.getPosition(date, latitude, longitude);
        CelestialPositionResult result = new CelestialPositionResult();
        result.altitude = res.altitude;
        result.azimuth = res.azimuth + Math.PI;
        result.parallactic = double.NaN;
        result.distance = CelestialScale.SunAproxDistance;
        result.radius = CelestialScale.SunRadiusKm;
        if (res.altitude < 0) 
            status = "Below the horizon";
        else
            status = "Over the horizon";
        return result;
    }

    public Texture2D Texture
    {
        get { throw new System.NotImplementedException(); }
    }


    public string Status
    {
        get 
        {
            return this.status;
        }
    }
}

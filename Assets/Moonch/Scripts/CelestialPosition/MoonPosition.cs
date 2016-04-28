using System;
using UnityEngine;
using System.Collections;

public class MoonPosition : ICelestialPosition {

    public string Name { get { return "Moon"; } }

    public string Status 
    { 
        get 
        {
            var illumination  = SunCalc.getMoonIllumination(DateTime.UtcNow);
            var phase = MoonPhase.GetMoonPhase((float)illumination.phase);
            return phase.Name;
        } 
    }
    public CelestialPositionResult CalculatePosition(System.DateTime date, double latitude, double longitude, double alture = 0)
    {
        var res = SunCalc.getMoonPosition(date, latitude, longitude);
        CelestialPositionResult result = new CelestialPositionResult();
        result.altitude = res.altitude;
        result.azimuth = res.azimuth + Math.PI;
        result.parallactic = res.parallacticAngle;
        result.distance = res.distance;
        result.radius = CelestialScale.MoonRadiusKm;
        return result;
    }

    public Texture2D Texture
    {
        get 
        {
            //var illumination = SunCalc.getMoonIllumination(DateTime.UtcNow);
            //var phase = MoonPhase.GetMoonPhase((float)illumination.phase);
            //return phase.ImagePath;
            return null;
        }
    }
}

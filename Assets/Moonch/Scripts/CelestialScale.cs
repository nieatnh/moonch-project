using UnityEngine;
using System.Collections;

public class CelestialScale {
    //https://www.nasa.gov/pdf/180561main_ETM.Distance.Moon.pdf
    public const double MoonRadiusKm = 3475 / 2;
    public const double MoonAverageDistance = 382500;

    //http://www.nasa.gov/mission_pages/voyager/multimedia/pia17046.html#.VxuVMTB974Z
    public const double SunRadiusKm = 695508;
    public const double SunAproxDistance = 150000000;

    public struct ScaledInfo
    {
        public double AproxDistance;
        public double Radius;
    }

    public static ScaledInfo MoonScale(double targetRadius) 
    {
        ScaledInfo result = new ScaledInfo();
        result.Radius = targetRadius;
        result.AproxDistance = MoonAverageDistance / MoonRadiusKm * targetRadius;
        return result;
    }

    public static ScaledInfo SunScale(double targetRadius)
    {
        ScaledInfo result = new ScaledInfo();
        result.Radius = targetRadius;
        result.AproxDistance = SunAproxDistance / SunRadiusKm * targetRadius;
        return result;
    }
}

using System;
using UnityEngine;
using System.Collections;

public struct CelestialPositionResult
{
    public double azimuth;
    public double altitude;
    public double parallactic;
    public double distance;
    public double radius;
}

public interface ICelestialPosition {
    CelestialPositionResult CalculatePosition(DateTime date, double latitude, double longitude, double alture = 0);
    Texture2D Texture { get; }
    string Name { get; }
    string Status { get; }
}

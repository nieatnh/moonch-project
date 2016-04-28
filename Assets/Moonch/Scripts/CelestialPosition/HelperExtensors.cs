using System;
using UnityEngine;
using System.Collections;

public static class HelperExtensors
{
    public static float RadToDeg(this float rad)
    {
        return rad * 180 / Mathf.PI;
    }

    public static float DegToRad(this float deg)
    {
        return deg * Mathf.PI / 180;
    }
    public static double RadToDeg(this double rad)
    {
        return rad * 180 / Math.PI;
    }

    public static double DegToRad(this double deg)
    {
        return deg * Math.PI / 180;
    }

    //Adds an vertical angle (theta) of the target vector
    public static Vector3 AddTheta(this Vector3 target, float theta)
    {
        Spherical spherical = (Spherical)target;
        spherical.theta += theta;
        return (Vector3)spherical;
    }
}

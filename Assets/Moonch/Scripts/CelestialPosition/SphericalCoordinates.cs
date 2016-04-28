using System;
using UnityEngine;
using System.Collections;

struct Spherical
{
    public float radius;
    public float phi;
    public float theta;
    public Spherical(float radius, float phi, float theta)
    {
        this.radius = radius;
        this.phi = phi;
        this.theta = theta;
    }

    public static Vector3 SphericalToCartesian(double theta /*θ*/, double phi /*φ*/, double radius)
    {
        Vector3 result = new Vector3();
        result.z = (float)(radius * Math.Sin(theta) * Math.Cos(phi));
        result.x = (float)(radius * Math.Sin(theta) * Math.Sin(phi));
        result.y = (float)(radius * Math.Cos(theta));
        return result;
    }

    public static Spherical CartesianToSpherical(Vector3 cartesian)
    {
        Spherical result = new Spherical();
        result.radius = Mathf.Sqrt(cartesian.x * cartesian.x + cartesian.y * cartesian.y + cartesian.z * cartesian.z);
        result.theta = Mathf.Acos(cartesian.y / result.radius);
        //result.phi = Mathf.Atan(cartesian.x / cartesian.z);
        result.phi = Mathf.Atan2(cartesian.x, cartesian.z);
        return result;
    }
    public static explicit operator Spherical(Vector3 vector) 
    {
        return Spherical.CartesianToSpherical(vector);
    }
    public static explicit operator Vector3(Spherical spherical)
    {
        return Spherical.SphericalToCartesian(spherical.theta, spherical.phi, spherical.radius);
    }
}

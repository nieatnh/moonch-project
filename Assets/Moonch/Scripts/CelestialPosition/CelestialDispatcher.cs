using UnityEngine;
using System.Collections;

public class CelestialDispatcher 
{
    public enum ECelestials { Sun, Moon}

    public static ICelestialPosition CreateInstance(ECelestials target)
    {
        switch (target)
        { 
            case ECelestials.Moon:
                return new MoonPosition();
            case ECelestials.Sun:
                return new SunPosition();
        }
        return null;
    }
}

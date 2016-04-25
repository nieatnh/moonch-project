using UnityEngine;
using System.Collections;


public class MoonPhase 
{
    //Based in https://github.com/mourner/suncalc documentation
    //Images from https://svs.gsfc.nasa.gov/cgi-bin/details.cgi?aid=4404
    public static MoonPhase[] Phases = new MoonPhase[] 
    { 
        new MoonPhase(0.000f, "New Moon", "00-new-moon") ,
        new MoonPhase(0.125f, "Waxing Crescent", "01-waxing-crescent") ,
        new MoonPhase(0.250f, "First Quarter", "02-first-quarter") ,
        new MoonPhase(0.375f, "Waxing Gibbous", "03-waxing-gibbous") ,
        new MoonPhase(0.500f, "Full Moon", "04-full-moon") ,
        new MoonPhase(0.625f, "Waning Gibbous", "05-waning-gibbous") ,
        new MoonPhase(0.750f, "Last Quarter", "06-last-quarter") ,
        new MoonPhase(0.875f, "Waning Crescent", "07-waning-crescent") ,

    };
    public float TargetValue;
    public string Name;
    public string ImagePath;

    public MoonPhase(float TargetValue, string Name, string ImagePath)
    {
        this.TargetValue = TargetValue;
        this.Name = Name;
        this.ImagePath = ImagePath;
    }

    public static MoonPhase GetMoonPhase(float illumination)
    {
        float minDistance = 0;
        int idx = 0;
        for (int i = 0; i < MoonPhase.Phases.Length; i++)
        { 
            float targetValue = MoonPhase.Phases[i].TargetValue;
            float diff = Mathf.Abs(targetValue-illumination);
            if (i == 0 || diff < minDistance)
            {
                idx = i;
                minDistance = diff;
            }
        }
        return MoonPhase.Phases[idx];
    }
}



/*
public class MoonPhase : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
*/
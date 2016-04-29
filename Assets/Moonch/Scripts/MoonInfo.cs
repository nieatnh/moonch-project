using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System;

public class MoonInfo : MonoBehaviour {

	LocationInfo location = new LocationInfo();
	private Text countDownText;

	private Text distanceText;
	private Text phaseText;

	//private SunCalc sunCalcX;

	IEnumerator Start()
	{
		//sunCalcX = new SunCalc ();
		distanceText = GameObject.Find("/Menu/Background/DistanceValue").GetComponent<Text>();
		phaseText = GameObject.Find("/Menu/Background/PhaseValue").GetComponent<Text>();

		//countDownText = GameObject.Find("/Canvas/CountDown").GetComponent<Text>();
		//Debug.Log (moonImage.transform);
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
	}
	
	// Update is called once per frame
	void Update () {

		double latitude = Input.location.lastData.latitude; //-17.3663289;
		double longitude = Input.location.lastData.longitude; //-66.1758675;

		var moonPosition = SunCalc.getMoonPosition(DateTime.UtcNow, latitude, longitude);
		var moonIllumination = SunCalc.getMoonIllumination(DateTime.UtcNow);
        var phase = MoonPhase.GetMoonPhase((float)moonIllumination.phase);

        phaseText.text = phase.Name;
        distanceText.text = moonPosition.distance + " Km.";
		
		//Texture2D texture = Resources.Load<Texture2D>(phase.ImagePath);
	

	}
}

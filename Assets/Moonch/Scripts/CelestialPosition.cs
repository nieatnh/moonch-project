using System;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class CelestialPosition : MonoBehaviour {

    LocationInfo location = new LocationInfo();
    double lastPhi;
    double lastTheta;
	GameObject moonImage;
	public int countDown = 10;
	private float delay = 5f;
	private float nextUsage;
	private Text countDownText;
    IEnumerator Start()
    {
		nextUsage = Time.time + delay;
		moonImage = GameObject.Find ("/MoonImage");
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

	private IEnumerator Coroutine() {
		countDown--;
		yield return new WaitForSeconds(5);
	}

    protected void Update()
    {
        double latitude = Input.location.lastData.latitude; //-17.3663289;
        double longitude = Input.location.lastData.longitude; //-66.1758675;

        var suncalc = new SunCalcX();
        var moonPosition = suncalc.getMoonPosition(DateTime.UtcNow, latitude, longitude);
        double distance = CelestialScale.MoonScale(0.1f).AproxDistance;
        double azimuth = moonPosition.azimuth + Mathf.PI;

        Vector3 v = new Vector3();

        v = SphericalToCartesian(Math.PI/2 - moonPosition.altitude, azimuth, distance /* moonPosition.distance / 405629.76174948126*/);

        Vector3 cameraDirection = Camera.main.transform.rotation * Vector3.forward;
        Vector3 celestialDirection = v;

        CalculateHelper(cameraDirection, celestialDirection, out lastPhi, out lastTheta);

		//double degLastPhi = lastPhi*180/Math.PI;

		double angle = Math.Atan2 (lastTheta, -lastPhi)+Math.PI;
		double radius = 0.2f;
		double module = Math.Sqrt (lastTheta*lastTheta + lastPhi*lastPhi);
		//StartCoroutine (Coroutine ());
		//if (Time.time > nextUsage) {
		//	nextUsage = Time.deltaTime + delay;
		//	countDown--;
		//	if (countDown >= 0) {
		//		countDownText.text = countDown.ToString();
		//	}
		//}
		//countDown--;

		if (module < (4 * Math.PI) / 180) {
			//Application.LoadLevel ("Test");
			moonImage.transform.position = new Vector3 (20f, 20f, 0f);
			//Application.LoadLevel ("Test");
			//countDown--;
			//countDownText.text = countDown.ToString();
		} else {
			//countDown = 10;
			moonImage.transform.position = new Vector3 (0.5f, 0.5f, 0f) + new Vector3((float)(radius*Math.Cos(angle)), (float)(radius*Math.Sin(angle)), 0f);
		}

		//countDown--;


		//moonImage.transform.position = new Vector3 (0.3f, 0.3f, 0f);
		/*if (Math.Abs (lastPhi) < (4*Math.PI)/180) {
			moonImage.transform.position = new Vector3 (0.5f, 0.5f, 0f);
		} else if(lastPhi < 0){
			moonImage.transform.position = new Vector3 (0.3f, 0.5f, 0f);
		} else if(lastPhi > 0){
			moonImage.transform.position = new Vector3 (0.7f, 0.5f, 0f);
		}
        /*
        float r = 3;
        v.y = 1/3.0f;
        v.x = Mathf.Sin((float)azimuth);
        v.z = Mathf.Cos((float)azimuth);
         * */
        transform.position = v;
    }

    void OnGUI()
    {
        GUIStyle guiStyle = new GUIStyle();
        guiStyle.fontSize = 100;
        GUI.Label(new Rect(0, 0, 500, 500), String.Format("Phi: {0}\nTheta: {1}", toDeg(lastPhi), toDeg(lastTheta)), guiStyle);
    }


    void CalculateHelper(Vector3 camera, Vector3 target, out double deltaPhi, out double deltaTheta)
    {
        Spherical sCamera = CartesianToSpherical(camera);
        Spherical sTarget = CartesianToSpherical(target);

        deltaPhi = sTarget.phi - sCamera.phi;
        deltaTheta = sTarget.theta - sCamera.theta;

        if (deltaPhi > Math.PI) deltaPhi = -2 * Math.PI + deltaPhi;
        else if (deltaPhi < -Math.PI) deltaPhi = 2 * Math.PI + deltaPhi;
    }

    double toDeg(double radian)
    {
        return radian * 180 / Math.PI;
    }

    Vector3 SphericalToCartesian(double theta /*θ*/, double phi /*φ*/, double radius) {
        Vector3 result = new Vector3();
        result.z = (float)(radius * Math.Sin(theta) * Math.Cos(phi));
        result.x = (float)(radius * Math.Sin(theta) * Math.Sin(phi));
        result.y = (float)(radius * Math.Cos(theta));
        return result;
    }

    struct Spherical
    {
        public float radius;
        public float phi;
        public float theta;
    }

    Spherical CartesianToSpherical(Vector3 cartesian) 
    {
        Spherical result = new Spherical();
        result.radius = Mathf.Sqrt(cartesian.x * cartesian.x + cartesian.y * cartesian.y + cartesian.z * cartesian.z);
        result.theta = Mathf.Acos(cartesian.y / result.radius);
        //result.phi = Mathf.Atan(cartesian.x / cartesian.z);
        result.phi = Mathf.Atan2(cartesian.x ,  cartesian.z);
        return result;
    }

}

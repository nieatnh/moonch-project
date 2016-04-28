using System;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class CelestialPosition : MonoBehaviour {

    public CelestialDispatcher.ECelestials TargetCelestial;
    ICelestialPosition celestialManager;

    LocationInfo location = new LocationInfo();
    double lastPhi;
    double lastTheta;
	GameObject moonImage;
	RawImage moonTargetCardboard;
	public int countDown = 10;
	private float delay = 5f;
	private float nextUsage;
	private Text countDownText;
	
	private Text phaseText, distanceText;
    private GameObject moon;
    private Dictionary<string, Texture2D> moonPhaseTextures;


    GameObject myText;
    TextMesh myTextMesh;

    IEnumerator Start()
    {
        celestialManager = CelestialDispatcher.CreateInstance(this.TargetCelestial);
        //GameObject.CreatePrimitive(PrimitiveType.

        myText = new GameObject("southText");
        myText.transform.position = new Vector3(0, 0, -10);
        myText.transform.rotation = Quaternion.EulerAngles(0, Mathf.PI, 0);
        myText.transform.localScale = new Vector3(0.25f, 0.25f, 0.25f);

        MeshRenderer meshRenderer = myText.AddComponent<MeshRenderer>();
        meshRenderer.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.On;
        meshRenderer.receiveShadows = true;        
        meshRenderer.useLightProbes = true;
        meshRenderer.reflectionProbeUsage = UnityEngine.Rendering.ReflectionProbeUsage.BlendProbes;
        meshRenderer.material = Resources.Load<Material>("Arial");

        myTextMesh = myText.AddComponent<TextMesh>();
        myTextMesh.text = "South";
        myTextMesh.offsetZ = 0;
        myTextMesh.characterSize = 1;
        myTextMesh.lineSpacing = 1;
        myTextMesh.anchor = TextAnchor.MiddleCenter;
        myTextMesh.alignment = TextAlignment.Center;
        myTextMesh.fontSize = 0;
        myTextMesh.fontStyle = FontStyle.Normal;
        myTextMesh.richText = true;
        myTextMesh.color = Color.white;
        myTextMesh.font = Resources.Load<Font>("Arial");

        Instantiate(myText);

        
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
    }

    protected void Update()
    {
        

        double latitude = Input.location.lastData.latitude;
        double longitude = Input.location.lastData.longitude;

        var position = this.celestialManager.CalculatePosition(DateTime.UtcNow, latitude, longitude);

		
		double distance = CelestialScale.MoonScale(0.04f).AproxDistance;

        Vector3 v = Spherical.SphericalToCartesian(Math.PI / 2 - position.altitude, position.azimuth, distance /* moonPosition.distance / 405629.76174948126*/);
        Spherical vSpherical = (Spherical)v;

        Vector3 titlePosition = v.AddTheta(-Mathf.PI/16);
        //titlePosition.y+=1.5f;


        myTextMesh.text = String.Format("{0}\n{1}\n{2} Km.", celestialManager.Name, celestialManager.Status, distance);
        myText.transform.position = titlePosition;
        myText.transform.rotation = Quaternion.EulerAngles(0, vSpherical.phi, 0);

        transform.rotation = Quaternion.EulerAngles(0, vSpherical.phi + Mathf.PI/2, 0);
        transform.position = v;
    }

}

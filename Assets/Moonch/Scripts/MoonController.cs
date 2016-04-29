using UnityEngine;
using System.Collections;

public class MoonController : MonoBehaviour {

	private float distance;
	// Use this for initialization
	void Start () {
		distance = (float)CelestialScale.MoonScale (2f).AproxDistance;

		GameObject moon = GameObject.Find ("/Moon");

		GameObject cardBoardObject = GameObject.Find ("/Moon/CardObject");
		//cardBoardObject.transform.position = moon.transform.position;
		GameObject cardBoardObject1 = GameObject.Find ("/Moon/CardObject1");
		GameObject cardBoardObject2 = GameObject.Find ("/Moon/CardObject2");
		GameObject cardBoardObject3 = GameObject.Find ("/Moon/CardObject3");
		GameObject cardBoardObject4 = GameObject.Find ("/Moon/CardObject4");
		GameObject cardBoardObject5 = GameObject.Find ("/Moon/CardObject5");
		GameObject cardBoardObject6 = GameObject.Find ("/Moon/CardObject6");
		GameObject cardBoardObject7 = GameObject.Find ("/Moon/CardObject7");
		GameObject cardBoardObject8 = GameObject.Find ("/Moon/CardObject8");
		GameObject cardBoardObject9 = GameObject.Find ("/Moon/CardObject9");
		GameObject cardBoardObject10 = GameObject.Find ("/Moon/CardObject10");

		ArrayList list = new ArrayList ();
		list.Add (cardBoardObject);
		list.Add (cardBoardObject1);
		list.Add (cardBoardObject2);
		list.Add (cardBoardObject3);
		list.Add (cardBoardObject4);
		list.Add (cardBoardObject5);
		list.Add (cardBoardObject6);
		list.Add (cardBoardObject7);
		list.Add (cardBoardObject8);
		list.Add (cardBoardObject9);
		list.Add (cardBoardObject10);


//		Debug.Log (distance);
		Vector3 moonVector = new Vector3(0f, 0f, -distance);
		transform.position = moonVector;
		Random random = new Random ();
		for (int i = 0; i<list.Count; i++) {
			//list [i].transform.position = new Vector3 ();
			Vector3 noise = new Vector3(Random.Range(-10f,10f), Random.Range(-10f,10f), Random.Range(-10f,10f));
			((GameObject)list[i]).transform.position = moonVector/(list.Count+1) * (i+1)+noise;
		}

	}

	void OnTriggerEnter(Collider other) {
		if (other.gameObject.tag == "Player") {
			Application.LoadLevel ("LandingMoonScene");
		}
	}
	
	// Update is called once per frame
	void Update () {

		//double grab = Mathf.Floor (Random.Range (0f,));
	
	}
}

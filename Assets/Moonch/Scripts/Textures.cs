using UnityEngine;
using System.Collections;

public class Textures : MonoBehaviour {

	public Texture2D myTexture;
	// Use this for initialization
	void Start () {
		gameObject.GetComponent<Renderer>().material.mainTexture = myTexture;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}

using UnityEngine;
using System.Collections;

public class MainMenuController : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void StartGame()
    {
		//Debug.Log ("Test");
        Application.LoadLevel("GoToTheMoonScene");
    }

    public void finishGame()
    {
		//Debug.Log ("finish");
        Application.Quit();
    }

}

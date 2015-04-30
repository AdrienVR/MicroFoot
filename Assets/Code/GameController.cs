using UnityEngine;
using System.Collections;

public class GameController : MonoBehaviour {

	// Use this for initialization
	void Awake () {
		
		if (AudioManager.Instance == null && ControllerInterface.Instance == null)
		{
			Application.LoadLevelAdditive("commonScene");
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}

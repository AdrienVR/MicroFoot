using UnityEngine;
using System.Collections;

public class Goal : MonoBehaviour {

	public ColorType attackerColor;

	int goalIn;

	// Use this for initialization
	void Start () {
	
	}

	void OnTriggerEnter(Collider other) {
		if (other.gameObject.name == "Ball")
		{
			goalIn++;
			Debug.Log ("But !");
			if (attackerColor == ColorType.Bleu)
			{
				Debug.Log(goalIn.ToString() + " points pour l'équipe bleue !");
			}
			else
			{
				Debug.Log(goalIn.ToString() + " points pour l'équipe rouge !");
			}
		}
	}

	// Update is called once per frame
	void Update () {
	
	}

	public enum ColorType
	{
		Bleu,
		Rouge
	}
}

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class IntroRoot : MonoBehaviour {

	public List<MenuText> Texts;
	
	// Use this for initialization
	void Awake () 
	{
		Application.LoadLevelAdditive("commonScene");
	}

	// Use this for initialization
	void Start () 
	{
	}
	
	// Update is called once per frame
	void Update () 
	{

		if (m_enabled = false)
			return;

		if (ControllerInterface.GetKeyDown("start"))
		{
			m_enabled = false;
			ChangeScene();
		}
	}

	private void ChangeScene()
	{
		foreach(MenuText text in Texts)
		{
			text.Hide();
		}
		Application.LoadLevel("battleScene");

	}


	
	private bool m_enabled = true;

}

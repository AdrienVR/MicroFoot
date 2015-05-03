using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;

public class ControllerManager : MonoBehaviour 
{

	public static ControllerInterface ControllerInterface;

	void Awake()
	{
		if (ControllerInterface.Instance == null)
		{
			ControllerInterface = new ControllerInterface();
		}
		DontDestroyOnLoad(transform.gameObject);
	}

	void Update()
	{
		ControllerInterface.Instance.UpdateInternal();
	}

}

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ControllerTesting : MonoBehaviour {

	public List<GUIText> labels;

	// Use this for initialization
	void Start () {
		StartCoroutine(UpdateLabels());
	}

	private List<string> controllerNames = new List<string>();

	private IEnumerator UpdateLabels()
	{
		List<string> labelTexts = new List<string>{"","","",""};
		string axisName;
		while(true)
		{
			for(int i = 1 ; i < 5 ; i++)
			{
				labelTexts[i - 1] = "";
				foreach(string rawAxisName in axisNamesList)
				{
					axisName = rawAxisName.Replace("0",i.ToString());
					labelTexts[i - 1] += axisName + " : " + Input.GetAxis(axisName) + "\n";
				}
			}

			for(int j = 0 ; j < Input.GetJoystickNames ().Length ; j++)
			{
				if (controllerNames.Count < Input.GetJoystickNames ().Length)
					controllerNames.Add(Input.GetJoystickNames ()[j]);
				labels[j].text = controllerNames[j] + 
					"\n\n" +
					labelTexts[j];
			}
			
			for(int k = Input.GetJoystickNames ().Length ; k < 4 ; k++)
			{
				string name = "none";
				if (k < controllerNames.Count)
					name = controllerNames[k];
				labels[k].text = name + 
					"\n\n" +
						labelTexts[k];
			}
			yield return new WaitForSeconds(0.1f);
		}
	}

	static List<string> axisNamesList = new List<string>
	{
		{"J0_X_Axis"},
		{"J0_Y_Axis"},
		{"J0_Axis_1"},
		{"J0_Axis_2"},
		{"J0_Axis_3"},
		{"J0_Axis_4"},
		{"J0_Axis_5"},
		{"J0_Axis_6"},
		
	};

}

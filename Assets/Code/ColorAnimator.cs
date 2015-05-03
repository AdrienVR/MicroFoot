using UnityEngine;
using System.Collections;

public class ColorAnimator : MonoBehaviour {

	public float Period = 4.5f;
	public float Blink = 0.75f;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		m_timer += Time.deltaTime;
		if (m_timer > Period)
		{
			m_timer = 0;
		}

		/*
		if ( (int)(m_timer / Blink ) % 2 == 1 )
		{
			SetColor(0,0,0,0);
			return;
		}
		*/


		float r = Triangle(-1.5f, 1.5f, 1, m_timer) + Triangle(3, 6, 1, m_timer);
		float g = Triangle(0, 3, 1, m_timer);
		float b = Triangle(1.5f, 4.5f, 1, m_timer);

		/*
		float a = 0;
		for (int i = 0 ; i < 2 * Period / Blink ; i++)
		{
			a += Triangle(i*Blink, i+1, 1f, m_timer);

		}
		a = a;
		*/

		SetColor(r,g,b,1);
	}

	public void Enable()
	{
		enabled = true;
	}
	
	public void Disable()
	{
		SetColor(1,1,1,1);
		enabled = false;
	}

	//    a b   b' c
	//      _____
	//    _/     \_
	//
	private float Triangle(float limitMin, float limitMax, float remainMiddleTime,float value)
	{
		value = Mathf.Clamp(value, limitMin, limitMax);
		if (value >= limitMin + remainMiddleTime && value <= limitMax - remainMiddleTime)
			return 1;
		else if (value < limitMin + remainMiddleTime)
		{
			return (value - limitMin) / remainMiddleTime;
		}
		else if (value > limitMax - remainMiddleTime)
		{
			return 1 - (value - (limitMax - remainMiddleTime)) / remainMiddleTime;
		}
		return 0;
	}

	void SetColor(float r, float g, float b, float a)
	{
		guiText.color = new Color(r,g,b,a);
	}

	private float m_timer;

}

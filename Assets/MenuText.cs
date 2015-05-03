using UnityEngine;
using System.Collections;

public class MenuText : VirtualSelector 
{

	public ColorAnimator colorAnimator;

	// Use this for initialization
	void Start () 
	{
		m_size = guiText.fontSize;
	}

	// Update is called once per frame
	void Update () 
	{
		if (m_screenHeight != Screen.height)
			UpdateHeight(Screen.height);
	}

	public override void Select()
	{
		colorAnimator.Enable();
	}
	
	public override void Deselect()
	{
		colorAnimator.Disable();
	}

	public void Hide()
	{
		m_enableTimer = true;
		enabled = false;
	}
	
	private void UpdateHeight(float height)
	{
		m_screenHeight = height;
		guiText.fontSize = (int) (m_size * height / 300);
	}

	private bool m_enableTimer = false;
	private float m_size;
	private float m_screenHeight = 300;
}

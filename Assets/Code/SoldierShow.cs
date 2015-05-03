using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SoldierShow : VirtualSelector 
{

	public float RotationSpeed = 70;

	public List<Material> materials;

	public SoldierShow alterSoldier;
	
	public Animation MoveAnimation;

	[HideInInspector]
	public ColorType colorType;

	// Use this for initialization
	void Start () 
	{
		m_angle = Vector3.zero;
		mesh = transform.Find("Cube").gameObject;
		ChangeColor(0);
		StartCoroutine(StopAnimation());
	}
	
	// Update is called once per frame
	void Update () 
	{
		m_angle.y += Time.deltaTime * RotationSpeed;
		transform.rotation = Quaternion.Euler(m_angle);
	}

	public override void Select()
	{
		StartCoroutine(LaunchAnimation());
	}
	
	public override void Deselect()
	{
		StartCoroutine(StopAnimation());
	}

	public override void UpAction()
	{
		ChangeColor(1);
	}
	
	public override void DownAction()
	{
		ChangeColor(-1);
	}
	
	private IEnumerator LaunchAnimation()
	{
		MoveAnimation["Armature|ArmatureAction"].time = 0;
		yield return new WaitForEndOfFrame();
		MoveAnimation.Play();
		
	}

	private IEnumerator StopAnimation()
	{
		MoveAnimation["Armature|ArmatureAction"].time = 0;
		yield return new WaitForEndOfFrame();
		MoveAnimation.Stop();
		yield return new WaitForEndOfFrame();
		MoveAnimation["Armature|ArmatureAction"].time = 0;
		yield return new WaitForEndOfFrame();
		MoveAnimation.Stop();
		
	}

	private void ChangeColor(int side)
	{
		int colorTypeInt = (int)colorType;
		colorTypeInt += side;

		if (side == 0)
			side = 1;

		if (((int)alterSoldier.colorType) == colorTypeInt)
			colorTypeInt += side;

		if (colorTypeInt > 3)
			colorTypeInt = 0;
		if (colorTypeInt < 0)
			colorTypeInt = 3;

		colorType = (ColorType)colorTypeInt;
		
		if (((int)alterSoldier.colorType) == colorTypeInt)
			colorTypeInt += side;
		
		if (colorTypeInt > 3)
			colorTypeInt = 0;
		if (colorTypeInt < 0)
			colorTypeInt = 3;
		
		colorType = (ColorType)colorTypeInt;

		switch (colorType) 
		{
			case ColorType.Bleu:
				mesh.renderer.material = materials[0];
				break;
			case ColorType.Rouge:
				mesh.renderer.material = materials[1];
				break;
			case ColorType.Vert:
				mesh.renderer.material = materials[2];
				break;
			case ColorType.Jaune:
				mesh.renderer.material = materials[3];
				break;
		}
	}
	
	private GameObject mesh;
	private Vector3 m_angle;

}

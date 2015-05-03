using UnityEngine;
using System.Collections;

public class Goal : MonoBehaviour {

	public static int ScoreMax = 5;

	public Light FlashLight;
	public Light PointLight;
	public Material goalMat;
	
	public ColorType AttackerColor
	{
		set{m_attackerColor = value;}
	}
	public ColorType DefenderColor
	{
		set{m_defenderColor = value;}
	}

	public string TeamName
	{
		get{return m_teamName;}
	}
	public int Score
	{
		get{return m_score;}
	}

	public void InitColor()
	{
		switch (m_attackerColor) 
		{
		case ColorType.Bleu:
			m_teamName = "Bleus";
			break;
		case ColorType.Rouge:
			m_teamName = "Rouges";
			break;
		case ColorType.Vert:
			m_teamName = "Verts";
			break;
		case ColorType.Jaune:
			m_teamName = "Jaunes";
			break;
		}
		
		switch (m_defenderColor) 
		{
		case ColorType.Bleu:
			teamColor = new Color(0, 0.3f, 1);
			break;
		case ColorType.Rouge:
			teamColor = new Color(1, 0.3f, 0.3f);
			break;
		case ColorType.Vert:
			teamColor = new Color(0, 1f, 0.3f);
			break;
		case ColorType.Jaune:
			teamColor = new Color(1, 1, 0.3f);
			break;
		}
		
		PointLight.color = teamColor;
		goalMat.color = teamColor;
	}

	void OnTriggerEnter(Collider other) 
	{
		if (other.gameObject.name == "Ball")
		{
			m_score++;
			DisableCollider();
			StartCoroutine(LaunchFlashGoal());
		}
	}
	
	public void EnableCollider()
	{
		collider.enabled = true;
	}
	
	public void ResetScore()
	{
		m_score = 0;
	}

	private void DisableCollider()
	{
		collider.enabled = false;
	}

	private IEnumerator LaunchFlashGoal()
	{
		FlashLight.color = Color.Lerp(Color.white, teamColor, 0.5f);
		yield return new WaitForSeconds(0.075f);
		FlashLight.color = Color.black;
		GameManager.Instance.UpdateGoal();
	}
	
	private int m_score;
	private ColorType m_attackerColor;
	private ColorType m_defenderColor;
	private Color teamColor;
	private string m_teamName;

}

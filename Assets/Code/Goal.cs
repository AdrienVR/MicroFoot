using UnityEngine;
using System.Collections;

public class Goal : MonoBehaviour {
	
	public ColorType AttackerColor
	{
		set{m_attackerColor = value;}
	}
	public ColorType DefenderColor
	{
		set{m_defenderColor = value;}
	}
	public Light FlashLight;
	public Light PointLight;
	public Material goalMat;
	public string TeamName
	{
		get{return m_teamName;}
	}
	public int Score
	{
		get{return m_score;}
	}
	
	private int m_score;
	private ColorType m_attackerColor;
	private ColorType m_defenderColor;
	private Color teamColor;
	private string m_teamName;

	// Use this for initialization
	void Start () 
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
			Debug.Log ("But !");
			Debug.Log(m_score.ToString() + " points pour l'équipe " + m_teamName + " !");
			StartCoroutine(LaunchFlashGoal());
		}
	}

	void OnTriggerExit(Collider other) 
	{
		if (other.gameObject.name == "Ball")
		{
			FlashLight.color = Color.black;
		}
	}

	IEnumerator LaunchFlashGoal()
	{
		FlashLight.color = Color.Lerp(Color.white, teamColor, 0.5f);
		yield return new WaitForSeconds(0.075f);
		FlashLight.color = Color.black;
		GameManager.Instance.UpdateGoal();
	}
}

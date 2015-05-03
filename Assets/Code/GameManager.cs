using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameManager : MonoBehaviour 
{
	public static GameManager Instance;
	public static int TotalPlayers = 2;
	
	public int timer;
	public List<GameObject> gameItems;
	public List<Transform> respawnPositions1;
	public List<Transform> respawnPositions2;
	public Transform ballRespawnPosition;
	public Goal Goal1;
	public Goal Goal2;
	public GUIText Text;
	public IngameMenu menu;

	[HideInInspector]
	public int Players = 2;
	
	[HideInInspector]
	public ColorType TeamColor1;
	[HideInInspector]
	public ColorType TeamColor2;


	// Use this for initialization
	void Awake () {

		Instance = this;

		if (AudioManager.Instance == null && ControllerInterface.Instance == null)
		{
			Application.LoadLevelAdditive("commonScene");
		}

	}

	public void ResetGame()
	{
		GameObject ball = null;
		foreach(GameObject item in gameItems)
		{
			if (item.name != "Ball")
			{
				Destroy(item);
			}
			else
			{
				ball = item;
			}
		}
		gameItems = new List<GameObject>{ball};
		
		TotalPlayers = Players;
		
		string path = "Prefab/Soldier";
		
		for(int i = 0 ; i < Players ; i++)
		{
			GameObject go = (GameObject) GameObject.Instantiate(Resources.Load(path), Vector3.zero, Quaternion.identity);
			gameItems.Add(go);
			PlayerController soldier = go.GetComponentInChildren<PlayerController>();
			if (i > Players / 2 - 1)
			{
				soldier.colorType = TeamColor2;
			}
			else
			{
				soldier.colorType = TeamColor1;
			}
			soldier.indexSoldier = i;
		}
		ResetItems();
		UpdateGoalColor();
	}

	private void UpdateGoalColor()
	{
		Goal1.AttackerColor = TeamColor1;
		Goal1.DefenderColor = TeamColor2;
		Goal2.AttackerColor = TeamColor2;
		Goal2.DefenderColor = TeamColor1;
		Goal1.InitColor();
		Goal2.InitColor();
	}

	private IEnumerator ShowStartText()
	{
		while (m_timer > 0)
		{
			Text.text = m_timer.ToString();
			Text.transform.Find ("Text").guiText.text = m_timer.ToString();
			yield return new WaitForSeconds(1);
			m_timer--;
		}
		Text.text = "";
		Text.transform.Find ("Text").guiText.text = "";

	}

	public void UpdateGoal()
	{
		StartCoroutine(ShowScore());
	}

	private IEnumerator ShowScore()
	{
		string text = Goal1.TeamName +  " : " +  Goal1.Score + " - " + Goal2.TeamName +  " : " + Goal2.Score;;
		Text.text = text;
		Text.transform.Find ("Text").guiText.text = text;
		yield return new WaitForSeconds(2);
		ResetItems();
		
		Goal1.EnableCollider();
		Goal2.EnableCollider();
		if (Goal1.Score >= Goal.ScoreMax || Goal2.Score >= Goal.ScoreMax)
		{
			menu.RestartGame();
		}
	}

	public void ResetItems()
	{
		m_timer = timer;
		ColorType? team1 = null;
		int indexRespawn1 = 0;
		int indexRespawn2 = 0;

		foreach(GameObject item in gameItems)
		{
			Transform respawnPosition = null;
			if (item.GetComponent<Ball>() != null)
			{
				respawnPosition = ballRespawnPosition;
				item.GetComponent<Ball>().timer = timer;
			}
			else if (item.GetComponentInChildren<PlayerController>() != null)
			{
				PlayerController player = item.GetComponentInChildren<PlayerController>();
				player.timer = timer;
				if (team1 == null)
				{
					team1 = player.colorType;
				}
				if (player.colorType == team1.Value)
				{
					respawnPosition = respawnPositions1[indexRespawn1++];
				}
				else
				{
					respawnPosition = respawnPositions2[indexRespawn2++];
				}
				player.ResetPosition();
			}

			item.transform.position = respawnPosition.position;
			item.transform.rotation = respawnPosition.rotation;

			Rigidbody rigBody = item.GetComponentInChildren<Rigidbody>();

			rigBody.velocity = Vector3.zero;
			rigBody.angularVelocity = Vector3.zero;
		}

		StartCoroutine(ShowStartText());
	}
	
	private int m_timer;
}


public enum ColorType
{
	Bleu = 0,
	Rouge = 1,
	Vert = 2,
	Jaune = 3
}

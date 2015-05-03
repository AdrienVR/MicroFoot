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
	public Goal goal1;
	public Goal goal2;
	public GUIText Text;

	[HideInInspector]
	public int Players = 2;
	
	[HideInInspector]
	public ColorType TeamColor1;
	[HideInInspector]
	public ColorType TeamColor2;


	// Use this for initialization
	void Awake () {

		Instance = this;

		TotalPlayers = Players;

		if (AudioManager.Instance == null && ControllerInterface.Instance == null)
		{
			Application.LoadLevelAdditive("commonScene");
		}

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
		goal1.AttackerColor = TeamColor1;
		goal1.DefenderColor = TeamColor2;
		goal2.AttackerColor = TeamColor2;
		goal2.DefenderColor = TeamColor1;
	}

	private IEnumerator ShowStartText()
	{
		while (m_timer > 0)
		{
			Text.text = m_timer.ToString();
			yield return new WaitForSeconds(1);
			m_timer--;
		}
		Text.text = "";

	}

	public void UpdateGoal()
	{
		StartCoroutine(ShowScore());
	}

	private IEnumerator ShowScore()
	{
		Text.text = goal1.TeamName +  " : " +  goal1.Score + " - " + goal2.TeamName +  " : " + goal2.Score;
		yield return new WaitForSeconds(1);
		ResetItems();
	}

	public void ResetItems()
	{
		m_timer = timer;
		ColorType? team1 = null;
		int indexRespawn1 = 0;
		int indexRespawn2 = 0;

		foreach(GameObject item in gameItems)
		{
			Transform respawnPosition = ballRespawnPosition;
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

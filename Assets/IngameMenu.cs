using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class IngameMenu : MonoBehaviour {


	public float lockSelectionDelay = 0.75f;
	
	public List<VirtualSelector> selectables;
	public List<int> selectablesIndex;
	public GameObject GameManagerObject;
	public GameManager GameManager;
	public GUIText PlayersText;
	public SoldierShow Team1;
	public SoldierShow Team2;

	[HideInInspector]
	public int ScoreMax;

	// Use this for initialization
	void Start () 
	{
		m_maxSelectables = FindMaxInSelectablesIndex();
	}
	
	// Update is called once per frame
	void Update () 
	{
		if (lockSelectionTimer > 0)
		{
			lockSelectionTimer -= Time.deltaTime;
			return;
		}

		m_lastSelector = m_selector;
		UpdateSelector();
		if (m_lastSelector == m_selector)
			return;
		UpdateSelectables();

	}
	
	void OnRestart()
	{
		
	}

	void OnStart()
	{

	}

	void UpdateSelector()
	{
		//First pass
		if (m_selector == -1)
			m_selector = 0;

		if (ControllerInterface.GetKeyDown("right"))
		{
			m_selector++;
			lockSelectionTimer += lockSelectionDelay;
		}
		else if (ControllerInterface.GetKeyDown("left"))
		{
			m_selector--;
			lockSelectionTimer += lockSelectionDelay;
		}
		else if (ControllerInterface.GetKeyDown("down"))
		{
			foreach(VirtualSelector selector in m_currentSelectables)
			{
				selector.DownAction();
			}
			lockSelectionTimer += lockSelectionDelay;
			if (m_selector == 2)
			{
				m_playersCounter--;
				m_playersCounter = Mathf.Clamp(m_playersCounter, 2, 4);
				PlayersText.text = m_playersCounter.ToString();
			}
		}
		else if (ControllerInterface.GetKeyDown("up"))
		{
			foreach(VirtualSelector selector in m_currentSelectables)
			{
				selector.UpAction();
			}
			lockSelectionTimer += lockSelectionDelay;
			if (m_selector == 2)
			{
				m_playersCounter++;
				m_playersCounter = Mathf.Clamp(m_playersCounter, 2, 4);
				PlayersText.text = m_playersCounter.ToString();
			}
		}
		else if (ControllerInterface.GetKeyDown("validate"))
		{
			if (m_selector == 3)
			{
				GameManager.Players = m_playersCounter;
				GameManager.TeamColor1 = Team1.colorType;
				GameManager.TeamColor2 = Team2.colorType;
				GameManagerObject.SetActive(true);
				gameObject.SetActive(false);
			}
			lockSelectionTimer += lockSelectionDelay;
		}

		if (m_selector > m_maxSelectables)
		{
			m_selector = 0;
		}
		
		if (m_selector < 0)
		{
			m_selector = m_maxSelectables;
		}
	}

	int FindMaxInSelectablesIndex()
	{
		int max = 0;
		foreach(int number in selectablesIndex)
		{
			if (number > max)
				max = number;
		}
		return max;
	}

	void UpdateSelectables()
	{
		m_currentSelectables = new List<VirtualSelector>();

		for(int i = 0 ; i < selectablesIndex.Count ; i++)
		{
			if (selectablesIndex[i] == m_selector)
			{
				selectables[i].Select();
				m_currentSelectables.Add(selectables[i]);
			}
			if (selectablesIndex[i] == m_lastSelector)
				selectables[i].Deselect();
		}
	}

	private int m_playersCounter = 2;
	private int m_maxSelectables;

	private int m_lastSelector;
	
	private List<VirtualSelector> m_currentSelectables;
	
	private int m_selector = -1;
	private float lockSelectionTimer = 0;
}

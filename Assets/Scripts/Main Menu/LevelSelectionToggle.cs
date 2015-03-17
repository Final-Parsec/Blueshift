using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelSelectionToggle : MonoBehaviour
{
	private GameObject pressStartButton;
	private GameObject levelSelectionCanvas;

	/// <summary>
	/// Use this for initialization.
	/// </summary>
	void Start()
	{
		pressStartButton = GameObject.Find("Press Start");
		levelSelectionCanvas = GameObject.Find("Level Selection");
		
		// Disable buttons for missions we haven't unlocked yet.
		SaveLoad.Load();
		foreach(var button in levelSelectionCanvas.GetComponentsInChildren<Button>())
		{
			var name = button.name;
			var missionNumber = 0;
			int.TryParse(name, out missionNumber);
			
			if (missionNumber > PersistentSettings.Current.highestUnlockedLevel)
			{
				button.interactable = false;
			}
		}
		
		levelSelectionCanvas.SetActive(false);
	}

	/// <summary>
	/// Update is called once per frame.
	/// </summary>
	void Update ()
	{
		if (Input.anyKey)
		{
			pressStartButton.SetActive(false);
			levelSelectionCanvas.SetActive(true);
			
		}
	}
}
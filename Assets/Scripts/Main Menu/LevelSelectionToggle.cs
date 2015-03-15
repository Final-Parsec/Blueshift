using UnityEngine;
using System.Collections;

public class LevelSelectionToggle : MonoBehaviour
{
	private GameObject pressStartButton;
	private GameObject levelSelectionCanvas;
	private GameObject[] missionStartButtons;

	/// <summary>
	/// Use this for initialization.
	/// </summary>
	void Start()
	{
		pressStartButton = GameObject.Find("Press Start");
		levelSelectionCanvas = GameObject.Find("Level Selection");
		levelSelectionCanvas.SetActive(false);
		missionStartButtons = GameObject.FindGameObjectsWithTag(TagsAndEnums.levelSelector);
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
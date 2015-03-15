using UnityEngine;
using System.Collections;

public class LevelSelectionToggle : MonoBehaviour
{
	private GameObject canvas;
	private GameObject levelSelectionCanvas;
	private GameObject[] missionStartButtons;

	/// <summary>
	/// Use this for initialization.
	/// </summary>
	void Start()
	{
		canvas = GameObject.Find("Canvas");
		levelSelectionCanvas = GameObject.Find("Level Selection Canvas");
		missionStartButtons = GameObject.FindGameObjectsWithTag(TagsAndEnums.levelSelector);
	}

	/// <summary>
	/// Update is called once per frame.
	/// </summary>
	void Update ()
	{
		if (Input.anyKey)
		{
			canvas.SetActive(false);
			levelSelectionCanvas.SetActive(true);
		}
	}
}
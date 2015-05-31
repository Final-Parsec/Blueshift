using UnityEngine;
using System;
using System.Collections;

public class PauseMenu : MonoBehaviour
{
	private static PauseMenu instance;

    private InGameDialogue inGameDialogue;
    private GameObject inGameDialogueContainer;
    
    private GameObject pauseEnableButton;
	private GameObject pauseUi;
    int resumeCount = 0;
	
	public static PauseMenu Instance
	{
		get
		{
			if (instance == null)
			{
				throw new InvalidOperationException("Bruh?");
			}
			return instance;
		}
	}

	public void PauseToggle()
	{
        this.inGameDialogue.HideForPause();
        this.inGameDialogueContainer.SetActive(!this.inGameDialogueContainer.activeSelf);
		this.pauseUi.SetActive(!this.pauseUi.activeSelf);
		this.pauseEnableButton.SetActive(!this.pauseEnableButton.activeSelf);
		if (this.pauseUi.activeSelf)
		{
			Time.timeScale = 0f;
		}
		else
		{
			Time.timeScale = 1f;
            resumeCount = 0;
		}
	}
	
	public void TurnOff()
	{
		this.pauseUi.SetActive(false);
	}
	
	/// <summary>
	/// 	Use this for initialization.
	/// </summary>
	void Start ()
	{
		instance = this;
        this.inGameDialogueContainer = GameObject.Find("In-Game Dialogue");
        this.inGameDialogue = this.inGameDialogueContainer.GetComponent<InGameDialogue>();
		this.pauseEnableButton = GameObject.Find("PauseEnableButton");
		this.pauseUi = GameObject.Find("PauseUI");
		this.pauseUi.SetActive(false);
	}
	
	/// <summary>
	/// 	Update is called once per frame.
	/// </summary>
	void Update ()
	{
        if((this.pauseUi.activeSelf && Input.GetMouseButtonUp(0)))
            resumeCount++;

        if ( resumeCount == 2 || Input.GetKeyUp(KeyCode.Escape))
		{
			this.PauseToggle();
		}
	}
}

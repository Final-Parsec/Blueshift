﻿using UnityEngine;
using System;
using System.Collections;

public class PauseMenu : MonoBehaviour
{
	private static PauseMenu instance;
	
	private GameObject pauseEnableButton;
	private GameObject pauseUi;
	
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
		this.pauseUi.SetActive(!this.pauseUi.activeSelf);
		this.pauseEnableButton.SetActive(!this.pauseEnableButton.activeSelf);
		if (this.pauseUi.activeSelf)
		{
			Time.timeScale = 0f;
		}
		else
		{
			Time.timeScale = 1f;
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
		this.pauseEnableButton = GameObject.Find("PauseEnableButton");
		this.pauseUi = GameObject.Find("PauseUI");
		this.pauseUi.SetActive(false);
	}
	
	/// <summary>
	/// 	Update is called once per frame.
	/// </summary>
	void Update ()
	{
		if ((this.pauseUi.activeSelf && Input.GetMouseButtonUp(0)) ||
			Input.GetKeyUp(KeyCode.Escape))
		{
			this.PauseToggle();
		}
	}
}

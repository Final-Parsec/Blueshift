using UnityEngine;
using System;
using System.Collections;

[Serializable]
public class PersistentSettings
{
	public int highestUnlockedLevel;
	
	private static PersistentSettings instance = new PersistentSettings();
	
	private PersistentSettings()
	{
		highestUnlockedLevel = 1;
	}
	
	public static PersistentSettings Current
	{
		get
		{
			return instance;		
		}
		set
		{
			instance = value;
		}
	}
}
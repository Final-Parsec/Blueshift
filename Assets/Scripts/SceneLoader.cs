using UnityEngine;
using System.Collections;

public class SceneLoader : MonoBehaviour {
	void Start()
	{
		SaveLoad.Load();
	}

	public void ReloadCurrentScene()
	{
		Time.timeScale = 1f;
		SaveLoad.Save();
		Application.LoadLevel(Application.loadedLevel);
	}
	
	public static void LoadCutscene(int i)
	{
		Time.timeScale = 1f;
	
		// Special handling for this as it indicates the end of the game.
		if (i == -1)
		{
			Application.LoadLevel("Final Cutscene");
			return;
		}
	
		if (i > PersistentSettings.Current.highestUnlockedLevel)
		{	
			PersistentSettings.Current.highestUnlockedLevel = i;
		}
		SaveLoad.Save();
		Application.LoadLevel("Cutscene " + i);
	}
	
	public void StartCutscene(int i)
	{
		Time.timeScale = 1f;
	
		if (i > PersistentSettings.Current.highestUnlockedLevel)
		{	
			PersistentSettings.Current.highestUnlockedLevel = i;
		}
		SaveLoad.Save();
		Application.LoadLevel("Cutscene " + i);
	}
	
	public void StartMission(int i)
	{
		Time.timeScale = 1f;
	
		SaveLoad.Save();
		Application.LoadLevel("Mission " + i);
	}
	
	public void BackToMainMenu()
	{
		Time.timeScale = 1f;
	
		SaveLoad.Save();
		Application.LoadLevel("Main Menu");
	}
}

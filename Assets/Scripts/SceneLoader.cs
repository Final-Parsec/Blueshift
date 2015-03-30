using UnityEngine;
using System.Collections;

public class SceneLoader : MonoBehaviour {
	void Start()
	{
		SaveLoad.Load();
	}

	public void ReloadCurrentScene()
	{
		SaveLoad.Save();
		Application.LoadLevel(Application.loadedLevel);
	}
	
	public static void LoadCutscene(int i)
	{
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
		if (i > PersistentSettings.Current.highestUnlockedLevel)
		{	
			PersistentSettings.Current.highestUnlockedLevel = i;
		}
		SaveLoad.Save();
		Application.LoadLevel("Cutscene " + i);
	}
	
	public void StartMission(int i)
	{
		SaveLoad.Save();
		Application.LoadLevel("Mission " + i);
	}
	
	public void BackToMainMenu()
	{
		SaveLoad.Save();
		Application.LoadLevel("Main Menu");
	}
}

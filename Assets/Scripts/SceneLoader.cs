using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour {
	void Start()
	{
		SaveLoad.Load();
	}

	public void ReloadCurrentScene()
	{
		Time.timeScale = 1f;
		SaveLoad.Save();
		SceneManager.LoadScene(SceneManager.GetActiveScene().name);
	}
	
	public static void LoadCutscene(int i)
	{
		Time.timeScale = 1f;
	
		// Special handling for this as it indicates the end of the game.
		if (i == -1)
		{
			SceneManager.LoadScene("Final Cutscene");
			return;
		}
	
		if (i > PersistentSettings.Current.highestUnlockedLevel)
		{	
			PersistentSettings.Current.highestUnlockedLevel = i;
		}
		SaveLoad.Save();
		SceneManager.LoadScene("Cutscene " + i);
	}
	
	public void StartCutscene(int i)
	{
		Time.timeScale = 1f;
	
		if (i > PersistentSettings.Current.highestUnlockedLevel)
		{	
			PersistentSettings.Current.highestUnlockedLevel = i;
		}
		SaveLoad.Save();
		SceneManager.LoadScene("Cutscene " + i);
	}
	
	public void StartMission(int i)
	{
		Time.timeScale = 1f;
	
		SaveLoad.Save();
		SceneManager.LoadScene("Mission " + i);
	}
	
	public void BackToMainMenu()
	{
		Time.timeScale = 1f;
	
		SaveLoad.Save();
		SceneManager.LoadScene("Main Menu");
	}
}

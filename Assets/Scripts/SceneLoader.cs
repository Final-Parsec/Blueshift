using UnityEngine;
using System.Collections;

public class SceneLoader : MonoBehaviour {
	public void ReloadCurrentScene()
	{
		Application.LoadLevel(Application.loadedLevel);
	}
	
	public void StartCutscene(int i)
	{
		Application.LoadLevel("Cutscene " + i);
	}
	
	public void StartMission(int i)
	{
		Application.LoadLevel("Mission " + i);
	}
	
	public void BackToMainMenu()
	{
		Application.LoadLevel("Main Menu");
	}
}

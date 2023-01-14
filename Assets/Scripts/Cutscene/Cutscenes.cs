using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Text.RegularExpressions;
using System.Diagnostics;

public class Cutscenes : MonoBehaviour{
	public string sceneToLoad;
	
	CutscenesIO io;
	Text dialogue;
	Text speakerName;
	Image portrait;
	
	private int curLine = 0;
	private bool talking = false;
	private AudioSource audiosource;
	private float cortime = 0;
	private int corCharCount = 0;
	private string toSay;
	private Stopwatch stopwatch = new Stopwatch();
	private int waitTime = 5000;

	void Start(){
		io = new CutscenesIO (SceneManager.GetActiveScene().name);
		dialogue = GameObject.Find ("Dialogue").GetComponent<Text>();
		speakerName = GameObject.Find ("CharName").GetComponent<Text> ();
		portrait = GameObject.Find ("Portrait").GetComponent<Image>();
		
		SetSpeaker (io.GetName (curLine));
		StartCoroutine(SayDialogue());
		//audiosource = GetComponent<AudioSource> ();
	}

	/// <summary>
	/// Go to next line. if speaker is done talking, restart the coroutine.
	/// setimage, speaker, line.
	/// </summary>
	public void NextLine()
	{
		corCharCount = 0;
		curLine++;
		toSay = io.GetLine (curLine);
		
		var currentSpeaker = io.GetName(curLine);
		if (toSay == CutscenesIO.EndOfCutscene ||
		    currentSpeaker == CutscenesIO.EndOfCutscene)
		{
			currentSpeaker = "Loading " + sceneToLoad + "...";
			SetSpeaker(currentSpeaker);
			SceneManager.LoadScene(this.sceneToLoad);
		}
		else
		{
			SetSpeaker(currentSpeaker);
			if (!talking)
				StartCoroutine (SayDialogue ());
		}
	}
	
	void Update()
	{	
		if (!talking && !stopwatch.IsRunning) 
		{
			stopwatch.Start();
		}

		if (Input.anyKeyDown)
		{
			if (stopwatch.IsRunning) 
			{
				waitTime = 10000;
				stopwatch.Stop();
				stopwatch.Reset();
			}

			if(talking)
				FinishCurrentLine();
			else
				NextLine ();
		}

		if(stopwatch.ElapsedMilliseconds >= waitTime)
		{
			NextLine ();
			waitTime = 5000;
			stopwatch.Stop();
			stopwatch.Reset();
		}
	}

	/// <summary>
	/// Starts the dialogue and scrolls it ever .o2 sec using a waitforseconds
	/// </summary>
	IEnumerator SayDialogue () {
		talking = true;

		if(curLine == 0)
			toSay = io.GetLine (curLine);

		toSay = Regex.Replace (toSay, @"\r\n|\n|\r", "");//normalize curLine

		while (corCharCount <= toSay.Length){
			dialogue.text = toSay.Substring(0, corCharCount);


			//if()  //check if it's an integer
			if(corCharCount > 0 && (toSay[corCharCount-1] != ' ' || toSay[corCharCount-1] != '\n')){
				GetComponent<AudioSource>().Play();
				//Debug.Log (toSay[corCharCount-1]);
			}

			if(!talking)
				yield break;

			if((corCharCount > 0) && (toSay[corCharCount-1] == '.' || toSay[corCharCount-1] == '?' || toSay[corCharCount-1] == '!')){
				cortime+= .05f;
				yield return new WaitForSeconds (0.5f);
			}
			else{
				cortime += .02f;
				yield return new WaitForSeconds (0.02f);
			}
			corCharCount++;
		}
		
		//while (!Input.GetMouseButton(0))
			//yield return;

		talking = false;
	}


	/// <summary>
	/// Skips to the end of the current line of dialogue.
	/// </summary>
	void FinishCurrentLine(){
		corCharCount = toSay.Length-1;
		talking = false;
	}


	void SetSpeaker(string name){
		speakerName.text = name;
		var portraitImage = (Texture2D)Resources.Load ("Portraits/" + name);
		
		if (portraitImage == null)
		{
			dialogue.enabled = false;
			portrait.enabled = false;
		}
		else
		{
			dialogue.enabled = true;
			portrait.enabled = true;
			
			portrait.color = new Color(255, 255, 255, 245);
			portrait.sprite = Sprite.Create (portraitImage, new Rect(0,0, portraitImage.width, portraitImage.height), new Vector2(0, 0));
		}
	}



}
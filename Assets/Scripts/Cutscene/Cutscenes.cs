using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Text.RegularExpressions;

public class Cutscenes : MonoBehaviour{
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

	void Start(){
		io = new CutscenesIO (Application.loadedLevelName);
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
		SetSpeaker (io.GetName (curLine));
		if (!talking)
			StartCoroutine (SayDialogue ());
	}
	
	void Update()
	{
		if (Input.anyKeyDown)
		{
			if(talking)
				FinishCurrentLine();
			else
				NextLine ();
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
		Debug.Log (Resources.Load ("Portraits/" + name));
		portrait.sprite = Sprite.Create ((Texture2D)Resources.Load ("Portraits/" + name), new Rect(1,1,605,799), new Vector2(0, 0));//TODO: use screen.width/screen.height and make a percentage of it
	}



}
using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Cutscenes : MonoBehaviour{
	CutscenesIO io;
	int curLine = 0;
	Text dialogue;
	Text speakerName;
	bool talking = false;
	private AudioSource audiosource;
	float cortime = 0;
	int corCharCount = 0;
	string toSay;

	void Start(){
		io = new CutscenesIO (Application.loadedLevelName);
		dialogue = GameObject.Find ("Dialogue").GetComponent<Text>();
		speakerName = GameObject.Find ("CharName").GetComponent<Text> ();

		StartCoroutine(SayDialogue());
		//audiosource = GetComponent<AudioSource> ();
	}

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
		if (Input.anyKeyDown && cortime > .1)
		{
			NextLine ();
		}
	}

	IEnumerator SayDialogue () {
		talking = true;
		SetSpeaker (io.GetName (curLine));
		toSay = io.GetLine (curLine);
		while (corCharCount <= toSay.Length){
			dialogue.text = toSay.Substring(0, corCharCount);
			//audiosource.Play ();
			//Debug.Log ("play");
			cortime += .02f;
			yield return new WaitForSeconds (0.02f);
			corCharCount++;
		}
		
		//while (!Input.GetMouseButton(0))
			//yield return;

		talking = false;

	}

	void SetSpeaker(string name){
		speakerName.text = name;
	}



}
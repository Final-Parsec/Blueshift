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

	void Start(){
		io = new CutscenesIO (Application.loadedLevelName);
		dialogue = GameObject.Find ("Dialogue").GetComponent<Text>();
		speakerName = GameObject.Find ("CharName").GetComponent<Text> ();
		//audiosource = GetComponent<AudioSource> ();
	}

	public void NextLine()
	{
		SetSpeaker (io.GetName(curLine));
		StartCoroutine(SayDialogue(io.GetLine (curLine)));
		curLine++;
	}

	IEnumerator SayDialogue (string toSay) {
		talking = true;
		
		for (int i = 0; i <= toSay.Length; i ++) {
			dialogue.text = toSay.Substring(0, i);
			//audiosource.Play ();
			Debug.Log ("play");
			yield return new WaitForSeconds (0.02f);
		}
		
		//while (!Input.GetMouseButton(0))
			//yield return new WaitForEndOfFrame();

		talking = false;

	}

	void Update()
	{
		if (Input.anyKeyDown)
		{
			NextLine ();
		}
	}

	void SetSpeaker(string name){
		speakerName.text = name;
	}



}
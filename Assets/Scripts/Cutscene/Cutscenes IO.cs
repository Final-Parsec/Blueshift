using UnityEngine;
using System.Collections;
using System.IO;

public class CutscenesIO{
	string[] lines;
	public CutscenesIO (string filename) 
	{
		StreamReader reader = File.OpenText(@"Assets\Cutscenes\Dialogue\" + filename + ".txt");
		string text = reader.ReadToEnd ();
		lines = text.Split('\n');
	}

	//Returns the specified line of dialogue.
	public string GetLine(int dialogue_number)
	{
		if (lines.Length-1 >= dialogue_number) 
		{
			string[] str = lines [dialogue_number].Split ('@');
			return str [1];
		}
		return "You outa lines, nigga.";
	}

	//Returns the specified name attached to dialogue.
	public string GetName(int dialogue_number)
	{
		if (lines.Length-1 >= dialogue_number) 
		{
			string[] str = lines [dialogue_number].Split ('@');
			return str [0];
		}
		return "You outa lines, nigga.";
		
	}
}

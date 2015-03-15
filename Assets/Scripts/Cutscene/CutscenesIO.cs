using UnityEngine;
using System.Collections;
using System.IO;

/// <summary>
/// Reads scripts for cutscenes from a file.
/// </summary>
public class CutscenesIO
{
	string[] lines;
	
	public const string EndOfCutscene = "You outa lines, nigga.";
	
	/// <summary>
	/// Initializes a new instance of the <see cref="CutscenesIO"/> class.
	/// </summary>
	/// <param name="filename">Filename for script text file.</param>
	public CutscenesIO (string filename) 
	{
		var scriptAsset = Resources.Load(@"Dialogue/" + filename) as TextAsset;
		var scriptContent = scriptAsset == null ? string.Empty : scriptAsset.text;
		lines = scriptContent.Split('\n');
	}

	/// <summary>
	/// Returns the specified line of dialogue.
	/// </summary>
	/// <returns>The line.</returns>
	/// <param name="dialogue_number">Dialogue line number.</param>
	public string GetLine(int dialogue_number)
	{
		if (lines.Length-1 >= dialogue_number) 
		{
			string[] str = lines [dialogue_number].Split ('@');
			return str [1];
		}
		
		return CutscenesIO.EndOfCutscene;
	}

	/// <summary>
	/// Returns the specified name attached to dialogue.
	/// </summary>
	/// <returns>The speaker's name.</returns>
	/// <param name="dialogue_number">Dialogue line number.</param>
	public string GetName(int dialogue_number)
	{
		if (lines.Length-1 >= dialogue_number) 
		{
			string[] str = lines [dialogue_number].Split ('@');
			return str [0];
		}
		
		return CutscenesIO.EndOfCutscene;
	}
}
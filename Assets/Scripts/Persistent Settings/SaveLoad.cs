using UnityEngine;
using System.Collections;
using System.Collections.Generic; 
using System.IO;
using System.Runtime.Serialization.Formatters.Binary; 

public class SaveLoad
{
	private static readonly string PersistentSettingsFile = Application.persistentDataPath + "/LegacyEarth.fpbs";

	public static void Load()
	{
		if (File.Exists(PersistentSettingsFile))
		{
			var binaryFormatter = new BinaryFormatter();
			var file = File.Open(PersistentSettingsFile, FileMode.Open);
			PersistentSettings.Current = (PersistentSettings)binaryFormatter.Deserialize(file);
			file.Close();
		}
	}

	public static void Save()
	{
		var binaryFormatter = new BinaryFormatter();
		var file = File.Create (PersistentSettingsFile);
		binaryFormatter.Serialize(file, PersistentSettings.Current);
		file.Close();
	}
}
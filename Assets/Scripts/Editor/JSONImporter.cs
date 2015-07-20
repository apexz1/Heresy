// Creates or rewrites a .txt file for each .resx file in the same folder
// whenever the .resx changes

using UnityEditor;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public class JSONImporter : AssetPostprocessor 
{
	public static void OnPostprocessAllAssets(string[] importedAssets, string[] deletedAssets, string[] movedAssets, string[] movedFromAssetPaths)
	{
		foreach (string asset in importedAssets)
		{
			if (asset.EndsWith(".json"))
			{
				string filePath = asset.Substring(0, asset.Length - Path.GetFileName(asset).Length) + "Resources/";
				string newFileName = filePath + Path.GetFileNameWithoutExtension(asset) + ".txt";
				
				if (!Directory.Exists(filePath))
				{
					Directory.CreateDirectory(filePath);
				}
				
				StreamReader reader = new StreamReader(asset);
				string fileData = reader.ReadToEnd();
				reader.Close();

				FileStream resourceFile = new FileStream(newFileName, FileMode.OpenOrCreate, FileAccess.Write);
				StreamWriter writer = new StreamWriter(resourceFile);
				writer.Write(fileData);
				writer.Close();
				resourceFile.Close();
				
				AssetDatabase.Refresh(ImportAssetOptions.Default);
			}
		}
	}
	
}
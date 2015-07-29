using UnityEngine;
using System;
using System.IO;
using System.Collections;

public class LoadTextures {

    public static Texture2D LoadImage(string filePath)
    {

        Texture2D tex = new Texture2D(2, 2);
        byte[] fileData;

        if (File.Exists(filePath))
        {
            fileData = File.ReadAllBytes(filePath);
            tex.LoadImage(fileData); //..this will auto-resize the texture dimensions.
        }
        //Debug.Log(tex.ToString());
        return tex;
    }

    public static void LoadFromFile(string filePath)
    {

        string[] fileArray = Directory.GetFiles(filePath);

        for (int i = 0; i < fileArray.Length; i++)
        {
            Texture2D tex = LoadImage(fileArray[i]);
            //Debug.Log(fileArray[i].ToString());

            string oldName = fileArray[i].ToString();
            string tmpName = oldName.Remove(0, oldName.Length - 7);
            string newName = tmpName.Remove(tmpName.Length - 4, 4);
            int texName;
            Int32.TryParse(newName, out texName);
            //Debug.Log(texName);

            for (int j = 0; j < CardLibrary.Get().cardList.Count; j++)
            {
                if (CardLibrary.Get().cardList[j].cardID == texName)
                {
                    //Debug.Log("name found");
                    CardLibrary.Get().cardList[j].texture = tex;
                }
            }
        }
    }
}

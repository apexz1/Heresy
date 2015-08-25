using UnityEngine;
using System;
using System.IO;
using System.Collections;

public class LoadTextures
{

    public static Texture2D LoadImage( string filePath )
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

    public static void LoadFromFile(int texId)
    {
        string skin = "";
        //string[] fileArray = Directory.GetFiles(filePath);
        var array = Resources.LoadAll("Images/", typeof(Texture2D));

        Debug.Log(OptionsMenu.isDarkFantasy + " " + OptionsMenu.isWonderland);

        if (OptionsMenu.isDarkFantasy)
        {
            skin = "cards_DF/";
            Debug.Log(skin);
        }
        else if (OptionsMenu.isWonderland)
        {
            skin = "cards_WL/";
            Debug.Log(skin);
        }
        else
        {
            Debug.LogError("NO SKIN ASSIGNED;");
            Debug.Log(skin);
        }

        Debug.Log(skin);

        if (texId == 0)
        {
            array = Resources.LoadAll("Images/" + skin + "inPlay", typeof(Texture2D));
        }
        if (texId == 1)
        {
            array = Resources.LoadAll("Images/" + skin + "preview", typeof(Texture2D));
        }
        var imgArray = new Texture2D[array.Length];

        for (int i = 0; i < array.Length; i++)
        {
            imgArray[i] = array[i] as Texture2D;
        }

        for (int i = 0; i < imgArray.Length; i++)
        {
            Debug.Log("texname " + imgArray[i].name);
            for (int j = 0; j < CardLibrary.Get().cardList.Count; j++)
            {
                if (CardLibrary.Get().cardList[j].cardID == Int32.Parse(imgArray[i].name))
                {
                    if (texId == 0)
                    {
                        CardLibrary.Get().cardList[j].texture = imgArray[i];
                    }
                    if (texId == 1)
                    {
                        CardLibrary.Get().cardList[j].texture_p = imgArray[i];
                    }
                    /*
                    else
                    {
                        GameManager.Get().SendNotification(GameManager.Get().localPlayerId, "Unable to load textures, texture load index invalid");
                    }
                     * */
                }
            }
        }
    }
}

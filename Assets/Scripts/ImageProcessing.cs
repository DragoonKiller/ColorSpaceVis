using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public static class ImageProcessing
{
    public static Color[,] ToColorArray(this Texture2D tex)
    {
        var cc = tex.GetPixels();
        var res = new Color[tex.height, tex.width];
        for(int y = 0; y < tex.height; y ++) for(int x = 0; x < tex.width; x++)
        {
            res[y, x] = cc[y * tex.width + x];
        }
        return res;
    }
    
    public static Texture2D ReadImage(string path)
    {
        var uri = "file:///" + UnityWebRequest.EscapeURL(path);
        System.IO.File.WriteAllText("G:/log.txt", uri);
        var req = UnityWebRequest.Get(uri);
        
        req.downloadHandler = new DownloadHandlerBuffer();
        req.SendWebRequest();
        var baseTime = System.DateTime.Now;
        while(!req.downloadHandler.isDone)
        {
            var curTime = System.DateTime.Now;
            var deltaTime = curTime - baseTime;
            if(deltaTime > System.TimeSpan.FromSeconds(1.0)) break;
        }
        if(!req.downloadHandler.isDone) throw new System.Exception();
        var tex = new Texture2D(0, 0);
        ImageConversion.LoadImage(tex, req.downloadHandler.data);
        return tex;
    }
    
}

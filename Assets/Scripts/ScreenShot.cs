using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
 
public class ScreenShot : MonoBehaviour {
    public Camera targetCamera;
    public string outputPath = "/ScreenShot/";
    private int resWidth;
    private int resHeight;
    string path;

    void Start () {
        resWidth = Screen.width;
        resHeight = Screen.height;
        path = Application.dataPath + outputPath;
        Debug.Log(path);
    }
 
    public void ClickScreenShot()
    {
        DirectoryInfo dir = new DirectoryInfo(path);
        if (!dir.Exists)
        {
            Directory.CreateDirectory(path);
        }
        string name;
        name = path + System.DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss") + ".png";
        RenderTexture rt = new RenderTexture(resWidth, resHeight, 24);
        targetCamera.targetTexture = rt;
        Texture2D screenShot = new Texture2D(resWidth, resHeight, TextureFormat.RGB24, false);
        Rect rec = new Rect(0, 0, screenShot.width, screenShot.height);
        targetCamera.Render();
        RenderTexture.active = rt;
        screenShot.ReadPixels(new Rect(0, 0, resWidth, resHeight), 0, 0);
        screenShot.Apply();
 
        byte[] bytes = screenShot.EncodeToPNG();
        File.WriteAllBytes(name, bytes);
    }
}
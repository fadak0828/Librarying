using System;
using System.Collections;
using System.IO;
using UnityEngine;
using UnityEngine.Android;
using UnityEngine.UI;

// 매니페스트 선언필요.  <uses-permission android:name="android.permission.WRITE_EXTERNAL_STORAGE" />
//https://answers.unity.com/questions/200173/android-how-to-refresh-the-gallery-.html
public class ScreenShot : MonoBehaviour
{
    public GameObject screenshotBlinker;
    public Animator btnGalleryAnim;
    string _name = "";

    public void OpenGallery()
    {
#if UNITY_ANDROID

        AndroidJavaClass unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
        AndroidJavaObject unityActivity = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity");
        AndroidJavaClass intentStaticClass = new AndroidJavaClass("android.content.Intent");
        string actionView = intentStaticClass.GetStatic<string>("ACTION_VIEW");
        AndroidJavaClass uriClass = new AndroidJavaClass("android.net.Uri");
        AndroidJavaObject uriObject = uriClass.CallStatic<AndroidJavaObject>("parse", "content://media/external/images/media");
        AndroidJavaObject intent = new AndroidJavaObject("android.content.Intent", actionView, uriObject);
        unityActivity.Call("startActivity", intent);
#endif
    }

    public void CaptureScreenshot()
    {
        _name = "";
        _name = "Screenshot_" + GetCurTime() + ".png";

#if UNITY_ANDROID
        StartCoroutine(CutImage(_name));
#endif
    }
    //Screen capture and save
    IEnumerator CutImage(string name)
    {
        Blink();
        //size of picture     
        Texture2D tex = new Texture2D(Screen.width, Screen.height, TextureFormat.RGB24, true);
        yield return new WaitForEndOfFrame();
        tex.ReadPixels(new Rect(0, 0, Screen.width, Screen.height), 0, 0, true);
        tex.Apply();
        yield return tex;
        byte[] byt = tex.EncodeToPNG();

        string path = Application.persistentDataPath.Substring(0, Application.persistentDataPath.IndexOf("Android"));

        File.WriteAllBytes(path + "/DCIM/Camera/" + name, byt); //Save to the Camera folder under DCIM/ on Android phones
        // File.WriteAllBytes(path + "/screenshot /" + name, byt); //Save to the "Screenshot" folder under File Management on Android Phone      
        string[] paths = new string[1];
        paths[0] = path;
        ScanFile(paths);
    }
    //Refresh the image and display it in the album.
    void ScanFile(string[] path)
    {
        using (AndroidJavaClass PlayerActivity = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
        {
            AndroidJavaObject playerActivity = PlayerActivity.GetStatic<AndroidJavaObject>("currentActivity");
            using (AndroidJavaObject Conn = new AndroidJavaObject("android.media.MediaScannerConnection", playerActivity, null))
            {
                Conn.CallStatic("scanFile", playerActivity, path, null, null);
            }
        }

    }
    string GetCurTime()
    {
        return DateTime.Now.Year.ToString() + DateTime.Now.Month.ToString() + DateTime.Now.Day.ToString()
            + DateTime.Now.Hour.ToString() + DateTime.Now.Minute.ToString() + DateTime.Now.Second.ToString();
    }

    [ContextMenu("Blink")]
    private void Blink()
    {
        screenshotBlinker.GetComponent<Animator>().Play("ScreenshotBlink");
        StartCoroutine(IeShowBtnGallery());
    }

    private IEnumerator IeShowBtnGallery()
    {
        btnGalleryAnim.Play("BounceIn");
        yield return new WaitForSeconds(3);
        btnGalleryAnim.Play("BounceOut");
    }
}
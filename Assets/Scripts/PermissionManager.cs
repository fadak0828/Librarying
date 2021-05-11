using System;
using System.Collections;
using System.IO;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Android;

// unity가 자동으로 메니페스트 수정 안해주는 경우
// 요청할 권한 메니페스트에 선언 해두어야 함. 
// unity 자동 권한 생성 목록        : https://docs.unity3d.com/kr/2018.4/Manual/android-manifest.html
// 안드로이드 manifest 권한 목록    : https://developer.android.com/reference/android/Manifest.permission?hl=ko
public class PermissionManager : MonoBehaviour
{
    public static PermissionManager Instance;
    private void Awake() {
        Instance = this;
    }

    // Permission
    public void CheckPermission(string targetPermission) {
        StartCoroutine(PermissionCheckCoroutine(targetPermission));
    }

    IEnumerator PermissionCheckCoroutine(string targetPermission, UnityAction onSuccess = null, UnityAction onFailed = null)
    {
        yield return new WaitForEndOfFrame();
        if (Permission.HasUserAuthorizedPermission(targetPermission) == false)
        {
            Permission.RequestUserPermission(targetPermission);

            yield return new WaitForSeconds(0.2f); // 0.2초의 딜레이 후 focus를 체크하자.
            yield return new WaitUntil(() => Application.isFocused == true);

            if (Permission.HasUserAuthorizedPermission(targetPermission) == false)
            {
                if (onFailed != null) onFailed();
                yield break;
            }
        }

        if (onSuccess != null) onSuccess(); 
    }


    // 해당 앱의 설정창을 호출한다.
    // https://forum.unity.com/threads/redirect-to-app-settings.461140/
    private void OpenAppSetting()
    {
        try
        {
#if UNITY_ANDROID
            using (var unityClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
            using (AndroidJavaObject currentActivityObject = unityClass.GetStatic<AndroidJavaObject>("currentActivity"))
            {
                string packageName = currentActivityObject.Call<string>("getPackageName");

                using (var uriClass = new AndroidJavaClass("android.net.Uri"))
                using (AndroidJavaObject uriObject = uriClass.CallStatic<AndroidJavaObject>("fromParts", "package", packageName, null))
                using (var intentObject = new AndroidJavaObject("android.content.Intent", "android.settings.APPLICATION_DETAILS_SETTINGS", uriObject))
                {
                    intentObject.Call<AndroidJavaObject>("addCategory", "android.intent.category.DEFAULT");
                    intentObject.Call<AndroidJavaObject>("setFlags", 0x10000000);
                    currentActivityObject.Call("startActivity", intentObject);
                }
            }
#endif
        }
        catch (Exception ex)
        {
            Debug.LogException(ex);
        }
    }
}
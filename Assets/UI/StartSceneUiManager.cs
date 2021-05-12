using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartSceneUiManager : MonoBehaviour
{
    public Popup_Common popupExit;
    public Popup_Settings popupSettings;

    public void OnClickBtnExit() {
        print("test");
        popupExit.gameObject.SetActive(true);
    }

    public void OnClickSetting() {
        popupSettings.gameObject.SetActive(true);
    }

    public void GoToStartScene() {
        SceneManager.LoadScene(0);
    }

    public void GoToArScene() {
        Invoke("LoadArScene", 2);
    }

    private void LoadArScene() {
        SceneManager.LoadScene(1);
    }

    public void QuitApplication() {
        Application.Quit();
    }
}

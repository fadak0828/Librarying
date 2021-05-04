using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartSceneUiManager : MonoBehaviour
{
    public Popup_Common popupExit;
    public Popup_Settings popupSettings;

    public void OnClickBtnExit() {
        print("test");
        popupExit.gameObject.SetActive(true);
    }

    public void OnClickSetting() {
        print("test22");

        popupSettings.gameObject.SetActive(true);
    }
}

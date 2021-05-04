using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartSceneUiManager : MonoBehaviour
{
    public Popup_Common popupExit;
    public Popup_Settings popupSettings;

    public void OnClickBtnExit() {
        popupExit.gameObject.SetActive(true);
    }

    public void OnClickSetting() {
        popupSettings.gameObject.SetActive(true);
    }
}

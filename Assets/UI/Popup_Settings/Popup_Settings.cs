using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Popup_Settings : MonoBehaviour
{
    public Slider musicVolumeSlider;
    public Slider narrationVolumeSlider;
    private Animator animator;

    private void Awake() {
        animator = GetComponent<Animator>();
    }

    private void OnEnable() {
        animator.Play("open");

        musicVolumeSlider.value = SettingManager.Instance.currentSetting.musicVolume;
        narrationVolumeSlider.value = SettingManager.Instance.currentSetting.narrationVolume;
    }

    public void OnClickClose() {
        ClosePopup();
    }

    private void ClosePopup() {
        StartCoroutine(IeClosePopup());
    }

    private IEnumerator IeClosePopup() {
        animator.Play("close");
        yield return new WaitForSeconds(0.58f);
        gameObject.SetActive(false);
    }
}

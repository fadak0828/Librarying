using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OnboardingUI {
    public readonly static string ZOOM_QR = "ZoomQR";
    public readonly static string SHAKE_PHONE = "ShakePhone";
}

public class OnboadringUIManager : MonoBehaviour
{
    [SerializeField]
    private string TestUI;
    private Animator animator;

    private void OnEnable() {
        animator = GetComponent<Animator>();
        InitOnboardUI();
    }

    private void InitOnboardUI() {
        GetComponent<Image>().color = new Color(0, 0, 0, 0);
        HideAllOnboardingUI();
    }

    private void HideAllOnboardingUI() {
        for(int i = 0; i < transform.childCount; i++) {
            transform.GetChild(i).gameObject.SetActive(false);
        }
    }

    public void ShowUiByName(string name) {
        animator.Play($"Enter{name}");
    }

    public void HideUiByName(string name) {
        animator.Play($"Exit{name}");
    }

    [ContextMenu("ShowTestUI")]
    private void ShowTestUI() {
        ShowUiByName(TestUI);
    }

    [ContextMenu("HideTestUI")]
    private void HideTestUI() {
        HideUiByName(TestUI);
    }
}

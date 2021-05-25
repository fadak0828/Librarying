using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NextPageTimer : MonoBehaviour
{
    public float delay = 10;
    public bool isManual = false;

    private void OnEnable() {
        if (!isManual) {
            Invoke("ShowFindNextPageUI", delay);
        }
    }

    private void OnDisable() {
        CancelInvoke();    
    }

    public void ShowFindNextPageUI() {
        OnboardingUIManager.Instance.ShowUiByName("FindNextPage");
    }
}

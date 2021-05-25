using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NextPageTimer : MonoBehaviour
{
    public float delay = 10;
    public float timer = 0;

    private void OnEnable() {
        timer = 0;
    }

    private void Update() {
        timer += Time.deltaTime;

        if (timer >= delay) {
            ShowFindNextPageUI();
        }
    }

    public void ShowFindNextPageUI() {
        OnboardingUIManager.Instance.ShowUiByName("FindNextPage");
    }
}

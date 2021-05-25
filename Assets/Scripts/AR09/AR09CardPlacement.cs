using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AR09CardPlacement : CardPlacement
{
    public static int knockCount = 0;
    public AudioSource audioSource;
    private KnockEffect knock;

    protected override void OnEnable() {
        base.OnEnable();
        knock = GetComponent<KnockEffect>();
        knockCount = 0;
    }

    protected override void OnCardPlacement()
    {
        base.OnCardPlacement();
        print("KnockCard Placed");
        knock.PlayKnock();
        knockCount++;
        if (knockCount == 3) {
            Invoke("PlayVoice", 4);
            Invoke("ShowNextPageUI", 8);
        }
    }

    private void PlayVoice() {
        audioSource.Stop();
        audioSource.Play();
    }

    private void ShowNextPageUI() {
        OnboardingUIManager.Instance.ShowUiByName("FindNextPage");
    }
}

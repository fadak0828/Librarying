using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public enum ArAnimationType {
    NONE,
    GROW_UP,
    GROW_DOWN
}

public class ArAnimation : MonoBehaviour
{

    private Tween currentTween;

    public void StartAnim(ArAnimationType animationType, TweenCallback callback = null, float duration = 0.7f) {
        switch(animationType) {
            case ArAnimationType.GROW_UP:
                currentTween = transform.DOScale(Vector3.zero, duration)
                    .From()
                    .SetEase(Ease.OutBack)
                    .OnComplete(callback);
                break;

            case ArAnimationType.GROW_DOWN:
                currentTween = transform.DOScale(Vector3.zero, duration)
                    .SetEase(Ease.OutBack);

                currentTween.OnComplete(() => {
                    currentTween.Rewind();
                    callback();
                });
                break;
        }
    }   

    public bool IsPlaying() {
        return currentTween != null && currentTween.IsActive() && currentTween.IsPlaying();
    }
}

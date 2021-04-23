using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ArAnimationBase : MonoBehaviour
{
    public readonly AnimationCurve animationCurve;

    private Coroutine animationCoroutine;

    private void OnEnable() {
        Initialize();
    }

    protected virtual void Initialize() {

    }

    public abstract void StartAnim();
    public abstract void StopAnim();
}
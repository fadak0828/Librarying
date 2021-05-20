using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AR10AnimalAnimationController : MonoBehaviour
{
    public string walkClipName;
    public string cleanClipName;
    public string idleClipName;
    public string finishCleanClipName;
    private Animator animator;
    private void OnEnable() {
        animator = GetComponent<Animator>();
    }

    public void PlayAnim() {
        Invoke("PlayWalkAnim", Random.Range(1, 3));
        Invoke("PlayCleanAnim", Random.Range(3, 5));
    }

    public void PlayWalkAnim() {
        animator.Play(walkClipName);
    }
    public void PlayCleanAnim() {
        animator.Play(cleanClipName);
    }

    public void PlayIdle() {
        animator.Play(idleClipName);
    }

    public void PlayCleanFinish() {
        animator.Play(finishCleanClipName);
    }
}

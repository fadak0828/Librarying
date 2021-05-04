using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class BridgeAnim : MonoBehaviour
{
    private Animator animator;

    private void Start() {
        animator = GetComponent<Animator>();
    }

    [ContextMenu("PlayBounceAnim")]
    public void PlayBounceAnim() {
        // transform.DOJump(transform.position, 2, 1, 0.5f);
        animator.Play("BridgeBounce");
        print("play bounce anim");
    }
}

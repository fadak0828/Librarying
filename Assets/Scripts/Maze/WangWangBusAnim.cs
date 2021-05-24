using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class WangWangBusAnim : MonoBehaviour
{
    public GameObject dustPref;
    public void PlayAnim() {
        GetComponent<Animator>().Play("WangWangBus");
        // StartJumpAnim();
    }
    public void StartJumpAnim() {
        Vector3 endValue = transform.localPosition;
        endValue.y += 10;
        transform.DOLocalJump(endValue, 5, 20, 5, true);
    }
}

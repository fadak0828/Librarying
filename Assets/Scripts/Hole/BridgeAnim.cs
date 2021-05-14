using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class BridgeAnim : MonoBehaviour
{
    [ContextMenu("PlayBounceAnim")]
    public void PlayBounceAnim() {
        transform.DOPunchPosition(Vector3.down, 0.4f);
    }

    public void FallDown(string direction) {
        if (direction == "left") {
            transform.DOLocalRotate(new Vector3(15, 90, 180), 0.5f);
        } else {
            transform.DOLocalRotate(new Vector3(15, -90, 0), 0.5f);
        }
    }
}

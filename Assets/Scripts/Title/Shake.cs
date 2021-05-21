using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Shake : MonoBehaviour
{
    public void DoShake() {
        transform.DOShakePosition(0.3f, 3);
    }
}

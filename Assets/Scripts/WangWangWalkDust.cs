using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WangWangWalkDust : MonoBehaviour
{
    public Transform leftFoot;
    public Transform rightFoot;
    public GameObject dustPref;
    public Vector3 positionOffset;
    public float scaleOffset;

    private void CreateDust(Transform landedFootTransform) {
        Instantiate(dustPref, landedFootTransform.position + positionOffset, landedFootTransform.rotation).transform.localScale *= transform.lossyScale.magnitude * scaleOffset;
    }

    public void OnLandLeftFoot() {
        CreateDust(leftFoot);
    }

    public void OnLandRightFoot() {
        CreateDust(rightFoot);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WangWangBusDust : MonoBehaviour
{
    public GameObject dustPref;
    public Vector3 positionOffset;
    public float scaleOffset;

    public void CreateDust() {
        GameObject dust = Instantiate(dustPref, transform);
        dust.transform.localPosition = positionOffset;
        dust.transform.localScale *= scaleOffset;
        dust.transform.parent = null;
    }
}

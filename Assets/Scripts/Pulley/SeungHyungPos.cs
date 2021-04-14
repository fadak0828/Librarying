using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Constants;

public class SeungHyungPos : MonoBehaviour
{
    [Tooltip("사용 가능한지?")]
    public bool isUsable = true;

    private void Awake() {
        gameObject.tag = Tag.SEUNG_HYUNG_POS;
    }
}

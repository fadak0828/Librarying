using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PulleyMass : MonoBehaviour
{
    public int mass = 1;

    private void Awake() {
        gameObject.layer = LayerMask.NameToLayer("PulleyMass");
    }
}

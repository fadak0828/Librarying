using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeungHyung : MonoBehaviour
{
    public GameObject spawnEffectPref;
    private void Start() {
        Instantiate(spawnEffectPref, transform).transform.parent = null;
    }

    private void OnTriggerEnter(Collider other) {
        if (other.gameObject.name.Contains("Basket")) {
            GetComponent<Rigidbody>().isKinematic = true;
        }
    }
}

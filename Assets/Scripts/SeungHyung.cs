using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeungHyung : MonoBehaviour
{
    private void Update() {
    }

    private void OnTriggerEnter(Collider other) {
        if (other.gameObject.name.Contains("Basket")) {
            GetComponent<Rigidbody>().isKinematic = true;
        }
    }
}

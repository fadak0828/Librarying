using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WangWangPower : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        other.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
    }
}

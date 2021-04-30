using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WangWangPower : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<Rigidbody>() != null)
            other.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
    }
}

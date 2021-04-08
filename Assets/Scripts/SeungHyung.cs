using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeungHyung : MonoBehaviour
{
    public float speed = 2;
    void Start()
    {
        
    }

    void Update()
    {
        transform.Translate(transform.forward * Time.deltaTime * speed);
    }
}

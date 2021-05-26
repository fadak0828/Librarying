using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UiBillboard : MonoBehaviour
{
    private Camera cam;
    private void Start()
    {
        cam = Camera.main;
    }
    void Update()
    {
        transform.LookAt(cam.transform.position);
    }
}

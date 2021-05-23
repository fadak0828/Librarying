using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AR4_TalkBublleControll : MonoBehaviour
{
    Camera cam;
    private void Start()
    {
        cam = Camera.main;
    }
    void Update()
    {
        transform.LookAt(cam.transform.position);
        transform.Rotate(Vector3.up * 45);
        //transform.eulerAngles = Camera.main.transform.eulerAngles;
    }
}

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
        transform.forward = cam.transform.position;
        //transform.eulerAngles = Camera.main.transform.eulerAngles;
    }
}

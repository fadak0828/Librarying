using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public GameObject shakeScreen;
    public GameObject zoomQR;
    void Start()
    {
        shakeScreen.SetActive(false);
        zoomQR.SetActive(false);
    }

    void Update()
    {
        int count=0;
        if (GameObject.FindGameObjectsWithTag("")==null)        //�������� ������ ������ ������
        {
            count++;
        }
        if (count > 30)
        {
            shakeScreen.SetActive(false);
            if (GameObject.FindGameObjectsWithTag("") != null)
                Destroy(shakeScreen);
        }

        int countB=0;
        if (GameObject.FindGameObjectsWithTag("")==null)        //QR�ڵ尡 �νĵ��� �� ������ ������
        {
            countB++;
        }
        if (count > 30)
        {
            zoomQR.SetActive(false);
            if (GameObject.FindGameObjectsWithTag("") != null)
                Destroy(zoomQR);
        }
    }
}

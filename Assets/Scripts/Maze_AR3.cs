using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Maze_AR3 : MonoBehaviour
{
    public GameObject ww;
    public GameObject sh;
    public float moveSpeed = 0.1f;
    bool move;
    Rigidbody rig;
    private void Start()
    {
        Invoke("Move", 2.1f);
        rig = GetComponent<Rigidbody>();
    }

    private void FixedUpdate() {
        if (move == true)
        {
            transform.Translate(Vector3.forward * Time.deltaTime * moveSpeed);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.name == "FinishLine")       //�̷� Ż�� ���ϸ��̼�     //��ƼŬ �߰��ϱ�
        {
            ww.GetComponent<Animator>().Play("T-Rex_Roar 1");
            sh.GetComponent<Animator>().Play("SHFinished");
            move = false;
        }
    }

    public void RotateTo(Vector3 direction) {
        transform.forward = direction;
        Vector3 localRotation = transform.localRotation.eulerAngles;
        localRotation.x = 0;
        localRotation.z = 0;
        transform.localEulerAngles = localRotation;
        Move();
    }

    void Move()
    {
        move = true;
    }
}


using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Maze_AR3 : MonoBehaviour
{
    public GameObject ww;
    public GameObject sh;
    public GameObject startLine;
    bool move;
    Rigidbody rig;
    private void Start()
    {
        Invoke("Move", 2.1f);
        rig = GetComponent<Rigidbody>();
        Invoke("StartLine", 3);
    }
    void Update()
    {
        if (move == true)
        {
            rig.MovePosition(transform.position + transform.forward * Time.deltaTime * .5f);
            //transform.Translate(Vector3.forward * Time.deltaTime * .5f);
        }
    }
    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Wall")) move = false;
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

    public void RotateTo(Vector3 direction)
    {
        transform.forward = direction;
        Move();
    }

    void Move()
    {
        move = true;
    }

    void StartLine()
    {
        startLine.SetActive(true);
    }
}


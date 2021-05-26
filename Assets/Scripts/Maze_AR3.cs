using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Maze_AR3 : MonoBehaviour
{
    public GameObject ww;
    public GameObject sh;
    public float moveSpeed = 0.1f;
    public GameObject startLine;
    public GameObject finishGate;
    public GameObject finishPosition;
    bool move;
    public bool finish;
    Rigidbody rig;
    private void Start()
    {
        Invoke("Move", 2.1f);
        rig = GetComponent<Rigidbody>();
        Invoke("StartLine", 3.5f);
        //finishZone.SetActive(false);
    }

    private void FixedUpdate() {
        if (move == true)
        {
            transform.Translate(Vector3.forward * Time.deltaTime * moveSpeed);
        }
        if(finish==true)
            transform.position = Vector3.MoveTowards(transform.position, finishPosition.transform.position, .2f * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.name == "FinishLine")       //�̷� Ż�� ���ϸ��̼�     //��ƼŬ �߰��ϱ�
        {
            ww.GetComponent<Animator>().SetTrigger("Finish");
            sh.GetComponent<Animator>().SetTrigger("Finish");
            GetComponent<Animator>().Play("MazeClear");
            //finishZone.SetActive(true);
            finishGate.GetComponent<Animator>().enabled = true;
            move = false;
            finish = true;
        }
    }

    public void RotateTo(Vector3 direction)
    {
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

    void StartLine()
    {
        startLine.GetComponent<BoxCollider>().enabled = true;
    }
}


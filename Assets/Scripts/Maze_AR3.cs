using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Maze_AR3 : MonoBehaviour
{
    public GameObject ww;
    public GameObject sh;
    public GameObject startLine;
    public GameObject finishPosition;
    bool move;
    bool finish;
    Rigidbody rig;
    private void Start()
    {
        Invoke("Move", 2.1f);
        rig = GetComponent<Rigidbody>();
        Invoke("StartLine", 3.5f);
    }
    void Update()
    {
        if (move == true)
        {
            rig.MovePosition(transform.position + transform.forward * Time.deltaTime * .5f);
            //transform.Translate(Vector3.forward * Time.deltaTime * .5f);
        }
        if(finish==true)
            transform.position = Vector3.MoveTowards(transform.position, finishTarget.transform.position, .2f * Time.deltaTime);
    }
    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Wall")) move = false;
    }
    public GameObject finishTarget;
    private void OnTriggerEnter(Collider other)
    {
        if (other.name == "FinishLine")       //�̷� Ż�� ���ϸ��̼�     //��ƼŬ �߰��ϱ�
        {
            ww.GetComponent<Animator>().SetTrigger("Finish");
            sh.GetComponent<Animator>().SetTrigger("Finish");
            GetComponent<Animator>().Play("MazeClear");
            move = false;
            finish = true;
            finishPosition.SetActive(true);            
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
        print(0);
    }

    void StartLine()
    {
        startLine.GetComponent<BoxCollider>().enabled = true;
    }
}


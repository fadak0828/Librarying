using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Maze_AR3 : MonoBehaviour
{
    public GameObject ww;
    public GameObject sh;
    public float moveSpeed = 0.1f;
    public Collider startLine;
    public GameObject finishGate;
    public GameObject finishPosition;
    public NextPageTimer nextPageUi;
    bool move;
    public bool finish;
    Rigidbody rig;

    private AudioSource audioSource;
    private void Start()
    {
        Invoke("Move", 2.1f);
        rig = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();
        GetComponent<HlMazeCardPlace>().enabled = false;
    }

    private void FixedUpdate()
    {
        if (move == true)
        {
            rig.MovePosition(transform.position + transform.forward * Time.deltaTime * moveSpeed);
        }
        if (finish == true)
        {
            transform.position = Vector3.MoveTowards(transform.position, finishPosition.transform.position, .05f * Time.deltaTime);
            if (Vector3.Distance(transform.position, finishPosition.transform.position) < 0.01f) {
                finishPosition.SetActive(true);
                finishPosition.GetComponent<AudioSource>().Play();
                finish = false;
                audioSource.Stop();
                audioSource.Play();
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.name == "FinishLine")       //�̷� Ż�� ���ϸ��̼�     //��ƼŬ �߰��ϱ�
        {
            GetComponent<HlMazeCardPlace>().enabled = false;
            ww.GetComponent<Animator>().SetTrigger("Finish");
            sh.GetComponent<Animator>().SetTrigger("Finish");
            GetComponent<Animator>().Play("MazeClear");
            //finishZone.SetActive(true);
            finishGate.GetComponent<Animator>().enabled = true;
            move = false;
            finish = true;

            nextPageUi.enabled = true;
            other.gameObject.SetActive(false);
        }

    }

    private void OnTriggerExit(Collider ohter) {
        if (ohter.gameObject.name == "StartLine") {
            startLine.isTrigger = false;
            GetComponent<HlMazeCardPlace>().enabled = true;
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
}


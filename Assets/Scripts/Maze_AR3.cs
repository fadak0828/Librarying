using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Maze_AR3 : MonoBehaviour
{
    public GameObject ww;
    public GameObject sh;
    bool move;
    Rigidbody rig;
    private void Start()
    {
        Invoke("Move", 2.1f);
        rig = GetComponent<Rigidbody>();
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
        float x = transform.eulerAngles.x;
        float y = transform.eulerAngles.y;
        float z = transform.eulerAngles.z;
        if (other.gameObject.name == "RightCard") y += 90; move = true; transform.eulerAngles = new Vector3(x, y, z);       //오른쪽 카드가 실행할 것!!
        if (other.gameObject.name == "LeftCard") y -= 90; move = true; transform.eulerAngles = new Vector3(x, y, z);         //왼쪽 카드가 실행할 것!!
        
        if (other.name == "FinishLine")       //미로 탈출 에니메이션     //파티클 추가하기
        {
            ww.GetComponent<Animator>().Play("T-Rex_Roar 1");
            sh.GetComponent<Animator>().Play("SHFinished");
            move = false;
        }
    }
    void Move()
    {
        move = true;
    }
}


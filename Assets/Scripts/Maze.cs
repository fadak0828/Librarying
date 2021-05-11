using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Maze : MonoBehaviour
{
    bool stay;
    Rigidbody rig;
    private void Start()
    {
        rig = GetComponent<Rigidbody>();
        //transform.eulerAngles = new Vector3(0, -90, 0);
    }
    void Update()
    {
        if (stay == false)
        {
            rig.MovePosition(transform.position + transform.forward * Time.deltaTime * .5f);
            //transform.Translate(Vector3.forward * Time.deltaTime * .5f);
        }
    }
    private void OnCollisionEnter(Collision other)
    {
        
        if (other.gameObject.CompareTag("Wall"))
            stay = true;
        
    }
    private void OnTriggerEnter(Collider other)
    {
        float x = transform.rotation.x;
        float y = transform.rotation.y;
        float z = transform.rotation.z;
        if (other.gameObject.name == "RightCard")
        {
            y += 90; 
            stay = false;
        }
        if (other.gameObject.name == "LeftCard")
        {
            y -= 90; 
            stay = false;
        }
        transform.eulerAngles = new Vector3(x, y, z);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnockEffect : MonoBehaviour
{
    AudioSource knockSound;
    Animator doorAnim;
    public GameObject door;
    public GameObject hand;
    public GameObject handPivot;
    Animator handAnim;
    void Start()
    {
        knockSound = GetComponent<AudioSource>();
        doorAnim = door.GetComponent<Animator>();
        handAnim = handPivot.GetComponent<Animator>();
        doorAnim.enabled = false;
        hand.SetActive(false);
    }
    private void OnTriggerEnter(Collider other)
    {
        other.GetComponent<MeshRenderer>().enabled = false;
        knockSound.Play();
        hand.SetActive(true);
        Invoke("DoorOpen", 1.7f);
    }
    void DoorOpen()
    {
        doorAnim.enabled = true;
        handAnim.enabled = false;
        knockSound.Stop();
    }
}

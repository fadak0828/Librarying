using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnockEffect : MonoBehaviour
{
    AudioSource knockSound;
    Animator doorAnim;
    public GameObject doorPivot;
    void Start()
    {
        knockSound = GetComponent<AudioSource>();
        doorAnim = doorPivot.GetComponent<Animator>();
        doorAnim.enabled = false;
    }
    private void OnTriggerEnter(Collider other)
    {
        knockSound.Play();
        doorAnim.enabled = true;
    }
}

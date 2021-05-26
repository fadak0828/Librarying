using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnockEffect : MonoBehaviour
{
    public GameObject door;
    public GameObject[] animals;

    AudioSource knockSound;
    Animator doorAnim;


    void Start()
    {
        knockSound = GetComponent<AudioSource>();
        doorAnim = door.GetComponent<Animator>();
        doorAnim.enabled = false;
    }

    public void PlayKnock() {
        knockSound.Play();
        Invoke("DoorOpen", 1.7f);
    }

    void DoorOpen()
    {
        doorAnim.enabled = true;
        knockSound.Stop();
        Invoke("AnimalsOut", 1f);
    }
    void AnimalsOut()
    {
        for (int i = 0; i < animals.Length; i++)
        {
            animals[i].SetActive(true);
        }
        door.GetComponent<AudioSource>().enabled = true;
    }
}

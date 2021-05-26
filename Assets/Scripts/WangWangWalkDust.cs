using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WangWangWalkDust : MonoBehaviour
{
    public Transform leftFoot;
    public Transform rightFoot;
    public GameObject dustPref;
    public Vector3 positionOffset;
    public float scaleOffset;
    public AudioClip walkSfxClip;
    public bool notWalking = false;
    private AudioSource audioSource;
    private void Awake() {
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null) {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
        audioSource.playOnAwake = false;
        audioSource.loop = false;
        audioSource.pitch = 1.2f;
        if (!notWalking) {
            audioSource.clip = walkSfxClip;
        }
    }

    private void CreateDust(Transform landedFootTransform) {
        Instantiate(dustPref, landedFootTransform.position + positionOffset, landedFootTransform.rotation).transform.localScale *= transform.lossyScale.magnitude * scaleOffset;
    }

    public void OnLandLeftFoot() {
        CreateDust(leftFoot);
        audioSource.Stop();
        audioSource.Play();
    }

    public void OnLandRightFoot() {
        CreateDust(rightFoot);
        audioSource.Stop();
        audioSource.Play();
    }
}

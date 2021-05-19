using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NarrationController : MonoBehaviour
{
    public AudioClip narrationClip;
    public float lastPlayTime = 0;
    private AudioSource audioSource;

    private void OnEnable() {
        InitAudioSource();     
        PlayNarration();   
    }
    private void OnDisable() {
        SaveLastPlayTime();
    }

    private void InitAudioSource() {
        audioSource = GetComponent<AudioSource>();
        audioSource.clip = narrationClip;
    }

    private void SaveLastPlayTime() {
        // 재생이 끝났으면 -1 아니면 재생된 시간을 저장
        lastPlayTime = audioSource.isPlaying ? audioSource.time : -1;
    }

    private void PlayNarration() {
        // 재생 중이었거나 처음 재생이면
        if (lastPlayTime >= 0) {
            audioSource.time = lastPlayTime;
            audioSource.Play();
        }
    }
}

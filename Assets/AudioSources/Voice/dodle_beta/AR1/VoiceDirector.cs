using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class VoiceDirector : MonoBehaviour
{
    private PlayableDirector playableDirector;
    private double playTime = 0;

    private void Awake() {
        playableDirector = GetComponent<PlayableDirector>();
    }

    private void OnEnable() {
        playableDirector.time = playTime;
        PlayVoiceScript();
    }

    private void OnDisable() {
        playTime = playableDirector.time;
    }

    public void PlayVoiceScript() {
        if (playTime < playableDirector.duration) {
            playableDirector.Play();
        } else {
            playableDirector.Pause();
        }
    }
}

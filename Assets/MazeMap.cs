using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class MazeMap : MonoBehaviour
{
    public WangWangBusAnim wangwangBusAnim;
    public MazeCardPlace[] mazeCardPlaces;
    public GameObject clearEffect;
    public bool allCorrectPlaced = false;
    private AudioSource audioSource;

    private void Start() {
        audioSource = GetComponent<AudioSource>();
    }

    private void Update() {
        UpdateMazeClearState();
    }

    private void UpdateMazeClearState() {
        if (allCorrectPlaced == false && mazeCardPlaces.All(mazeCardPlace => mazeCardPlace.isCorrect)) {
            allCorrectPlaced = true;
            OnMazeClear();
        }
    }

    private void OnMazeClear() {
        wangwangBusAnim.PlayAnim();
        Invoke("ShowFindNextUI", 10);
        Invoke("PlayVoice", 6.3f);
    }

    private void PlayVoice() {
        audioSource.Stop();
        audioSource.Play();
        clearEffect.SetActive(true);
    }

    private void ShowFindNextUI() {
        OnboardingUIManager.Instance.ShowUiByName("FindNextPage");
    }
}

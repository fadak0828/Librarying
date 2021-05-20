using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HlMazeCardPlace : CardPlacement
{
    private Maze_AR3 maze_AR3;
    public List<AudioClip> clips;
    public AudioSource audioSource;

    private void Start() {
        maze_AR3 = GetComponent<Maze_AR3>();
    }

    protected override void OnCardPlacement()
    {
        base.OnCardPlacement();
        maze_AR3.RotateTo(placedCardDirection);

        if (!audioSource.isPlaying) {
            audioSource.Stop();
            audioSource.clip = clips[Random.Range(0, clips.Count)];
            audioSource.Play();
        }
    }
}

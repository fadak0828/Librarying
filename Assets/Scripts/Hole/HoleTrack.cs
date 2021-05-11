using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class HoleTrack : MonoBehaviour
{
    public float speed;

    [ContextMenu("PlayAnim")]
    public void PlayAnim() {
        PlayableDirector director = GetComponent<PlayableDirector>();
        director.RebuildGraph(); // the graph must be created before getting the playable graph
        director.playableGraph.GetRootPlayable(0).SetSpeed(speed);
        director.Play();
    }
}

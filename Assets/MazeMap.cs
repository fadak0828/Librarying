using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class MazeMap : MonoBehaviour
{
    public WangWangBusAnim wangwangBusAnim;
    public MazeCardPlace[] mazeCardPlaces;
    public bool allCorrectPlaced = false;

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
    }
}

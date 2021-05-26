using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HlMazeCardPlace : CardPlacement
{
    private Maze_AR3 maze_AR3;

    private void Start() {
        maze_AR3 = GetComponent<Maze_AR3>();
    }

    protected override void OnCardPlacement()
    {
        base.OnCardPlacement();
        maze_AR3.RotateTo(placedCardDirection);
    }
}

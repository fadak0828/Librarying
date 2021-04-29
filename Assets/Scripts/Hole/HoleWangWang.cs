using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public enum HoleWangWangState {
    NONE,
    IDLE,
    WALK,
    CRYING,
    SHAKE_HEAD
}

public class HoleWangWang : MonoBehaviour
{
    [SerializeField]
    public HoleWangWangState state = HoleWangWangState.NONE;
    private HoleWangWangState prevState = HoleWangWangState.NONE;
    private Animator animator;

    private void Start() {
        animator = GetComponent<Animator>();
    }

    private void Update() {
        if (prevState != state) {
            animator.SetTrigger(GetStateName());
            prevState = state;
            state = HoleWangWangState.NONE;
        }
    }

    private string GetStateName() {
        switch(state) {
            case HoleWangWangState.IDLE:
                return "Idle";
            case HoleWangWangState.CRYING:
                return "Crying";
            case HoleWangWangState.WALK:
                return "Walk";
            case HoleWangWangState.SHAKE_HEAD:
                return "ShakeHead";
            default:
                return "Idle";
        }
    }
}

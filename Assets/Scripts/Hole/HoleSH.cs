using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum HoleShState {
    NONE,
    RUN,
    CROUCH,
    JUMP
}
public class HoleSH : MonoBehaviour
{
    [SerializeField]
    public HoleShState state = HoleShState.NONE;
    private HoleShState prevState = HoleShState.NONE;
    private Animator animator;

    private void Start() {
        animator = GetComponent<Animator>();
    }

    private void Update() {
        if (prevState != state) {
            animator.SetTrigger(GetStateName());
            prevState = state;
            state = HoleShState.NONE;
        }
    }

    private string GetStateName() {
        switch(state) {
            case HoleShState.RUN:
                return "Run";
            case HoleShState.JUMP:
                return "Jump";
            case HoleShState.CROUCH:
                return "Crouch";
            default:
                return "Idle";
        }
    }
}

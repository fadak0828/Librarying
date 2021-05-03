using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public enum HoleWangWangState {
    NONE,
    IDLE,
    WALK,
    CRYING,
    SHAKE_HEAD,
    JUMP,
    POWER
}

public class HoleWangWang : MonoBehaviour
{
    public Transform leftFoot;
    public Transform rightFoot;
    public GameObject dustPref;
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
            case HoleWangWangState.JUMP:
                return "Jump";
            case HoleWangWangState.POWER:
                return "Power";
            default:
                return "Idle";
        }
    }

    public void CreateDust() {
        GameObject dust = Instantiate(dustPref, transform);
        dust.transform.parent = null;
    }
}

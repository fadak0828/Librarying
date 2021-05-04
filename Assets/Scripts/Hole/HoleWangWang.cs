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
    public BridgeAnim[] bridges;

    [SerializeField]
    public HoleWangWangState state = HoleWangWangState.NONE;
    private HoleWangWangState prevState = HoleWangWangState.NONE;
    private Animator animator;
    public bool shakeBridge;

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

    private void CreateDust(Transform landedFootTransform) {
        if (shakeBridge) {
            foreach(var bridge in bridges) {
                print("bridge");
                bridge.PlayBounceAnim();
            }
        }
        Instantiate(dustPref, landedFootTransform.position, landedFootTransform.rotation);
    }

    public void OnLandLeftFoot() {
        CreateDust(leftFoot);
    }

    public void OnLandRightFoot() {
        CreateDust(rightFoot);
    }

    private void OnTriggerEnter(Collider other) {
        print(other.name);
        print(other.gameObject.name);
        if (other.gameObject.name.Contains("BridgeShakeZone")) {
            shakeBridge = true;
        }
    }

    private void OnTriggerExit(Collider other) {
        if (other.gameObject.name.Contains("BridgeShakeZone")) {
            shakeBridge = false;
        }
    }
}

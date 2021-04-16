using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Constants;

public class Dino : MonoBehaviour
{
    [Tooltip("구멍에서 탈출했는지?")]
    public bool isEscaped = false;
    public Collider holeCollider;

    private Animator animator;
    private void Awake() {
        animator = GetComponent<Animator>();
    }

    private void Update() {
        if (!isEscaped) {
            if (Physics.OverlapBox(holeCollider.transform.position, holeCollider.bounds.size / 2, holeCollider.transform.rotation)
                .FirstOrDefault(obj => obj.gameObject.name == gameObject.name) == null) {
                OnEscpae();
            }
        }
    }

    private void OnEscpae() {
        isEscaped = true;
        animator.enabled = true;
        animator.Play("DinoJumpToGround");
    }
}

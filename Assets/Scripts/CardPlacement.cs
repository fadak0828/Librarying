using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface CardExcute {
    public void ExcuteCard();
}

// 카드가 CardIndicator에 놓여져 있는지 체크
// 일정시간(public)동안 이펙트를 보여주고
// 그 시간이 지나면 카드 효과를 실행한다.
public class CardPlacement : MonoBehaviour
{
    [Tooltip("time(초) 동안 카드가 놓여있어야 효과가 실행된다")]
    public float time = 2;

    [Tooltip("효과가 여러번 실행될 수 있는지?")]
    public bool repeat = false;

    [Tooltip("이펙트")]
    public GameObject effectPref;

    [Tooltip("실행될 카드 효과")]
    public CardExcute cardExcute;

    // 카드가 인디케이터에 닿았는가?
    private bool isIn = false;

    // 카드가 실행됐었는지?
    private bool isExcuted = false;

    // 얼마동안 놓여있었는지 체크용
    private float currentPlacementTime;

    private void Update() {
        UpdateEffectAnimation();
    }

    private void FixedUpdate() {
        if (isIn) {
            currentPlacementTime += Time.deltaTime;
        } else {
            currentPlacementTime -= Time.deltaTime;
        }

        // 카드 실행 시간이 충족되었는가?
        if (currentPlacementTime >= time) {
            // 실행 안됐거나 반복 가능한 경우에만 실행
            if (!isExcuted || repeat) {
                ExcuteCard();
            } 
        }
    }

    private void UpdateEffectAnimation() {

    }

    private void ExcuteCard() {
        currentPlacementTime = 0;
        isExcuted = true;
        cardExcute?.ExcuteCard();
    }

    private void OnTriggerEnter(Collider other) {
        if (other.gameObject.layer == LayerMask.NameToLayer("CardIndicator")) {
            isIn = true;
        }
    }

    private void OnTriggerExit(Collider other) {
        if (other.gameObject.layer == LayerMask.NameToLayer("CardIndicator")) {
            currentPlacementTime = 0;
            isIn = false;
        }
    }
}

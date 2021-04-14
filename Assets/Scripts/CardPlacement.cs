using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CardExcuteBehavior : MonoBehaviour {
    public abstract void ExcuteCard();
}

// 카드가 CardIndicator에 놓여져 있는지 체크
// 일정시간(public)동안 이펙트를 보여주고
// 그 시간이 지나면 카드 효과를 실행한다.
public class CardPlacement : MonoBehaviour
{
    [Tooltip("time(초) 동안 카드가 놓여있어야 효과가 실행된다")]
    public float time = 1;

    [Tooltip("효과가 여러번 실행될 수 있는지?")]
    public bool repeat = false;

    [Tooltip("이펙트 프리팹")]
    public GameObject effectPref;
    private ParticleSystem particleEffect;

    [Tooltip("소환될 프리팹")]
    public GameObject targetjPref;

    // 카드가 인디케이터에 닿았는가?
    private bool isIn = false;

    // 카드가 실행됐었는지?
    private bool isExcuted = false;

    // 얼마동안 놓여있었는지 체크용
    public float currentPlacementTime;
    private List<GameObject> spawnList = new List<GameObject>();

    public void CleanUp() {
        spawnList?.ForEach(obj => Destroy(obj));
    }

    private void OnDisable() {
        CleanUp();
    }

    private void FixedUpdate() {
        CheckCardPlacement();
    }

    // 카드가 닿았는지 확인하는 함수
    private void CheckCardPlacement() {
        isIn = Physics.OverlapBox(transform.position, transform.lossyScale / 2, transform.rotation, LayerMask.GetMask("CardIndicator")).Length > 0;

        // 실행 안됐거나 반복 가능한 경우에만 실행
        if (!isExcuted || repeat) {
            if (isIn) {
                currentPlacementTime += Time.deltaTime;
            } else {
                currentPlacementTime = Mathf.Clamp(currentPlacementTime - Time.deltaTime, 0, float.MaxValue);
            }

            // 카드 실행 시간이 충족되었는가?
            if (currentPlacementTime >= time) {
                ExcuteCard();
            }
        } 
    }

    private void ExcuteCard() {
        GameObject effect = Instantiate(effectPref, transform);
        effect.transform.SetParent(null);
        
        currentPlacementTime = 0;
        isExcuted = true;
        spawnList.Add(Instantiate(targetjPref, transform.position, transform.rotation));
    }

    void OnDrawGizmosSelected()
    {
        // 무게 감지 범위 씬뷰에 그리기
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(transform.position, transform.lossyScale);
    }
}

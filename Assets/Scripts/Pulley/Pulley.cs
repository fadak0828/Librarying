using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pulley : MonoBehaviour
{
    public List<PulleyBasket> pulleyBasket;
    public Transform top;
    public Transform bottom;
    public float speed;

    private void Awake() {
        pulleyBasket[0].speed = speed;
        pulleyBasket[1].speed = speed;
    }

    private void FixedUpdate() {
        float topY = top.transform.position.y;
        float bottomY = bottom.transform.position.y;

        if (pulleyBasket[0].mass == pulleyBasket[1].mass) {
            // 무게가 같은 경우
            pulleyBasket[0].destinationY = (topY + bottomY) / 2;
            pulleyBasket[1].destinationY = (topY + bottomY) / 2;
        } else if (pulleyBasket[0].mass > pulleyBasket[1].mass) {
            // 0번이 더 무거운 경우
            pulleyBasket[0].destinationY = bottomY;
            pulleyBasket[1].destinationY = topY;
        } else {
            // 1번이 더 무거운 경우
            pulleyBasket[0].destinationY = topY;
            pulleyBasket[1].destinationY = bottomY;
        }
    }
}

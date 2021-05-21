using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class TextJump : MonoBehaviour
{
    public float jumpHeight = 300;
    public float duration = 0.7f;
    public AnimationCurve animationCurve;
    private RectTransform rectTransform;

    private void Awake() {
        rectTransform = GetComponent<RectTransform>();
    }

    [ContextMenu("Jump")]
    public void Jump() {
        StartCoroutine("IeJump");
    }

    private IEnumerator IeJump() {
        float time = 0;
        Vector2 size = rectTransform.sizeDelta;
        Vector2 originSize = size;
        while (time < duration) {
            time += Time.deltaTime;
            
            size.y = originSize.y + animationCurve.Evaluate(time / duration) * 300;
            rectTransform.sizeDelta = size;

            yield return new WaitForEndOfFrame();
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;


// 미로카드 놓는 자리
public class CardPlacement : MonoBehaviour
{
    public string targetTag;
    public bool isCorrect;

    public float thresholdTime = 2f;

    private Transform[] points;

    private float currentTime = 0;
    private GameObject[] targetList;
    private GameObject prevInnerObj;

    private void OnEnable() {
        points = transform.GetComponentsInChildren<Transform>().Where(t => t.gameObject.name.Contains("Point")).ToArray();
        targetList = GameObject.FindGameObjectsWithTag(targetTag);
    }

    private void Update() {
        CheckIsCorrect();
    }

    private void CheckIsCorrect() {
        if (!isCorrect) {
            GameObject currentInnerObj = GetInsideCard();

            if (currentInnerObj && currentInnerObj == prevInnerObj) {
                currentTime += Time.deltaTime;
            } else {
                currentTime = Mathf.Clamp(currentTime - Time.deltaTime, 0, float.MaxValue);
            }

            if (currentTime >= thresholdTime) {
                isCorrect = true;
                OnCardPlacement();
            }

            prevInnerObj = currentInnerObj;
        }
    }

    protected virtual void OnCardPlacement() {
        print("OnCardPlacement");
    }

    private GameObject GetInsideCard() {
        Vector2[] vertices = new Vector2[points.Length];
        
        for(int i = 0; i < points.Length; i++) {
            vertices[i] = GetScreenPoint(points[i].position);
        }

        foreach (GameObject target in targetList) {
            if (PolyUtil.IsPointInPolygon(GetScreenPoint(target.transform.position), vertices)) 
            return target;
        }

        return null;
    }

    private Vector3 GetScreenPoint(Vector3 point) {
        return Camera.main.WorldToScreenPoint(point);
    }

    private Vector3? GetCardDirection(Vector3 cardForward) {
        float detectZone = 30;

        if (Vector3.Angle(cardForward, transform.forward) <= detectZone) {
            return transform.forward;
        } else if (Vector3.Angle(cardForward, transform.right) <= detectZone) {
            return transform.right;
        } else if (Vector3.Angle(cardForward, -transform.forward) <= detectZone) {
            return -transform.forward;
        } else if (Vector3.Angle(cardForward, -transform.right) <= detectZone) {
            return -transform.right;
        }

        return null;
    }
}

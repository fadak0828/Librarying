using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;


// 미로카드 놓는 자리
public class MazeCardPlace : MonoBehaviour
{
    public bool isCorrect;
    public string answerName;
    public Transform[] points;
    private GameObject innerMazeCard;
    private GameObject innerMazeCardModel;

    private GameObject cardPivot;
    public GameObject[] cards;

    public GameObject includeCard;

    public GameObject effectPref;

    private AudioSource audioSource;

    private void Awake() {
        cardPivot = gameObject;
        // cards = GameObject.FindGameObjectsWithTag("MazeCard");
        audioSource = GetComponent<AudioSource>();
    }

    private void Update() {
        if (isCorrect) return;

        includeCard = GetInsideCard();

        if (includeCard != null) {
            innerMazeCard = includeCard;
            innerMazeCardModel = innerMazeCard.transform.GetChild(0).gameObject;
            
            Vector3? side = GetCardDirection(innerMazeCard.transform.forward);
            if (side != null) {
                innerMazeCardModel.transform.parent = cardPivot.transform;
                innerMazeCardModel.transform.localPosition = Vector3.zero;
                innerMazeCardModel.transform.localRotation = Quaternion.identity;
                // innerMazeCardModel.transform.up = transform.up;
                innerMazeCardModel.transform.forward = (Vector3)side;
            }
            isCorrect = side == transform.forward && includeCard.name.Contains(answerName);

            if (!includeCard.name.Contains(answerName) && !audioSource.isPlaying) {
                audioSource.Stop();
                audioSource.Play();
            }

            if (!isCorrect) {
                innerMazeCardModel.transform.parent = innerMazeCard.transform;
            } else {
                GameObject effect = Instantiate(effectPref, transform);
                effect.transform.localScale = Vector3.one * 15;
                innerMazeCardModel.transform.localPosition = Vector3.zero;
                innerMazeCardModel.transform.localRotation = Quaternion.identity;
            }
        } else {
            if (innerMazeCard != null) {
                innerMazeCardModel.transform.parent = innerMazeCard.transform;
                innerMazeCardModel.transform.localPosition = Vector3.zero;
                innerMazeCardModel.transform.localRotation = Quaternion.identity;

                innerMazeCard = null;
                innerMazeCardModel = null;
            }
            isCorrect = false;
        }
    }

    private GameObject GetInsideCard() {
        Vector2[] vertices = new Vector2[points.Length];
        
        for(int i = 0; i < points.Length; i++) {
            vertices[i] = GetScreenPoint(points[i].position);
        }

        foreach (GameObject card in cards) {
            if (PolyUtil.IsPointInPolygon(GetScreenPoint(card.transform.position), vertices)) 
            return card;
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

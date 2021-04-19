using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;


// 미로카드 놓는 자리
public class MazeCardPlace : MonoBehaviour
{
    public Collider m_collider;
    public bool isCorrect;
    public string answerName;
    public List<GameObject> models;
    private GameObject innerMazeCard;
    private GameObject temp;

    private void Start() {
        models = new List<GameObject>();
        foreach(Transform child in transform) {
            if (child != transform) models.Add(child.gameObject);
        }
    }

    private void Update() {
        temp = Physics.OverlapBox(transform.position, m_collider.bounds.size * 0.5f, transform.rotation, LayerMask.GetMask("MazeCardIndicator"))
                            .Select(c => c.gameObject)
                            .FirstOrDefault(mci => mci != null);

        if (temp != null) {
            innerMazeCard = temp;

            
            Vector3? side = GetCardDirection(innerMazeCard.transform.forward);
            if (side != null) {
                models.ForEach(m => {
                    m.SetActive(temp.name.Contains(m.name));
                    m.transform.forward = (Vector3)side;
                });
            }
            isCorrect = side == transform.forward && temp.name.Contains(answerName);
        } else {
            if (innerMazeCard != null) {
                innerMazeCard = null;
            }
            models.ToList().ForEach(m => m.SetActive(false));
            isCorrect = false;
        }
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

    public static void DrawCube(Vector3 position, Quaternion rotation, Vector3 scale)
    {
        Matrix4x4 cubeTransform = Matrix4x4.TRS(position, rotation, scale);
        Matrix4x4 oldGizmosMatrix = Gizmos.matrix;

        Gizmos.matrix *= cubeTransform;
        Gizmos.color = new Color(1, 0, 0, 0.2f);

        Gizmos.DrawCube(Vector3.zero, Vector3.one);

        Gizmos.matrix = oldGizmosMatrix;
    }

    private void OnDrawGizmos() {
        DrawCube(transform.position, transform.rotation, m_collider.bounds.size);
    }
}

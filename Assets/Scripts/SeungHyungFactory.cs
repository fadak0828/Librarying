using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Constants;

public class SeungHyungFactory : MonoBehaviour
{
    public Transform[] seungHyungPositions;
    public GameObject seungHyungPref;
    public float interval = 1;

    private void Start() {
        FindSeungHyungPositions();
        StartCoroutine(IeCreateSeungHyung());
    }

    private IEnumerator IeCreateSeungHyung() {
        for (int i = 0; i < seungHyungPositions.Length; i++)
        {
            Vector3 pos = seungHyungPositions[i].position;
            pos.y = 0.03f;

            GameObject sh = Instantiate(seungHyungPref);
            sh.transform.position = pos;
            sh.transform.parent = seungHyungPositions[i];

            yield return new WaitForSeconds(interval);
        }
    }

    private void FindSeungHyungPositions() {
        seungHyungPositions = GameObject.FindGameObjectsWithTag(Tag.SEUNG_HYUNG_POS)
                                .Select(obj => obj.transform)
                                .ToArray();
    }
}

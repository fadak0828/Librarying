using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeungHyungFactory : MonoBehaviour
{
    public GameObject seungHyungPref;
    public float createInterval = 2;
    void Start()
    {
        StartCoroutine(IeCreateSeungHyung());
    }

    private IEnumerator IeCreateSeungHyung() {
        float coolTime = 0;
        while(true) {
            coolTime += Time.deltaTime;

            if (coolTime >= createInterval) {
                coolTime = 0;
                GameObject seungHyung = Instantiate(seungHyungPref);
                seungHyung.transform.position = transform.position;
                seungHyung.transform.rotation = transform.rotation;
            }

            yield return new WaitForEndOfFrame();            
        }
    }
}

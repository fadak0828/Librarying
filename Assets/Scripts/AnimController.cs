using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimController : MonoBehaviour
{
    public GameObject wangwang;
    public GameObject sh;
    Animator wwAnim;
    Animator shAnim;
    public Page page;
    public enum Page
    {
        AR1,
        AR2,
        AR3,
        AR4,
        AR5,
        AR6,
        AR7,
        AR8,
        AR9,
    }
    void Start()
    {
        if (wangwang != null && wangwang.GetComponent<Animator>() != null)
            wwAnim = wangwang.GetComponent<Animator>();
        if (sh != null && sh.GetComponent<Animator>() != null)
            shAnim = sh.GetComponent<Animator>();
    }
    void Update()
    {
        switch (page)
        {
            case Page.AR1:
                One(); break;
            case Page.AR2:
                StartCoroutine(Two()); break;
            case Page.AR3:
                Three(); break;
            case Page.AR4:
                Four(); break;
            case Page.AR6:
                Six(); break;
            case Page.AR7:
                StartCoroutine( Seven()); break;
        }
    }

    float count;
    void One()
    {
        shAnim.SetTrigger("Waving");
        count += Time.deltaTime;
        if (count > 4)
        {
            wwAnim.SetTrigger("Roar");
            count = 0;
        }
    }
    bool check;
    IEnumerator Two()
    {
        if (check == false)
        {
            wwAnim.SetTrigger("Walk");
            shAnim.SetTrigger("Run");
            check = true;
        }
        yield return new WaitForSeconds(2.53f);
        //wangwang.transform.position += transform.TransformPoint(Vector3.back) * Time.deltaTime;
        wangwang.transform.Translate(Vector3.forward * Time.deltaTime*0.4f);
    }
    void Three()
    {

    }
    void Four()
    {
        wwAnim.SetTrigger("Questioning");
        shAnim.SetTrigger("Point");
    }
    void Six()
    {
        wwAnim.SetTrigger("LookInto");
    }
    IEnumerator Seven()
    {
        wwAnim.SetTrigger("Power");
        yield return new WaitForSeconds(2.1f);
        sh.transform.GetChild(0).gameObject.SetActive(true);
        yield return new WaitForSeconds(6.5f);
        sh.transform.GetChild(1).gameObject.SetActive(true);
    }
}

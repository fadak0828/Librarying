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
    public AudioSource wwRoar;
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
            //    case Page.AR6:
            //        Six(); break;
            case Page.AR7:
                Seven(); break;
            case Page.AR8:
                StartCoroutine(Eight()); break;
        }
    }

    float count;
    void One()
    {
        shAnim.SetTrigger("Waving");
        wwAnim.SetTrigger("Roar");
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
        yield return new WaitForSeconds(2.1f);
        wangwang.transform.Translate(Vector3.forward * Time.deltaTime * 0.04f);
    }
    void Three()
    {
        wwAnim.SetTrigger("Walk");
        shAnim.SetTrigger("Walk");
    }
    void Four()
    {
        wwAnim.SetTrigger("Questioning");
        shAnim.SetTrigger("Point");
    }
    void Seven()
    {
        wwAnim.SetTrigger("LookInto");
    }
    IEnumerator Eight()
    {
        //sh.transform.GetChild(1).gameObject.SetActive(false);
        wwAnim.SetTrigger("Power");
        yield return new WaitForSeconds(4.1f);
        //sh.transform.GetChild(0).gameObject.SetActive(true);
        sh.transform.GetChild(0).GetComponent<AudioSource>().enabled = true;
        yield return new WaitForSeconds(4.5f);
        sh.transform.GetChild(1).gameObject.SetActive(true);
        yield return new WaitForSeconds(.5f);
        sh.transform.GetChild(1).GetComponent<AudioSource>().enabled = true;
    }
}

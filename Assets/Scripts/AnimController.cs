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
        wwAnim = wangwang.GetComponent<Animator>();
        shAnim = sh.GetComponent<Animator>();
    }
    void Update()
    {
        switch (page)
        {
            case Page.AR1:
                One(); break;
            case Page.AR2:
                Two(); break;
            case Page.AR3:
                Three(); break;
            case Page.AR7:
                StartCoroutine(Seven());break;
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
    void Two()
    {
        wwAnim.SetTrigger("Walk");
        shAnim.SetTrigger("Run");
        Destroy(wangwang,9.1f);
        Destroy(sh,9.1f);
    }
    void Three()
    {

    }
    IEnumerator Seven()
    {
        wwAnim.SetTrigger("Power");
        yield return new WaitForSeconds(6f);
        wwAnim.SetTrigger("Power2");
        yield return new WaitForSeconds(2f);
        transform.GetChild(1).gameObject.SetActive(false);
        transform.GetChild(2).gameObject.SetActive(true);        
    }
}

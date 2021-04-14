using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeungHyungFactory : CardExcuteBehavior
{
    public GameObject seungHyungPref;

    public override void ExcuteCard() {
        GameObject seungHyung = Instantiate(seungHyungPref);
        seungHyung.transform.position = transform.position;
        seungHyung.transform.rotation = transform.rotation;
    } 
}

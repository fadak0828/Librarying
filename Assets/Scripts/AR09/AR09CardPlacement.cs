using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AR09CardPlacement : CardPlacement
{
    private KnockEffect knock;

    protected override void OnEnable() {
        base.OnEnable();
        knock = GetComponent<KnockEffect>();
    }

    protected override void OnCardPlacement()
    {
        base.OnCardPlacement();
        print("KnockCard Placed");
        knock.PlayKnock();
    }
}

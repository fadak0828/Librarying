using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AR09CardPlacement : CardPlacement
{
    private KnockEffect knock;

    private void OnEnable() {
        knock = GetComponent<KnockEffect>();
    }

    protected override void OnCardPlacement()
    {
        base.OnCardPlacement();
        knock.PlayKnock();
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoleCardPlacement : CardPlacement
{
    public HoleTrack clearAnimTrack;

    protected override void OnCardPlacement()
    {
        base.OnCardPlacement();
        clearAnimTrack.PlayAnim();
    }
}

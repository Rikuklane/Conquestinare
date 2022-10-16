using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReceiveUnitsTurn : State
{
    public override IEnumerator Start()
    {
        // TODO get a selection of 3 cards
        return base.Start();
    }

    public override IEnumerator Action()
    {
        // TODO select 1 card (move to MarketTurn)
        return base.Action();
    }
}

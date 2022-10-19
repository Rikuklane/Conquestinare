using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleTurn : State
{
    public override IEnumerator Start()
    {
        // TODO now only see the map and be able to attack
        return base.Start();
    }

    public override IEnumerator Action()
    {
        // TODO attack from one territory to another
        return base.Action();
    }

    public override IEnumerator End()
    {
        // TODO press next phase button (ReorganizeTurn)
        return base.End();
    }
}

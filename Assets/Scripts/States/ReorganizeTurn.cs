using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReorganizeTurn : State
{
    public override IEnumerator Start()
    {
        // TODO only interact with your own territories
        return base.Start();
    }

    public override IEnumerator Action()
    {
        // TODO move troops from one territory to another one
        return base.Action();
    }

    public override IEnumerator End()
    {
        // TODO press end turn button (Next player turn)
        return base.End();
    }
}

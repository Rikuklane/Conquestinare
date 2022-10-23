using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CardStates
{
    public abstract class AbstractCardState
    {
        public virtual IEnumerator CardOnClick(UnitCardPresenter card)
        {
            yield break;
        }

        public virtual IEnumerator NextState(UnitCardPresenter card)
        {
            yield break;
        }
    }
}

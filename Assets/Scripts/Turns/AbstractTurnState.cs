using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Turns
{
    public abstract class AbstractTurnState
    {
        public virtual IEnumerator EnterState(TurnManager turnManager, Player player)
        {
            yield break;
        }
    
        public virtual IEnumerator Action(TurnManager turnManager, Player player)
        {
            yield break;
        }

        public virtual IEnumerator EndState(TurnManager turnManager, Player player)
        {
            yield break;
        }
    }
}

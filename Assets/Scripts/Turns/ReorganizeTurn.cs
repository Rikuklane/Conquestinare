using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Turns
{
    public class ReorganizeTurn : AbstractTurnState
    {
        public override IEnumerator EnterState(TurnManager turnManager, Player player)
        {
            // TODO only interact with your own territories
            EndState(turnManager, player);
            return base.EnterState(turnManager, player);
        }

        public override IEnumerator EndState(TurnManager turnManager, Player player)
        {
            // TODO press end turn button (Next player turn)
            turnManager.SwitchPlayerTurn();
            turnManager.SwitchTurnState(turnManager.PlayerStartTurn);
            return base.EndState(turnManager, player);
        }
    }
}

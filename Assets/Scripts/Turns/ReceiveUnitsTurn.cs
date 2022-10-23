using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Turns
{
    public class ReceiveUnitsTurn : AbstractTurnState
    {
    
        public override IEnumerator EnterState(TurnManager turnManager, Player player)
        {
            Debug.Log("Start ReceiveUnitsTurn for: " + player.Name);
            Events.DisplayUnitSelection();
            return base.EnterState(turnManager, player);
        }

        public override IEnumerator Action(TurnManager turnManager, Player player)
        {
            // TODO select 1 card (move to MarketTurn)
            return base.Action(turnManager, player);
        }

        public override IEnumerator EndState(TurnManager turnManager, Player player)
        {
            turnManager.SwitchTurnState(turnManager.MarketTurn);
            return base.Action(turnManager, player);
        }
    }
}


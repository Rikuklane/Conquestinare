using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Turns
{
    public class BattleTurn : AbstractTurnState
    {
        public override IEnumerator EnterState(TurnManager turnManager, Player player)
        {
            Debug.Log("Start BattleTurn for: " + player.Name);
            // TODO now only see the map and be able to attack
            return base.EnterState(turnManager, player);
        }

        public override IEnumerator Action(TurnManager turnManager, Player player)
        {
            // TODO attack from one territory to another
            return base.Action(turnManager, player);
        }

        public override IEnumerator EndState(TurnManager turnManager, Player player)
        {
            // TODO press next phase button
            turnManager.SwitchTurnState(turnManager.ReorganizeTurn);
            return base.EndState(turnManager, player);
        }
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Turns
{
    public class ReorganizeTurn : AbstractTurnState
    {
        public override IEnumerator EnterState(TurnManager turnManager, Player player)
        {
            AttackGUI.instance.ChangeButtonClickAttack(false);

            // TODO only interact with your own territories
            //EndState(turnManager, player);
            AttackLogic.Instance.isReorganizeTurn = true;
            return base.EnterState(turnManager, player);
        }

        public override IEnumerator EndState(TurnManager turnManager, Player player)
        {
            // TODO press end turn button (Next player turn)
            AttackLogic.Instance.isReorganizeTriggered = false;
            AttackLogic.Instance.isReorganizeTurn = false;
            AttackLogic.Instance.DeselectAll();
            Events.NextPlayer();
            return base.EndState(turnManager, player);
        }
    }
}

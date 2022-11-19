using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Turns
{
    public class ReorganizeTurn : AbstractTurnState
    {
        public override IEnumerator EnterState(TurnManager turnManager, Player player)
        {
            GUI.instance.ChangeButtonClickAttack(false);

            // TODO only interact with your own territories
            //EndState(turnManager, player);
            AttackLogic.instance.isReorganizeTurn = true;
            return base.EnterState(turnManager, player);
        }

        public override IEnumerator EndState(TurnManager turnManager, Player player)
        {
            // TODO press end turn button (Next player turn)
            AttackLogic.instance.isReorganizeTurn = false;
            GUI.instance.TerritoryHoverPanel.SetActive(false);
            turnManager.SwitchPlayerTurn();
            turnManager.SwitchTurnState(turnManager.PlayerStartTurn);
            return base.EndState(turnManager, player);
        }
    }
}

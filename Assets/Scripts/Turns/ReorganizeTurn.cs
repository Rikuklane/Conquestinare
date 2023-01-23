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
            if (player.isNpc)
            {
                NpcBehaviour.Instance.ReOrganizeTurnActions();
            }
            return base.EnterState(turnManager, player);
        }

        public override IEnumerator EndState(TurnManager turnManager, Player player)
        {
            // TODO press end turn button (Next player turn)
            AttackLogic.Instance.DeselectAll();
            AttackLogic.Instance.isReorganizeTriggered = false;
            AttackLogic.Instance.isReorganizeTurn = false;
            Events.NextPlayer();
            return base.EndState(turnManager, player);
        }
        
        public override string ToString()
        {
            return "Reorganize your troops";
        }
    }
}

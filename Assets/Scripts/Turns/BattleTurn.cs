using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Turns
{
    public class BattleTurn : AbstractTurnState
    {
        public override IEnumerator EnterState(TurnManager turnManager, Player player)
        {
            AttackGUI.instance.ChangeButtonClickAttack(true);
            // TODO now only see the map and be able to attack
            AttackLogic.Instance.canHover = true;
            TerritoryManager.instance.gameObject.SetActive(true);
            return base.EnterState(turnManager, player);
        }

        public override IEnumerator EndState(TurnManager turnManager, Player player)
        {
            // TODO press next phase button
            // temporary bug fix
            AttackGUI.instance.attackButton.gameObject.SetActive(false);
            turnManager.SwitchTurnState(turnManager.ReorganizeTurn);
            return base.EndState(turnManager, player);
        }
    }

}

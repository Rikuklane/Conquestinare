using System.Collections;
using UnityEngine;

namespace Turns
{
    public class PlaceUnitsTurn: AbstractTurnState
    {
        public override IEnumerator EnterState(TurnManager turnManager, Player player)
        {
            // TODO see your cards & the map
            AttackLogic.Instance.territoryManager.SetActive(true);
            AttackLogic.Instance.isPlacementTurn = true;
            AttackLogic.Instance.canHover = true;
            return base.EnterState(turnManager, player);
        }

        public override IEnumerator EndState(TurnManager turnManager, Player player)
        {
            // TODO press next phase button
            AttackLogic.Instance.territoryManager.SetActive(false);
            turnManager.SwitchTurnState(turnManager.BattleTurn);
            return base.EndState(turnManager, player);
        }
    }
}
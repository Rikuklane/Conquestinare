using System.Collections;
using UnityEngine;

namespace Turns
{
    public class PlaceUnitsTurn: AbstractTurnState
    {
        public override IEnumerator EnterState(TurnManager turnManager, Player player)
        {
            // TODO see your cards & the map
            return base.EnterState(turnManager, player);
        }

        public override IEnumerator Action(TurnManager turnManager, Player player)
        {
            // TODO place cards on territories
            return base.Action(turnManager, player);
        }

        public override IEnumerator EndState(TurnManager turnManager, Player player)
        {
            // TODO press next phase button
            turnManager.SwitchTurnState(turnManager.BattleTurn);
            return base.EndState(turnManager, player);
        }
    }
}
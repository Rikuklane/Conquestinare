using System.Collections;
using UnityEngine;

namespace Turns
{
    public class MarketTurn: AbstractTurnState
    {
        public override IEnumerator EnterState(TurnManager turnManager, Player player)
        {
            // TODO open market place
            EndState(turnManager, player);
            return base.EnterState(turnManager, player);
        }

        public override IEnumerator EndState(TurnManager turnManager, Player player)
        {
            // TODO trigger on button press
            turnManager.SwitchTurnState(turnManager.PlaceUnitsTurn);
            return base.EndState(turnManager, player);
        }
    }
}
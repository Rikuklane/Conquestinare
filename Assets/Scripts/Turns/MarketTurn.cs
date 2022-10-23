﻿using System.Collections;
using UnityEngine;

namespace Turns
{
    public class MarketTurn: AbstractTurnState
    {
        public override IEnumerator EnterState(TurnManager turnManager, Player player)
        {
            Debug.Log("Start MarketTurn for: " + player.Name);
            // TODO open market place
            return base.EnterState(turnManager, player);
        }

        public override IEnumerator Action(TurnManager turnManager, Player player)
        {
            // TODO be able to buy stuff
            return base.Action(turnManager, player);
        }

        public override IEnumerator EndState(TurnManager turnManager, Player player)
        {
            // TODO trigger on button press
            turnManager.SwitchTurnState(turnManager.PlaceUnitsTurn);
            return base.EndState(turnManager, player);
        }
    }
}
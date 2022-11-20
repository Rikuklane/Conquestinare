using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Turns
{
    public class PlayerStartTurn : AbstractTurnState
    {
    
        public override IEnumerator EnterState(TurnManager turnManager, Player player)
        {
            CardHand.Instance.LoadHand(player);
            // TODO show Player start turn smth
            Debug.Log("Player prestige:" + player.GetPrestige());
            Events.SetGold(player, Events.RequestGold(player) + player.GetPrestige());
            return EndState(turnManager, player);
        }

        public override IEnumerator EndState(TurnManager turnManager, Player player)
        {
            turnManager.SwitchTurnState(turnManager.ReceiveUnitsTurn);
            return base.EndState(turnManager, player);
        }
    }
}

using System.Collections;
using UnityEngine;

namespace Turns
{
    public class MarketTurn: AbstractTurnState
    {
        public override IEnumerator EnterState(TurnManager turnManager, Player player)
        {
            AttackLogic.Instance.canHover = false;
            Events.DisplayMarketSelection(10);
            if (player.isNpc)
            {
                NpcBehaviour.Instance.MarketTurnActions();
            }
            return base.EnterState(turnManager, player);
        }

        public override IEnumerator EndState(TurnManager turnManager, Player player)
        {
            CardSelector.Instance.SetActive(false);
            turnManager.SwitchTurnState(turnManager.BattleTurn);
            return base.EndState(turnManager, player);
        }
        
        public override string ToString()
        {
            return "Marketplace";
        }
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Turns
{
    public class PlayerStartTurn : AbstractTurnState
    {
        public override IEnumerator EnterState(TurnManager turnManager, Player player)
        {
            CardHand.Instance.HideCurrentHand();
            AttackLogic.Instance.canHover = false;
            turnManager.nextTurnButton.gameObject.SetActive(false);
            FadeCanvasGroup.Instance.ActivateStartScreenWithFade();
            return EnterState(turnManager, player);
        }

        public override IEnumerator EndState(TurnManager turnManager, Player player)
        {
            Debug.Log("Player prestige:" + player.GetPrestige());
            Events.SetGold(player, Events.RequestGold(player) + player.GetPrestige());
            turnManager.SwitchTurnState(turnManager.ReceiveUnitsTurn);
            return base.EndState(turnManager, player);
        }
    }
}

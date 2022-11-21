using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Turns
{
    public class ReceiveUnitsTurn : AbstractTurnState
    {
    
        public override IEnumerator EnterState(TurnManager turnManager, Player player)
        {
            TurnManager.Instance.goldGainText.gameObject.SetActive(true);
            AttackLogic.Instance.canHover = false;
            SetNextButtonActive(turnManager, false);
            Events.DisplayUnitSelection(3);
            return base.EnterState(turnManager, player);
        }

        public override IEnumerator EndState(TurnManager turnManager, Player player)
        {
            TurnManager.Instance.goldGainText.gameObject.SetActive(false);
            CardSelector.Instance.SetActive(false);
            SetNextButtonActive(turnManager, true);
            turnManager.SwitchTurnState(turnManager.MarketTurn);
            return base.EndState(turnManager, player);
        }

        private void SetNextButtonActive(TurnManager turnManager, bool value)
        {
            turnManager.nextTurnButton.gameObject.SetActive(value);
        }
    }
}


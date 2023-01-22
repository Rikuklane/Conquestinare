using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Turns
{
    public class PlayerStartTurn : AbstractTurnState
    {
        public override IEnumerator EnterState(TurnManager turnManager, Player player)
        {
            AudioController.Instance.startTurn.Play();
            AudioController.Instance.coin.Play();
            TurnManager.Instance.goldGainText.gameObject.SetActive(true);
            LeanTween.moveLocalY(TurnManager.Instance.goldGainText.gameObject, -45, 0.8f);
            CardHand.Instance.HideCurrentHand();
            AttackLogic.Instance.canHover = false;
            turnManager.nextTurnButton.gameObject.SetActive(false);
            AttackGUI.instance.GetComponent<FadeCanvasGroup>().ActivateStartScreenWithFade();
            if (player.isNpc)
            {
                BlockingImage.Instance.ActivateBlockingImage(true);
                NpcBehaviour.Instance.PlayerStartTurnActions();
            }
            else
            {
                BlockingImage.Instance.ActivateBlockingImage(false);
            }
            return base.EnterState(turnManager, player);
        }

        public override IEnumerator EndState(TurnManager turnManager, Player player)
        {
            LeanTween.moveLocalY(TurnManager.Instance.goldGainText.gameObject, 0, 0.2f).setOnComplete(() => { TurnManager.Instance.goldGainText.gameObject.SetActive(false); });
            Debug.Log("Player prestige:" + player.GetPrestige());
            Events.SetGold(player, Events.RequestGold(player) + player.GetPrestige());
            turnManager.SwitchTurnState(turnManager.ReceiveUnitsTurn);
            return base.EndState(turnManager, player);
        }

        IEnumerator MoveGoldGain()
        {
            yield return null;
        }
    }
}

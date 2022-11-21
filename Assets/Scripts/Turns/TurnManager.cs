using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Turns
{
    public class TurnManager: MonoBehaviour
    {
        public Button nextTurnButton;
        public TextMeshProUGUI playerNameText;
        public TextMeshProUGUI turnNameText;
        public TextMeshProUGUI goldAmountText;
        public Image playerColorImage;
        public TextMeshProUGUI goldGainText;

        public static TurnManager Instance;
        public readonly PlayerStartTurn PlayerStartTurn = new();
        public readonly ReceiveUnitsTurn ReceiveUnitsTurn = new();
        public readonly MarketTurn MarketTurn = new();
        public readonly PlaceUnitsTurn PlaceUnitsTurn = new();
        public readonly BattleTurn BattleTurn = new();
        public readonly ReorganizeTurn ReorganizeTurn = new();
        private AbstractTurnState _currentState;
        
        public Player[] Players = { new("PlayerMustafa"), new("xxGamerBoyX")};
        private int _currentPlayerIndex;

        private void Awake()
        {
            Instance = this;
            Events.OnRequestPlayer += GetCurrentPlayer;
            Events.OnRequestGold += GetPlayerGold;
            Events.OnSetGold += SetPlayerGold;
            Events.OnNextPlayerStartTurn += SetNextPlayerTurn;
            nextTurnButton.onClick.AddListener(TriggerEndState);
        }
        
        private void Start()
        {
            foreach (var player in Players)
            {
                SetPlayerGold(player, 2);
            }
            playerColorImage.GetComponent<Image>().color = GetCurrentPlayer().color;

            TerritoryManager.instance.RandomShuffleTerritories(Players);
            CardHand.Instance.CreateCardHands(Players);
            SwitchTurnState(PlayerStartTurn);
            UpdatePlayerNameAndGold();
            goldGainText.text = "+" + GetCurrentPlayer().GetPrestige();
        }

        private void OnDestroy()
        {
            Events.OnRequestPlayer -= GetCurrentPlayer;
            Events.OnRequestGold -= GetPlayerGold;
            Events.OnSetGold -= SetPlayerGold;
        }

        private Player GetCurrentPlayer()
        {
            return Players[_currentPlayerIndex];
        }

        private void SetNextPlayerTurn()
        {
            _currentPlayerIndex++;
            if (_currentPlayerIndex >= Players.Length)
            {
                _currentPlayerIndex = 0;
            }
            for (int i = 0; i < Players.Length; i++)
            {
                if (GetCurrentPlayer().isAlive) break;
            }
            playerColorImage.GetComponent<Image>().color = GetCurrentPlayer().color;
            SwitchTurnState(PlayerStartTurn);
            UpdatePlayerNameAndGold();
            goldGainText.text = "+" + GetCurrentPlayer().GetPrestige();
        }

        private int GetPlayerGold(Player player)
        {
            return player.gold;
        }
    
        private void SetPlayerGold(Player player, int gold)
        {
            player.gold = gold;
            // Changing the gold amount seen on screen if the change was on the current player
            goldAmountText.text = GetCurrentPlayer().gold.ToString();
        }

        public void SwitchTurnState(AbstractTurnState state)
        {
            _currentState = state;
            StartCoroutine(_currentState.EnterState(this, GetCurrentPlayer()));
            turnNameText.text = _currentState.ToString();
        }

        public void TriggerEndState()
        {
            StartCoroutine(_currentState.EndState(this, GetCurrentPlayer()));
        }

        private void UpdatePlayerNameAndGold()
        {
            playerNameText.text = GetCurrentPlayer().Name;
            goldAmountText.text = GetCurrentPlayer().gold.ToString();
        }
    }
}

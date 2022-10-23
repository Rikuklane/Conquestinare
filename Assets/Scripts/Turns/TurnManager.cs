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
        
        public static TurnManager Instance;
        public readonly PlayerStartTurn PlayerStartTurn = new();
        public readonly ReceiveUnitsTurn ReceiveUnitsTurn = new();
        public readonly MarketTurn MarketTurn = new();
        public readonly PlaceUnitsTurn PlaceUnitsTurn = new();
        public readonly BattleTurn BattleTurn = new();
        public readonly ReorganizeTurn ReorganizeTurn = new();
        
        private AbstractTurnState _currentState;
        private Player _currentPlayer;
        private Player[] _players = { new("Player 1"), new("Player 2"), new("Player 3") };

        private void Awake()
        {
            Instance = this;
            Events.OnRequestPlayer += GetPlayer;
            nextTurnButton.onClick.AddListener(TriggerEndState);
        }

        private void OnDestroy()
        {
            Events.OnRequestPlayer -= GetPlayer;
        }

        private void Start()
        {
            _currentPlayer = _players[0];
            SwitchTurnState(PlayerStartTurn);
            playerNameText.text = _currentPlayer.Name;
        }

        public void SwitchTurnState(AbstractTurnState state)
        {
            _currentState = state;
            StartCoroutine(_currentState.EnterState(this, _currentPlayer));
            turnNameText.text = _currentState.ToString();
        }

        public void SwitchPlayerTurn()
        {
            var nextIndex = Array.IndexOf(_players, _currentPlayer) + 1;
            if (nextIndex == _players.Length)
            {
                nextIndex = 0;
            }
            _currentPlayer = _players[nextIndex];
            playerNameText.text = _currentPlayer.Name;
        }

        private Player GetPlayer()
        {
            return _currentPlayer;
        }

        public void TriggerEndState()
        {
            StartCoroutine(_currentState.EndState(this, _currentPlayer));
        }
    }
}

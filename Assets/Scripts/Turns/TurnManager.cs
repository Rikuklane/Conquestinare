using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Turns
{
    public class TurnManager: MonoBehaviour
    {
        public Button nextTurnButton;
        
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
            nextTurnButton.onClick.AddListener(TriggerEndState);
        }

        private void Start()
        {
            _currentPlayer = _players[0];
            SwitchTurnState(PlayerStartTurn);
        }

        public void SwitchTurnState(AbstractTurnState state)
        {
            _currentState = state;
            StartCoroutine(_currentState.EnterState(this, _currentPlayer));
        }

        public void SwitchPlayerTurn()
        {
            var nextIndex = Array.IndexOf(_players, _currentPlayer) + 1;
            if (nextIndex == _players.Length)
            {
                nextIndex = 0;
            }
            _currentPlayer = _players[nextIndex];
        }

        public void TriggerEndState()
        {
            StartCoroutine(_currentState.EndState(this, _currentPlayer));
        }
    }
}

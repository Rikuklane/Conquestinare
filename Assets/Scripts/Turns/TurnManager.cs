using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Turns
{
    public abstract class TurnManager: MonoBehaviour
    {
        private AbstractTurnState _currentState;
        private Player _currentPlayer;
        private Player[] _players = { new("Player 1"), new("Player 2"), new("Player 3") };
        public readonly PlayerStartTurn PlayerStartTurn = new();
        public readonly ReceiveUnitsTurn ReceiveUnitsTurn = new();
        public readonly MarketTurn MarketTurn = new();
        public readonly PlaceUnitsTurn PlaceUnitsTurn = new();
        public readonly BattleTurn BattleTurn = new();
        public readonly ReorganizeTurn ReorganizeTurn = new();

        private void Start()
        {
            SwitchTurnState(PlayerStartTurn);
        }

        public void SwitchTurnState(AbstractTurnState state)
        {
            _currentState = state;
            StartCoroutine(_currentState.EnterState(this, _currentPlayer));
        }

        public void SwitchPlayerTurn()
        {
            var index = Array.IndexOf(_players, _currentPlayer);
            if (index + 1 == _players.Length)
            {
                index = 0;
            }
            _currentPlayer = _players[index];
        }
    }
}

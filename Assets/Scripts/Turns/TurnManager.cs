using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Random = System.Random;

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
        public TextMeshProUGUI goldLossText;
        public GameObject settingsPanel;

        public static TurnManager Instance;
        public readonly PlayerStartTurn PlayerStartTurn = new();
        public readonly ReceiveUnitsTurn ReceiveUnitsTurn = new();
        public readonly MarketTurn MarketTurn = new();
        public readonly BattleTurn BattleTurn = new();
        public readonly ReorganizeTurn ReorganizeTurn = new();
        
        public Player[] Players;
        
        private AbstractTurnState _currentState;
        private int _currentPlayerIndex;
        private readonly Random _random = new();

        private void Awake()
        {
            SetupPlayers();
            Instance = this;
            Events.OnRequestPlayer += GetCurrentPlayer;
            Events.OnRequestGold += GetPlayerGold;
            Events.OnSetGold += SetPlayerGold;
            Events.OnNextPlayerStartTurn += SetNextPlayerTurn;
            nextTurnButton.onClick.AddListener(TriggerTurnEndStateButton);
        }
        
        private void SetupPlayers()
        {
            int playerNumber = PlayerPrefs.GetInt("playerNumber", 1);
            int npcNumber = PlayerPrefs.GetInt("npcNumber", 1);
            Debug.Log("Player count: " + playerNumber);
            Debug.Log("Npc count: " + npcNumber);
            List<Color> colors = CreateColors();
            Player[] newPlayers = new Player[playerNumber+npcNumber];
            for(int i = 0; i < playerNumber; i++)
            {
                newPlayers[i] = new Player($"Player {i + 1}", colors[i], false);
            }
            for(int i = playerNumber; i < playerNumber + npcNumber; i++)
            {
                newPlayers[i] = new Player($"AI BOT {i - playerNumber + 1}", colors[i], true);
            }
            Players = newPlayers;
        }

        private List<Color> CreateColors()
        {
            List<int> colorNumbers = new()
            {
                235, 64, 52,
                235, 217, 52,
                122, 235, 52,
                52, 235, 177,
                52, 92, 235,
                125, 52, 235,
                235, 52, 183,
                87, 58, 28,
                62, 64, 21,
                64, 21, 49
            };
            List<Color> colors = new List<Color>();
            for (int i = 0; i < colorNumbers.Count; i+=3)
            {
                Color color = new Color
                {
                    r = colorNumbers[i] / 255f,
                    g = colorNumbers[i+1] / 255f,
                    b = colorNumbers[i+2] / 255f,
                    a = 1
                };
                colors.Add(color);
            }
            
            var n = colors.Count;  
            while (n > 1) {  
                n--;  
                var k = _random.Next(n + 1);  
                (colors[k], colors[n]) = (colors[n], colors[k]);
            } 
            return colors;
        }

        private void Start()
        {
            float volumeSlider = PlayerPrefs.GetFloat("volumeSlider", 1f);
            Debug.Log("Volume slider" + volumeSlider);
            AudioController.Instance.volumeSliderValue = volumeSlider;
            AudioController.Instance.mixer.SetFloat("Master", Mathf.Log(volumeSlider) * 20f);
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

        private void Update()
        {
            if(Input.GetKeyDown("escape"))
            {
                settingsPanel.SetActive(!settingsPanel.activeSelf);
            }
        }

        private void OnDestroy()
        {
            Events.OnRequestPlayer -= GetCurrentPlayer;
            Events.OnRequestGold -= GetPlayerGold;
            Events.OnSetGold -= SetPlayerGold;
            Events.OnNextPlayerStartTurn -= SetNextPlayerTurn;
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
            if(playerColorImage != null)
            {
                playerColorImage.GetComponent<Image>().color = GetCurrentPlayer().color;
            }
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
            // Changing the gold amount seen on screen if the change was on the current player
            if (gold < player.gold)
            {
                goldLossText.gameObject.SetActive(true);
                goldLossText.text = "-" + (player.gold - gold);
                LeanTween.moveLocalY(goldLossText.gameObject, -45, 0.5f).setOnComplete(()=> { 
                    goldLossText.gameObject.SetActive(false);
                    LeanTween.moveLocalY(goldLossText.gameObject, 0, 0.1f);
                });
            } else
            {
            }
            player.gold = gold;
            goldAmountText.text = GetCurrentPlayer().gold.ToString();
        }

        public void SwitchTurnState(AbstractTurnState state)
        {
            _currentState = state;
            StartCoroutine(_currentState.EnterState(this, GetCurrentPlayer()));
            turnNameText.text = _currentState.ToString();
        }

        public void TriggerTurnEndStateButton()
        {
            AudioController.Instance.clickUIButton.Play();
            TriggerTurnEndState();
        }
        public void TriggerTurnEndState()
        {
            StartCoroutine(_currentState.EndState(this, GetCurrentPlayer()));
        }
        private void UpdatePlayerNameAndGold()
        {
            playerNameText.text = GetCurrentPlayer().name;
            goldAmountText.text = GetCurrentPlayer().gold.ToString();
        }
    }
}

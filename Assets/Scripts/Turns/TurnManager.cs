using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
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
        public TextMeshProUGUI goldLossText;
        public GameObject settingsPanel;

        public static TurnManager Instance;
        public readonly PlayerStartTurn PlayerStartTurn = new();
        public readonly ReceiveUnitsTurn ReceiveUnitsTurn = new();
        public readonly MarketTurn MarketTurn = new();
        public readonly BattleTurn BattleTurn = new();
        public readonly ReorganizeTurn ReorganizeTurn = new();
        private AbstractTurnState _currentState;
        
        private Player[] samplePlayers = { new("PlayerMustafa"), new("xxGamerBoyX"), new("CasualGamer"), new("BOT")};
        public Player[] Players;
        private int _currentPlayerIndex;

        private void setupPlayers()
        {
            int playerNumber = PlayerPrefs.GetInt("playerNumber", 2);
            int npcNumber = PlayerPrefs.GetInt("npcNumber", 1);; // TODO add this to menu
            Debug.Log("Player number" + playerNumber);
            Debug.Log("Npc number" + npcNumber);
            Player[] newPlayers = new Player[playerNumber+npcNumber];
            for(int i = 0; i < playerNumber; i++)
            {
                newPlayers[i] = samplePlayers[i];
                Debug.Log(newPlayers[i].Name);
            }
            for(int i = playerNumber; i < playerNumber+npcNumber; i++)
            {
                newPlayers[i] = new Player($"AI BOT {i - playerNumber + 1}", true);
                Debug.Log(newPlayers[i].Name);
            }
            Players = newPlayers;
        }

        private void Awake()
        {
            setupPlayers();
            Instance = this;
            Events.OnRequestPlayer += GetCurrentPlayer;
            Events.OnRequestGold += GetPlayerGold;
            Events.OnSetGold += SetPlayerGold;
            Events.OnNextPlayerStartTurn += SetNextPlayerTurn;
            nextTurnButton.onClick.AddListener(TriggerTurnEndStateButton);
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
            playerNameText.text = GetCurrentPlayer().Name;
            goldAmountText.text = GetCurrentPlayer().gold.ToString();
        }
    }
}

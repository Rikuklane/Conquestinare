using UnityEngine;
using System;
using System.Collections.Generic;

public class Events
{
    // Selections
    public static event Action<int> OnReceiveUnitsSelection;
    public static void DisplayUnitSelection(int cards) => OnReceiveUnitsSelection?.Invoke(cards);
    
    public static event Action<int> OnMarketSelection;
    public static void DisplayMarketSelection(int cards) => OnMarketSelection?.Invoke(cards);
    
    // Players
    public static event Action OnNextPlayerStartTurn;
    public static void NextPlayer() => OnNextPlayerStartTurn?.Invoke();
    public static event Func<Player> OnRequestPlayer;
    public static Player RequestPlayer() => OnRequestPlayer?.Invoke();
    
    // Gold
    public static event Action<Player, int> OnSetGold;
    public static void SetGold(Player player, int gold) => OnSetGold?.Invoke(player, gold);
    
    public static event Func<Player, int> OnRequestGold;
    public static int RequestGold(Player player) => OnRequestGold?.Invoke(player) ?? 0;
}
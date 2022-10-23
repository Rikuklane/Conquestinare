using UnityEngine;
using System;
using System.Collections.Generic;

public class Events
{
    public static event Action OnReceiveUnitsSelection;
    public static void DisplayUnitSelection() => OnReceiveUnitsSelection?.Invoke();
    
    public static event Func<Player> OnRequestPlayer;
    public static Player RequestPlayer() => OnRequestPlayer?.Invoke();
    
    // gold
    public static event Action<int> OnSpendGold;
    public static void SpendGold(int gold) => OnSpendGold?.Invoke(gold);
    
    public static event Action<int> OnAddGold;
    public static void AddGold(int gold) => OnAddGold?.Invoke(gold);
    
    public static event Func<int> OnRequestGold;
    public static int RequestGold() => OnRequestGold?.Invoke() ?? 0;
}
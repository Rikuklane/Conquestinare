using UnityEngine;
using System;
using System.Collections.Generic;

public class Events
{
    public static event Action OnReceiveUnitsSelection;
    public static void DisplayUnitSelection() => OnReceiveUnitsSelection?.Invoke();
}
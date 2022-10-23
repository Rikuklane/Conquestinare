using UnityEngine;
using System;
using System.Collections.Generic;

public class Events
{
    public static event Action<List<UnitData>> OnReceiveUnitsSelection;
    public static void DisplayUnitSelection(List<UnitData> data) => OnReceiveUnitsSelection?.Invoke(data);
}
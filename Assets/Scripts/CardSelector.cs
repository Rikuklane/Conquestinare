using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardSelector : MonoBehaviour
{
    private void Awake()
    {
        gameObject.SetActive(false);
        Events.OnReceiveUnitsSelection += ReceiveUnitsSelection;
    }

    private void OnDestroy()
    {
        Events.OnReceiveUnitsSelection -= ReceiveUnitsSelection;
    }

    private void ReceiveUnitsSelection(List<UnitData> data)
    {
        gameObject.SetActive(true);
    }

    private void SelectUnit()
    {
        gameObject.SetActive(false);
    }
}

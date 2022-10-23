using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Security;
using UnityEngine;
using UnityEngine.UI;

public class UnitCardSelector : MonoBehaviour
{
    public UnitCardPresenter unitCardPrefab;
    public static UnitCardSelector Instance;
    private List<UnitData> _unitSelection;
    private HorizontalLayoutGroup _layoutGroup;
    private void Awake()
    {
        Instance = this;
        _layoutGroup = gameObject.GetComponent<HorizontalLayoutGroup>();
        gameObject.transform.DetachChildren();
        Events.OnReceiveUnitsSelection += ReceiveUnitsSelection;
        SetActive(false);
    }

    private void OnDestroy()
    {
        Events.OnReceiveUnitsSelection -= ReceiveUnitsSelection;
    }

    private void ReceiveUnitsSelection()
    {
        const int count = 3;
        
        _unitSelection = CardCollection.Instance.GetSelectionOfUnits(count);
        foreach (var child in GetComponentsInChildren<UnitCardPresenter>())
        {
            Debug.Log("Destroyed child");
            Destroy(child.gameObject);
        }
        foreach (var unitData in _unitSelection)
        {
            Debug.Log("Create child");
            var unitCard = Instantiate(unitCardPrefab, transform.position, Quaternion.identity, transform);
            unitCard.SwitchState(unitCard.CardInSelection);
            unitCard.SetData(unitData);
        }
        SetActive(true);
    }

    public void SetActive(bool value)
    {
        gameObject.SetActive(value);
        _layoutGroup.gameObject.SetActive(value);
    }
}

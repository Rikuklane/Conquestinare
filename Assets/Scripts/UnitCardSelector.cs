using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UnitCardSelector : MonoBehaviour
{
    private List<UnitData> _unitSelection;
    private UnitCardPresenter[] _unitCardPresenters;
    private HorizontalLayoutGroup _layoutGroup;
    private void Awake()
    {
        _layoutGroup = gameObject.GetComponent<HorizontalLayoutGroup>();
        _unitCardPresenters = gameObject.GetComponentsInChildren<UnitCardPresenter>();
        Events.OnReceiveUnitsSelection += ReceiveUnitsSelection;
        SetActive(false);
    }

    private void OnDestroy()
    {
        Events.OnReceiveUnitsSelection -= ReceiveUnitsSelection;
    }

    private void ReceiveUnitsSelection()
    {
        print(_unitCardPresenters.Length);
        _unitSelection = CardCollection.Instance.GetSelectionOfUnits(_unitCardPresenters.Length);
        print(_unitSelection);
        for (var i = 0; i < _unitSelection.Count; i++)
        {
            var unitCard = _unitCardPresenters[i];
            unitCard.SetSelectionListener(this);
            unitCard.Button.onClick.AddListener(SelectReceivedUnit);
            unitCard.SetData(_unitSelection[i]);
        }
        SetActive(true);
    }

    private void SelectReceivedUnit()
    {
        // TODO add card to hand
        SetActive(false);
    }

    public void SetActive(bool value)
    {
        gameObject.SetActive(value);
        _layoutGroup.gameObject.SetActive(value);
    }
}

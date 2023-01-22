using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SpellCardPresenter : MonoBehaviour
{
    public SpellData spellData;
    public TextMeshProUGUI titleText;
    public TextMeshProUGUI descriptionText;
    public TextMeshProUGUI costText;
    public Image image;
    public GameObject childObject;
    public CardPresenterAbstractLogic cardLogic;

    private void Awake()
    {
        cardLogic.SetVariables(gameObject, childObject, spellData);
        if (spellData != null)
        {
            SetData();
        }
    }

    public void SetData(SpellData data)
    {
        spellData = data;
        if (spellData != null)
        {
            cardLogic.cardData = spellData;
            SetData();
        }
    }

    private void SetData()
    {
        titleText.text = spellData.title;
        descriptionText.text = spellData.description;
        costText.text = spellData.cost.ToString();
        image.sprite = spellData.sprite;
    }
}

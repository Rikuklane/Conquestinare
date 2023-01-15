using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TerritoryGraphics : MonoBehaviour
{
    public Color color;
    public Color playerColor;
    public Color enemyColor;

    public GameObject iconsParent;
    public Image iconPrefab;
    public List<UnitCardPresenter> presentUnits = new();
    public bool isShowBonus = false;
    public bool showingCards = false;

    private List<Image> icons = new();
    private SpriteRenderer spriteRenderer;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        if (color != null) spriteRenderer.material.color = color;
    }

    internal void CheckSelected()
    {
        int numberSelected = 0;
        foreach (UnitCardPresenter card in presentUnits)
        {
            if (card.cardLogic.isSelected)
            {
                numberSelected += 1;
            }
        }
        // cant select last one
        if (presentUnits.Count - numberSelected == 1)
        {
            foreach (UnitCardPresenter card in presentUnits)
            {
                if (!card.cardLogic.isSelected)
                {
                    card.cardLogic.ChangeInteractable(false);
                }
            }
        }
        else
        {
            // change others to interactable
            foreach (UnitCardPresenter card in presentUnits)
            {
                card.cardLogic.ChangeInteractable(true);
            }
        }


    }

    public void ShowBonus(bool showBonus)
    {
        isShowBonus = showBonus;
        if (showBonus)
        {
            iconsParent.GetComponent<Image>().enabled = true;
        }
        else
        {
            iconsParent.GetComponent<Image>().enabled = false;
        }
    }

    public void showCards()
    {
        //foreach(Transform child in parent.transform)
        ///{
        //    Destroy(child.gameObject);
        //}
        foreach (UnitCardPresenter unit in presentUnits)
        {
            //unit.transform.parent = parent.transform;
            unit.gameObject.SetActive(true);
        }
        AttackGUI.instance.TerritoryHoverPanel.SetActive(true);
        showingCards = true;
    }
    public void hideCards()
    {
        foreach (UnitCardPresenter unit in presentUnits)
        {
            //unit.transform.parent = parent.transform;
            unit.gameObject.SetActive(false);
        }
        AttackGUI.instance.TerritoryHoverPanel.SetActive(false);
        showingCards = false;
    }


    public void SetColor(Color _color)
    {
        color = _color;
        ChangeColor(_color);
    }

    public void ChangeColor(Color _color)
    {
        spriteRenderer.material.color = color;

    }

    public void UpdateIcons()
    {
        // remove old
        foreach(Image icon in icons)
        {
            Destroy(icon.gameObject);
        }
        icons.Clear();
        // insert new
        foreach(UnitCardPresenter unit in presentUnits)
        {
            Image icon = Instantiate(iconPrefab, iconsParent.transform);
            icon.sprite = unit.unitData.sprite;
            icons.Add(icon);
        }
    }

    private void OnMouseUp()
    {
        if (AttackLogic.Instance.canHover)
        {
            spriteRenderer.material.color = new Color(245 / 255f, 245 / 255f, 245 / 255f);
        }
    }

    private void OnMouseEnter()
    {
        if (AttackLogic.Instance.canHover)
        {
            spriteRenderer.material.color = new Color(245 / 255f, 245 / 255f, 245 / 255f);
            //showCards();
        }
    }
    private void OnMouseExit()
    {
        if (AttackLogic.Instance.canHover)
        {
            spriteRenderer.material.color = color;
            hideCards();
        }
    }
}

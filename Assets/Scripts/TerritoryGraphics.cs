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

    public TextMeshProUGUI summaryText;
    public GameObject iconsParent;
    public Image iconPrefab;
    public List<UnitCardPresenter> presentUnits = new();

    private List<Image> icons = new();
    private SpriteRenderer spriteRenderer;


    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        if (color != null) spriteRenderer.material.color = color;
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

    }
    public void hideCards()
    {
        foreach (UnitCardPresenter unit in presentUnits)
        {
            //unit.transform.parent = parent.transform;
            unit.gameObject.SetActive(false);
        }
        AttackGUI.instance.TerritoryHoverPanel.SetActive(false);
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

    public void SetPlayerColor(bool isEnemy)
    {
        if (isEnemy)
        {
            SetColor(enemyColor);
        }
        else
        {
            SetColor(playerColor);
        }
    }

    public void SetSummaryText(string text)
    {
        summaryText.text = text;
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
            Image icon = GameObject.Instantiate(iconPrefab, iconsParent.transform);
            icon.sprite = unit.unitData.sprite;
            icons.Add(icon);
        }
    }

    private void OnMouseUp()
    {
        spriteRenderer.material.color = new Color(245 / 255f, 245 / 255f, 245 / 255f);
    }

    private void OnMouseEnter()
    {
        if (AttackLogic.instance.canHover)
        {
            spriteRenderer.material.color = new Color(245 / 255f, 245 / 255f, 245 / 255f);
            showCards();
        }
    }
    private void OnMouseExit()
    {
        if (AttackLogic.instance.canHover)
        {
            spriteRenderer.material.color = color;
            hideCards();
        }
    }
}

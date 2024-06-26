using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TerritoryGraphics : MonoBehaviour
{
    public Color color;

    public GameObject iconsParent;
    public Image attackImage;
    public Image defenseImage;
    public Image markerImage;
    public GameObject ShowBonusPrefab;
    [HideInInspector]
    public List<UnitCardPresenter> presentUnits = new();
    [HideInInspector]
    public bool isShowBonus = false;
    [HideInInspector]
    public bool showingCards = false;

    public ScalingAnimation OpenAnimation;
    public ScalingAnimation CloseAnimation;

    private List<Image> icons = new();
    private Renderer _renderer;
    private Coroutine scrollCoroutine = null;
    private int lastSelected = -1;
    private GameObject bonusInstance = null;

    private void Awake()
    {
        _renderer = GetComponent<Renderer>();
        if (color != null) _renderer.material.color = color;
    }

    internal void CheckSelected(CardPresenterAbstractLogic currentSelected)
    {
        int numberSelected = 0;
        foreach (UnitCardPresenter card in presentUnits)
        {
            if (card.cardLogic.isSelected)
            {
                numberSelected += 1;
            }
        }
        // deselect first selected
        if (presentUnits.Count > 1 && presentUnits.Count == numberSelected)
        {
            presentUnits[lastSelected].cardLogic.TriggerSelected();
        }
        lastSelected = presentUnits.IndexOf(currentSelected.transform.parent.GetComponent<UnitCardPresenter>());
    }

    public void ShowBonus(bool showBonus)
    {
        isShowBonus = showBonus;
        if (showBonus)
        {
            bonusInstance = Instantiate(ShowBonusPrefab, iconsParent.transform.parent.position, Quaternion.identity, iconsParent.transform.parent);
            //iconsParent.GetComponent<Image>().enabled = true;
        }
        else
        {
            Destroy(bonusInstance);
            //iconsParent.GetComponent<Image>().enabled = false;
        }
    }

    public void showCards()
    {
        if(!AttackGUI.instance.TerritoryHoverPanel.activeSelf)
        {
            AudioController.Instance.place.Play();
        }
        if (AttackGUI.instance.TerritoryHoverPanel.activeSelf)
            return;
        foreach (UnitCardPresenter unit in presentUnits)
        {
            //unit.transform.parent = parent.transform;
            unit.gameObject.SetActive(true);
        }
        AttackGUI.instance.TerritoryHoverText.gameObject.SetActive(true);
        if (presentUnits.Count < 2) {
            AttackGUI.instance.TerritoryHoverText.text = presentUnits.Count + " card\n" + GetComponent<Territory>().getSummary();

        }
        else
        {
            AttackGUI.instance.TerritoryHoverText.text = presentUnits.Count + " cards\n" + GetComponent<Territory>().getSummary();
        }
        AttackGUI.instance.TerritoryHoverPanel.SetActive(true);
        if (scrollCoroutine != null) StopCoroutine(scrollCoroutine);
        scrollCoroutine = StartCoroutine(AttackGUI.instance.ScrollToRight(1f+1*presentUnits.Count/6));
        OpenAnimation.enabled = true;
        showingCards = true;
    }
    public void hideCards()
    {
        if (!AttackGUI.instance.TerritoryHoverPanel.activeSelf)
            return;
        foreach (UnitCardPresenter unit in presentUnits)
        {
            //unit.transform.parent = parent.transform;
            unit.gameObject.SetActive(false);
        }
        AttackGUI.instance.TerritoryHoverText.gameObject.SetActive(false);
        //AttackGUI.instance.TerritoryHoverPanel.SetActive(false);
        CloseAnimation.enabled = true;
        showingCards = false;
    }


    public void SetColor(Color _color)
    {
        color = _color;
        ChangeColor(_color);
    }

    public void ChangeColor(Color _color)
    {
        _renderer.material.color = color;

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
            Image icon = Instantiate(TerritoryManager.instance.iconPrefab, iconsParent.transform);
            icon.transform.localScale = new Vector3(0.4f, 0.4f, 0.4f);
            icon.sprite = unit.unitData.sprite;
            icons.Add(icon);
        }
    }

    private void OnMouseUp()
    {
        if (AttackLogic.Instance.canHover)
        {
            _renderer.material.color = new Color(245 / 255f, 245 / 255f, 245 / 255f);
        }
    }

    private void OnMouseEnter()
    {
        if (AttackLogic.Instance.canHover)
        {
            _renderer.material.color = new Color(245 / 255f, 245 / 255f, 245 / 255f);
            //showCards();
        }
    }
    private void OnMouseExit()
    {
        if (AttackLogic.Instance.canHover)
        {
            _renderer.material.color = color;
            hideCards();
        }
    }
}

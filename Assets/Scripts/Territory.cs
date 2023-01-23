using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;
using Random = System.Random;

public class Territory : MonoBehaviour
{
    public Player player;

    [Header("Neighbours")]
    [Space]
    public List<Territory> territories = new();
    [HideInInspector]
    public List<Territory> enemyTerritories = new();
    [HideInInspector]
    public List<Territory> allyTerritories = new();

    [Header("Units")]
    [Space]
    [HideInInspector]
    public List<UnitData> startUnits = new();
    public class Unit
    {
        public int attack;
        public int health;
        
        public Unit() { }

        public Unit(int attack, int health)
        {
            this.attack = attack;
            this.health = health;
        }
    }

    public List<Unit> units = new();

    [Header("Graphics")]
    [Space]

    public TerritoryGraphics TerritoryGraphics;

    [Header("Bonus Group")]
    [Space]
    public int bonusGroup;
    private readonly Random _random = new();
    private bool defenseActivated = false;

    public void AddUnits()
    {
        UpdateNeighborTerritories();
        foreach (UnitData unit in startUnits)
        {
            AddCard(unit, null);
        }
    }

    public string getSummary()
    {
        int attack = 0;
        int health = 0;
        foreach (Unit unit in units)
        {
            attack += unit.attack;
            health += unit.health;
        }
        return attack.ToString() + "AD/" + health.ToString() + "HP";
    }

    public Unit GetAttackHealth()
    {
        int attack = 0;
        int health = 0;
        foreach (Unit unit in units)
        {
            attack += unit.attack;
            health += unit.health;
        }

        return new Unit(attack, health);
    }

    public void CastSpellOnUnits(SpellData spellData)
    {
        for (int repetition = 0; repetition < spellData.repetition; repetition++)
        {
            if (spellData.effectArea == EffectArea.WholeTile)
            {
                for (int i = 0; i < units.Count; i++)
                {
                    SetUnitAttackAndHealth(i, spellData.attackChange, spellData.healthChange);
                }
            }
            else
            {
                SetUnitAttackAndHealth(_random.Next(units.Count), spellData.attackChange, spellData.healthChange);
            }    
        }
    }

    private void SetUnitAttackAndHealth(int i, int attack, int health)
    {
        units[i].attack += attack;
        units[i].health += health;
        TerritoryGraphics.presentUnits[i].SetAttack(units[i].attack);
        TerritoryGraphics.presentUnits[i].SetHealth(units[i].health);
        if (units[i].health <= 0)
        {
            RemoveCard(i);
            print("unit died");
        }
    }

    public void AttackUnit(int index, int damage)
    {
        //print(index + " " + damage + " " + units[index].health);
        units[index].health -= damage;
        TerritoryGraphics.presentUnits[index].SetHealth(units[index].health);

        if (units[index].health <= 0)
        {
            RemoveCard(index);
            print("unit died");
        }
    }

    public Unit GetUnitAtIndex(int index)
    {
        return units[index];
    }

    public int GetUnitsCount()
    {
        return units.Count;
    }
    public void AddCard(UnitData data, Unit unit)
    {
        UnitCardPresenter card = Instantiate(CardHand.Instance.unitCardPrefab, AttackGUI.instance.TerritoryHoverPanel.transform.GetChild(0).GetChild(0));
        card.SetData(data);
        if(unit != null)
        {
            card.SetHealth(unit.health);
        }
        AddCard(card, unit);
    }


    private void AddCard(UnitCardPresenter card, Unit unit)
    {

        card.gameObject.SetActive(false);
        // overwrite scale
        card.transform.localScale = new Vector3(1, 1, 1);

        card.cardLogic.SwitchState(CardStateController.Instance.CardInTerritory);

        Color color = card.childObject.GetComponent<Image>().color;
        color.a = 0.6f;
        card.childObject.GetComponent<Image>().color = color;

        //presentUnits.Add(card);
        TerritoryGraphics.presentUnits.Add(card);

        Unit newUnit = new();
        if (unit != null)
        {
            newUnit.attack = unit.attack;
            newUnit.health = unit.health;
        } else
        {
            newUnit.attack = card.unitData.attack;
            newUnit.health = card.unitData.health;
        }
        
        units.Add(newUnit);
        UpdateTerritoryImage();

        UpdateNeighborTerritories();

        card.cardLogic.isSelected = false;
        //TODO
    }

    public void RemoveCard(int index)
    {
        units.RemoveAt(index);
        //Destroy(presentUnits[index].gameObject);
        //presentUnits.RemoveAt(index);
        Destroy(TerritoryGraphics.presentUnits[index].gameObject);
        TerritoryGraphics.presentUnits.RemoveAt(index);
        UpdateTerritoryImage();
    }

    public void UpdateTerritoryImage()
    {
        // Summary
        int attack = 0;
        int health = 0;
        foreach(Unit unit in units)
        {
            attack += unit.attack;
            health += unit.health;
        }
        TerritoryGraphics.UpdateIcons();
        // Color
        if (attack == 0 && health == 0)
        {
            player = new Player("neutral", Color.gray, false);
        }
        TerritoryGraphics.SetColor(player.color);
    }

    public void ShowReorganizeOptionsRecursive(bool value)
    {
        if (defenseActivated == value) return;
        defenseActivated = value;
        TerritoryGraphics.defenseImage.enabled = value;
        foreach (Territory t in allyTerritories)
        {
            t.ShowReorganizeOptionsRecursive(value);
        }
    }


    public void ShowAttackOptions(bool isReorganizeTurn)
    {
        TerritoryGraphics.markerImage.enabled = true;
        if (isReorganizeTurn)
        {
            ShowReorganizeOptionsRecursive(true);
            TerritoryGraphics.defenseImage.enabled = false;
        } else
        {
            foreach (Territory t in enemyTerritories)
            {
                t.TerritoryGraphics.attackImage.enabled = true;
            }
        }

    }

    public void HideAttackOptions(bool isReorganizeTurn)
    {
        TerritoryGraphics.markerImage.enabled = false;
        if (isReorganizeTurn)
        {
            ShowReorganizeOptionsRecursive(false);
            //TerritoryGraphics.defenseImage.enabled = false;
        }
        else
        {
            foreach (Territory t in enemyTerritories)
            {
                t.TerritoryGraphics.attackImage.enabled = false;
            }
        }
    }

    public void UpdateNeighborTerritories()
    {
        enemyTerritories.Clear();
        allyTerritories.Clear();
        foreach(Territory area in territories)
        {
            if (player != area.player || area.player.name == "neutral")
            {
                enemyTerritories.Add(area);
            } else
            {
                allyTerritories.Add(area);
            }
        }
    }

    public void MoveCardToTerritory(CardPresenterAbstractLogic cardSelected, Vector3 targetPos)
    {
        LeanTween.move(cardSelected.cardInstance.gameObject, targetPos, 0.1f)
            .setOnComplete(() =>
            {
                AudioController.Instance.place.Play();
                cardSelected.childGameObject.transform.localPosition = Vector3.zero;
                if (cardSelected.cardData.GetType() == typeof(UnitData))
                {
                    AddCard((UnitData)cardSelected.cardData, null);
                }
                else if (cardSelected.cardData.GetType() == typeof(SpellData))
                {
                    CastSpellOnUnits((SpellData)cardSelected.cardData);
                }
                CardHand.Instance.DestroySelected();

            });
    }

    private void OnMouseOver()
    {
        if (AttackLogic.Instance.canHover)
        {
            if (Input.GetMouseButtonDown(0))
            {
                // Check if card selected
                if (CardHand.Instance.cardSelected)
                {
                    CardPresenterAbstractLogic cardSelected = CardHand.Instance.cardSelected;
                    if (cardSelected && (player == Events.RequestPlayer() || cardSelected.cardData.GetType() == typeof(SpellData)))
                    {
                        Vector3 targetPos = Camera.main.WorldToScreenPoint(transform.position);
                        //Vector3 targetPos = transform.InverseTransformVector(AttackGUI.instance.transform.position - transform.position);
                        MoveCardToTerritory(cardSelected, targetPos);
                    }
                }
                else
                {
                    if (Events.RequestPlayer().isNpc) return;
                    if (Turns.TurnManager.Instance.settingsPanel.activeSelf) return;
                    AttackLogic.Instance.SelectTerritory(this);
                }

                //waypoint.ToggleLines();
                TerritoryGraphics.ChangeColor(new Color(200 / 255f, 200 / 255f, 200 / 255f));
            } else if (Input.GetMouseButton(1))
            {
                TerritoryGraphics.showCards();
            } else if(TerritoryGraphics.showingCards)
            {
                TerritoryGraphics.hideCards();
            }
        }

    }

}

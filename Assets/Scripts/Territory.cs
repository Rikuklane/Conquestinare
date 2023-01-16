using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;
using Random = System.Random;

public class Territory : MonoBehaviour
{
    public Waypoint waypoint;
    public Player player;

    public List<Territory> territories = new();
    public List<Vector3> enemyTerritories = new();
       
    public List<UnitData> startUnits = new();

    public UnitCardPresenter cardPrefab;

    public TerritoryManager.BonusGroup bonusGroup;
    private readonly Random _random = new();

    public class Unit
    {
        public int attack;
        public int health;
    }

    public List<Unit> units = new();

    public TerritoryGraphics TerritoryGraphics;
        
    void Awake()
    {

        foreach(Territory area in territories)
        {
            waypoint.routes.Add(area.waypoint);
        }
        waypoint.CreateLines();

    }

    public void AddUnits()
    {
        UpdateEnemyTerritories();
        foreach (UnitData unit in startUnits)

        {
            AddCard(unit, null);
        }
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
        UnitCardPresenter card = Instantiate(cardPrefab, AttackGUI.instance.TerritoryHoverPanel.transform);
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

        UpdateEnemyTerritories();

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
            player = new Player("neutral", Color.gray);
        }
        TerritoryGraphics.SetColor(player.color);
    }

    public void ShowAttackOptions()
    {
        waypoint.SetLines(enemyTerritories, true);
    }

    public void HideAttackOptions()
    {
        waypoint.SetLines(enemyTerritories, false);
    }

    public void UpdateEnemyTerritories()
    {
        enemyTerritories.Clear();
        foreach(Territory area in territories)
        {
            if (player != area.player || area.player.Name == "neutral")
            {
                enemyTerritories.Add(area.waypoint.transform.position);
            }
        }
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
                    if (cardSelected && (player == Events.RequestPlayer() || cardSelected.CardData.GetType() == typeof(SpellData)))
                    {
                        Vector3 targetPos = Camera.main.WorldToScreenPoint(transform.position);
                        //Vector3 targetPos = transform.InverseTransformVector(AttackGUI.instance.transform.position - transform.position);
                        LeanTween.move(cardSelected.CardInstance.gameObject, targetPos, 0.1f)
                        .setOnComplete(() =>
                        {
                            cardSelected.ChildGameObject.transform.localPosition = Vector3.zero;
                            if (cardSelected.CardData.GetType() == typeof(UnitData))
                            {
                                AddCard((UnitData)cardSelected.CardData, null);
                            }
                            else if (cardSelected.CardData.GetType() == typeof(SpellData))
                            {
                                CastSpellOnUnits((SpellData)cardSelected.CardData);
                            }
                            CardHand.Instance.DestroySelected();

                        });
                    }
                }
                else
                {
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

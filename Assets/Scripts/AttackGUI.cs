using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class AttackGUI : MonoBehaviour
{
    public static AttackGUI instance;

    public Button attackButton;
    public GameObject TerritoryHoverPanel;
    public GameObject ArenaPanel;
    public GameObject ArenaTopPanel;
    public GameObject ArenaBottomPanel;

    public GameObject WinScreen;
    public TextMeshProUGUI WinScreenText;

    public List<UnitCardPresenter> arena1Cards = new();
    public List<UnitCardPresenter> arena2Cards = new();

    public AnimationCurve animationCurve;

    void Awake()
    {
        instance = this;
    }

    public void GameOver(bool isWin)
    {
        if (isWin)
        {
            WinScreen.SetActive(true);
            WinScreenText.text = "You Win!";
        } else
        {
            WinScreen.SetActive(true);
            WinScreenText.text = "You lose";
        }
    }

    public void ShowBattle(Territory attackers, Territory defenders)
    {
        arena1Cards.Clear();
        arena2Cards.Clear();
        // calculations for horizontal layout group simulation
        float cardSpacing = 75f;
        float xAddition = 0;
        float xAttackersStart;
        if (attackers.TerritoryGraphics.presentUnits.Count % 2 == 0)
        {
            xAttackersStart = -(attackers.TerritoryGraphics.presentUnits.Count / 2 - 0.5f) * cardSpacing;
        }
        else
        {
            xAttackersStart = -(attackers.TerritoryGraphics.presentUnits.Count / 2) * cardSpacing;
        }
        //Debug.Log(xAttackersStart);
        // Create top panel of cards (attacker)
        foreach (UnitCardPresenter unit in attackers.TerritoryGraphics.presentUnits)
        {
            UnitCardPresenter card = Instantiate(attackers.cardPrefab, ArenaTopPanel.transform);
            card.transform.localPosition = new Vector3(xAttackersStart + xAddition, 0, 0);
            card.SetData(unit.unitData);
            card.SetHealth(unit.health);
            Debug.Log("added attacker");
            arena1Cards.Add(card);
            xAddition += cardSpacing;
        }
        // calculations for horizontal layout group simulation
        xAddition = 0;
        float xDefendersStart;
        if (defenders.TerritoryGraphics.presentUnits.Count % 2 == 0)
        {
            xDefendersStart = -(defenders.TerritoryGraphics.presentUnits.Count / 2 - 0.5f) * cardSpacing;
        }
        else
        {
            xDefendersStart = -(defenders.TerritoryGraphics.presentUnits.Count / 2) * cardSpacing;
        }
        // Create bottom panel of cards (defender)
        foreach (UnitCardPresenter unit in defenders.TerritoryGraphics.presentUnits)
        {
            UnitCardPresenter card = Instantiate(defenders.cardPrefab, ArenaBottomPanel.transform);
            card.transform.localPosition = new Vector3(xDefendersStart + xAddition, 0, 0);
            card.SetData(unit.unitData);
            card.SetHealth(unit.health);
            Debug.Log("added defender");
            arena2Cards.Add(card);
            xAddition += cardSpacing;
        }
        ArenaPanel.gameObject.SetActive(true);
    }
    public void HideBattle()
    {
        ArenaPanel.gameObject.SetActive(false);
        foreach (Transform child in ArenaTopPanel.transform) {
            Destroy(child.gameObject);
        }
        foreach (Transform child in ArenaBottomPanel.transform)
        {
            Destroy(child.gameObject);
        }
        arena1Cards.Clear();
        arena2Cards.Clear();
    }

    public void ChangeButtonClickAttack(bool isAttack)
    {
        if (isAttack)
        {
            attackButton.GetComponentInChildren<TextMeshProUGUI>().text = "ATTACK";
            attackButton.onClick.RemoveAllListeners();
            attackButton.onClick.AddListener(AttackLogic.Instance.AttackPressed);
        }
        else
        {
            attackButton.GetComponentInChildren<TextMeshProUGUI>().text = "CONFIRM";
            attackButton.onClick.RemoveAllListeners();
            attackButton.onClick.AddListener(AttackLogic.Instance.CheckSelected);
        }

    }

    internal IEnumerator AttackAnimation(bool isDefenderTurn, Territory attacker, Territory defender)
    { 
        UnitCardPresenter animationFrom;
        UnitCardPresenter animationTo;
        if (isDefenderTurn)
        {
            animationFrom = arena2Cards[0];
            animationTo = arena1Cards[0];
        } else
        {
            animationFrom = arena1Cards[0];
            animationTo = arena2Cards[0];
        }
        Vector3 origin = animationFrom.transform.position;
        /*
        // option 1 - leantween
        RectTransform rectTransform = animationTo.GetComponent<RectTransform>();


        Vector3[] v = new Vector3[4];
        rectTransform.GetWorldCorners(v);


        Vector3 origin = animationFrom.transform.position;
        Vector3 target = v[2];

        LeanTween.move(animationFrom.gameObject, target, 1f).setOnComplete(() => { LeanTween.move(animationFrom.gameObject, origin, 2f); });
        */
        // option 2 - manual
        yield return StartCoroutine(MoveUsingCurve(animationFrom, animationTo, 1f));
        // simulate dealing damage
        if (isDefenderTurn)
        {
            int playerAttack = defender.GetUnitAtIndex(0).attack;
            attacker.AttackUnit(0, playerAttack);
            AttackCard(attacker, isDefenderTurn);
        }
        else
        {
            int enemyAttack = attacker.GetUnitAtIndex(0).attack;
            defender.AttackUnit(0, enemyAttack);
            AttackCard(defender, isDefenderTurn);
        }
        yield return StartCoroutine(MoveBack(animationFrom, origin, 0.6f));
        AttackLogic.Instance.SimulateBattle();
    }

    void AttackCard(Territory territory, bool isDefenderTurn)
    {
        if (isDefenderTurn)
        {
            if (territory.units.Count < arena1Cards.Count || territory.units[0].health <= 0)
            {
                Destroy(arena1Cards[0].gameObject);
                arena1Cards.RemoveAt(0);
            } else
            {
                arena1Cards[0].SetHealth(territory.units[0].health);
            }
        }
        else
        {
            if (territory.units.Count < arena2Cards.Count || territory.units[0].health <= 0)
            {
                Destroy(arena2Cards[0].gameObject);
                arena2Cards.RemoveAt(0);
            } else
            {
                arena2Cards[0].SetHealth(territory.units[0].health);
            }
        }
    }
        



    IEnumerator MoveUsingCurve(UnitCardPresenter animationFrom, UnitCardPresenter animationTo, float duration)
    {
        Vector3 origin = animationFrom.transform.position;
        RectTransform rectTransform = animationTo.GetComponent<RectTransform>();

        Vector3[] v = new Vector3[4];
        rectTransform.GetWorldCorners(v);

        Vector3 target;
        if(origin.y > rectTransform.transform.position.y)
        {
            target = new Vector3((v[2].x + v[1].x) / 2, v[2].y, v[2].z);

        } else
        {
            target = new Vector3((v[0].x + v[3].x) / 2, v[0].y, v[0].z);
        }
        float timePassed = 0f;
        while (timePassed <= duration)
        {
            timePassed += Time.deltaTime;
            float percent = Mathf.Clamp01(timePassed / duration);
            float curvePercent = animationCurve.Evaluate(percent);
            animationFrom.transform.position = Vector3.LerpUnclamped(origin, target, curvePercent);
            yield return null;
        }
    }

    IEnumerator MoveBack(UnitCardPresenter animationFrom, Vector3 animationTo, float duration)
    {
        //animationTo.transform.SetParent(transform, true);
        Vector3 origin = animationFrom.transform.position;
        Vector3 target = animationTo;
        float timePassed = 0f;
        while (timePassed <= duration)
        {
            timePassed += Time.deltaTime;
            float percent = Mathf.Clamp01(timePassed / duration);
            float curvePercent = animationCurve.Evaluate(percent);
            animationFrom.transform.position = Vector3.LerpUnclamped(origin, target, curvePercent);
            yield return null;
        }
    }

    public void AttackCleanup()
    {
        ArenaPanel.gameObject.SetActive(false);
        attackButton.gameObject.SetActive(false);
        // weird fix
        TerritoryHoverPanel.gameObject.SetActive(false);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Game/UnitData")]
public class UnitData : CardData
{
    [Range(0, 10)]
    public int attack;
    [Range(0, 10)]
    public int health;
    public Sprite sprite;
}

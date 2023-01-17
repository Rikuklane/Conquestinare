using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Game/SpellData")]
public class SpellData : CardData
{
    public Sprite sprite;
    public EffectArea effectArea = EffectArea.WholeTile;
    [Range(-10, 10)]
    public int attack;
    [Range(-10, 10)]
    public int health;
}

public enum EffectArea
{
    WholeTile, RandomInTile
}

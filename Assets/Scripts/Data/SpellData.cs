using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Game/SpellData")]
public class SpellData : CardData
{
    public Sprite sprite;
    public EffectArea effectArea = EffectArea.WholeTile;
    public int attack;
    public int health;
}

public enum EffectArea
{
    WholeTile, RandomInTile
}

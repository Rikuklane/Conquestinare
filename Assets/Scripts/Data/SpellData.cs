using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Game/SpellData")]
public class SpellData : CardData
{
    public Sprite sprite;
    public SpellType type;
    public EffectArea effectArea;
}

public enum SpellType
{
    Hostile, Friendly
}

public enum EffectArea
{
    WholeTile, RandomInTile
}


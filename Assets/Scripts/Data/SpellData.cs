using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Game/SpellData")]
public class SpellData : CardData
{
    public Sprite sprite;
    public EffectArea effectArea = EffectArea.WholeTile;
    public int attackChange;
    public int healthChange;
    public int repetition = 1;
}

public enum EffectArea
{
    WholeTile, RandomInTile
}

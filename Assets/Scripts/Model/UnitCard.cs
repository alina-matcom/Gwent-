using System;
using UnityEngine;

[CreateAssetMenu(fileName = "Card", menuName = "Card/Unit Card")]
public class UnitCard : CardOld
{
    public UnitType type;
    public int originalPower;
    public int power;
    public string effect;

    public override void Reset()
    {
        power = originalPower;
    }

    public override BoardSlot GetBoardSlot()
    {
        return type switch
        {
            UnitType.Melee => BoardSlot.MeleeZone,
            UnitType.Ranged => BoardSlot.RangedZone,
            UnitType.Siege => BoardSlot.SiegeZone,
            _ => throw new ArgumentException("Invalid card type"),
        };
    }
}
using UnityEngine;

[CreateAssetMenu(fileName = "Card", menuName = "Card/Field Card")]
public class FieldCard : SpecialCard
{
  public UnitType affectedType;
  public int powerReduction;

  public override BoardSlot GetBoardSlot()
  {
    return BoardSlot.FieldZone;
  }

  public override void Reset() { }
}
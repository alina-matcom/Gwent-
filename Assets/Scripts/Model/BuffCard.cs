using UnityEngine;

[CreateAssetMenu(fileName = "Card", menuName = "Card/Buff Card")]
public class BuffCard : SpecialCard
{
  public int powerIncrease = 0;
  public override void ApplyEffect() { }

  public override BoardSlot GetBoardSlot()
  {
    return BoardSlot.Buff;
  }

  public override void Reset() { }
}
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Card", menuName = "Card/Lider Card")]
public class LiderCard : CardOld
{
   int charges = 1;

  public void ApplyEffect(Zone targetZone)
  {
    Debug.Log("yeah");
    if (charges > 0 && targetZone != null)
    {
      Debug.Log("sip");
      charges--;
      List<CardDisplay> cards = targetZone.unitsSlot.GetCards();
      foreach (CardDisplay card in cards)
      {

        Debug.Log("rico");
        UnitCard unitCard = card.card as UnitCard;
        if (unitCard != null)
        {
          Debug.Log("sabroso");
          int doubledPower = unitCard.power * 2;
          card.SetPower(doubledPower);
        }
      }
    }

    BoardController.TriggerScoreUpdateNeeded();
  }


  public override BoardSlot GetBoardSlot()
  {
    return BoardSlot.None;
  }

public void ResetCharges()
{
    charges = 1;
}
  public override void Reset() { }
}
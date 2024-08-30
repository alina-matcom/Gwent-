using UnityEngine;

public class BuffSlot : Slot
{
  public CardDisplay cardDisplay;
  public CardOld card;

  public override void PlayCard(CardOld card)
{
    if (card == null)
    {
        Debug.LogError("PlayCard was called with a null Card.");
        return;
    }

    this.card = card;
    cardDisplay.SetCard(card);
    cardDisplay.gameObject.SetActive(true);
}

  public void Clear()
  {
    card = null;
    cardDisplay.gameObject.SetActive(false);
  }
}
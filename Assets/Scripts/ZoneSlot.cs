using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using GwentInterpreters;
using UnityEngine;

public class ZoneSlot : Slot
{
  public CardDisplay cardPrefab;
  public GameObject slot;
  private readonly float cardWidth = 100f;
  private readonly float zoneWidth = 600f;

  public List<CardDisplay> GetCards()
  {
    List<CardDisplay> cards = new();

    for (int i = 0; i < slot.transform.childCount; i++)
    {
      cards.Add(slot.transform.GetChild(i).GetComponent<CardDisplay>());
    }

    return cards;
  }

  public override void PlayCard(CardOld card)
  {
    if (card == null)
    {
      Debug.LogError("PlayCard was called with a null Card.");
      return;
    }

    if (card is UnitCard unitCard)
    {
      // Lógica específica para UnitCard
      CardDisplay cardDisplay = Instantiate(cardPrefab, slot.transform);
      cardDisplay.SetCard(unitCard);
      cardDisplay.transform.localScale = new Vector3(0.3f, 0.3f, 0.3f);
      cardDisplay.transform.localPosition = new Vector3(0, 0, 0);
      StartCoroutine(AdjustCardPositions());
    }
    else if (card is Card newCard)
    {
      // Lógica específica para Card
      CardDisplay cardDisplay = Instantiate(cardPrefab, slot.transform);
      cardDisplay.SetCard(newCard);
      cardDisplay.transform.localScale = new Vector3(0.3f, 0.3f, 0.3f);
      cardDisplay.transform.localPosition = new Vector3(0, 0, 0);
      StartCoroutine(AdjustCardPositions());
      // Llamada al método para aplicar el efecto de la carta
      newCard.ApplyEffects();
    }
  }

  public double GetRowPower()
  {
    return GetCards().Sum(cardDisplay =>
    {
      var unitCard = cardDisplay.card as UnitCard;
      var genericCard = cardDisplay.card as GwentInterpreters.Card;

      if (unitCard != null)
      {
        return unitCard.power;
      }
      else if (genericCard != null)
      {
        return genericCard.Power;
      }
      else
      {
        return 0;
      }
    });
  }
  public void ApplyEffect(PowerModifier modifier, int value)
  {
    List<CardDisplay> cards = GetCards();

    foreach (CardDisplay card in cards)
    {
      UnitCard unitCard = card.card as UnitCard;

      switch (modifier)
      {
        case PowerModifier.Increment:
          // Usa el poder actual de la carta de unidad en lugar de originalPower
          card.SetPower(unitCard.power + value);
          break;
        case PowerModifier.Decrement:
          card.SetPower(unitCard.power - value);
          break;
        case PowerModifier.Fix:
          card.SetPower(value);
          break;
      }
    }
  }

  IEnumerator AdjustCardPositions()
  {
    yield return new WaitForSeconds(0.01f);

    int cardCount = slot.transform.childCount;
    float totalCardsWidth = cardCount * cardWidth;
    bool fits = totalCardsWidth - zoneWidth < 0;
    float spacing = fits ? 10f : (zoneWidth - cardCount * cardWidth) / cardCount;
    float startX = (zoneWidth - (totalCardsWidth + spacing * (cardCount - 1))) / 2f;

    for (int i = 0; i < cardCount; i++)
    {
      Transform card = slot.transform.GetChild(i);
      float xPosition = startX + i * (cardWidth + spacing);
      card.localPosition = new Vector3(xPosition - zoneWidth / 2f, 0, (-i - 1) * 10);
    }
  }
}
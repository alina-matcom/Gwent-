using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FieldZone : Slot
{
    public int maxClimateCards = 3;
    private int currentClimateCards = 0;
    private Dictionary<UnitType, CardDisplay> climateCards = new Dictionary<UnitType, CardDisplay>();

    public Zone playerMeleeZone;
    public Zone playerRangedZone;
    public Zone playerSiegeZone;
    public Zone enemyMeleeZone;
    public Zone enemyRangedZone;
    public Zone enemySiegeZone;

    public CardDisplay climateCardPrefab;
    public GameObject climateCardsContainer;

    private readonly float zoneWidth = 245f;

    public override void PlayCard(CardOld card)
    {
        if (card == null)
        {
            Debug.LogError("PlayCard was called with a null Card.");
            return;
        }
        if (currentClimateCards >= maxClimateCards)
        {
            Debug.Log("No more climate cards can be played in this zone.");
            return;
        }

        if (card is FieldCard fieldCard)
        {
            if (climateCards.ContainsKey(fieldCard.affectedType))
            {
                Debug.Log("A climate card for this type already exists.");
                return;
            }

            CardDisplay climateCardDisplay = Instantiate(climateCardPrefab, climateCardsContainer.transform);
            climateCardDisplay.SetCard(fieldCard);
            climateCardDisplay.transform.localScale = new Vector3(0.3f, 0.3f, 0.3f);
            climateCardDisplay.transform.localPosition = new Vector3(0, 0, 0); // Ajusta el valor "-100" según sea necesario

            climateCards.Add(fieldCard.affectedType, climateCardDisplay);
            currentClimateCards++;

            ApplyClimateEffect(fieldCard);
            StartCoroutine(AdjustCardPositions());
        }
        else
        {
            Debug.Log("Only FieldCards can be played in the FieldZone.");
        }
    }

    private void ApplyClimateEffect(FieldCard fieldCard)
    {
        Zone playerZone = null;
        Zone enemyZone = null;

        switch (fieldCard.affectedType)
        {
            case UnitType.Melee:
                playerZone = playerMeleeZone;
                enemyZone = enemyMeleeZone;
                break;
            case UnitType.Ranged:
                playerZone = playerRangedZone;
                enemyZone = enemyRangedZone;
                break;
            case UnitType.Siege:
                playerZone = playerSiegeZone;
                enemyZone = enemySiegeZone;
                break;
        }

        if (playerZone != null)
        {
            playerZone.unitsSlot.ApplyEffect(PowerModifier.Decrement, fieldCard.powerReduction);
        }
        if (enemyZone != null)
        {
            enemyZone.unitsSlot.ApplyEffect(PowerModifier.Decrement, fieldCard.powerReduction);
        }
    }

    public void RemoveClimateCard(UnitType affectedType)
    {
        if (climateCards.TryGetValue(affectedType, out CardDisplay climateCardDisplay))
        {
            Destroy(climateCardDisplay.gameObject);
            climateCards.Remove(affectedType);
            currentClimateCards--;
        }
    }

    public override void Highlight(BoardSlot slot, int player)
    {
        if (CardManager.Instance.selectedCard != null && CardManager.Instance.selectedCard.card is FieldCard)
        {
            _highlight.SetActive(slotType == BoardSlot.FieldZone);
        }
        else
        {
            base.Highlight(slot, player);
        }
    }

    IEnumerator AdjustCardPositions()
    {
        yield return new WaitForSeconds(0.01f);

        int cardCount = climateCardsContainer.transform.childCount;
        float cardWidth = zoneWidth / cardCount; // Calcula el ancho de cada carta
        float startX = -zoneWidth / 2f + cardWidth / 2f; // Calcula la posición inicial

        for (int i = 0; i < cardCount; i++)
        {
            Transform card = climateCardsContainer.transform.GetChild(i);
            float xPosition = startX + i * cardWidth;
            card.localPosition = new Vector3(xPosition, 0, (-i - 1) * 10);
        }
    }
}

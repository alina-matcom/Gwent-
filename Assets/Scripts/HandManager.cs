using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandManager : MonoBehaviour
{
    public HandCardDisplay cardPrefab;
    public float handWidth = 1200f; // Total width of the hand space
    public float cardWidth = 170f; // Width of each card
    public int maxCardsInHand = 10;
    public int owner;

    public void AddCard(CardOld card)
    {
        if (card == null)
        {
            Debug.LogError("AddCard was called with a null Card.");
            return;
        }

        if (transform.childCount > maxCardsInHand)
        {
            return;
        }

        HandCardDisplay newCard = Instantiate(cardPrefab, transform);
        newCard.card = card;
        newCard.SetCard(card);
        newCard.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);

        StartCoroutine(AdjustCardPositions());
    }

    public void RemoveCard(CardDisplay card)
    {
        Destroy(card.gameObject);

        StartCoroutine(AdjustCardPositions());
    }

    public void Hide(bool hide)
    {
        transform.localPosition = new Vector3(-530, hide ? -700 : -530, 0);
        StartCoroutine(AdjustCardPositions());
    }

    public int GetCount()
    {
        return transform.childCount;
    }
    public List<CardOld> GetCardsInHand()
    {
        List<CardOld> cardsInHand = new List<CardOld>();
        for (int i = 0; i < transform.childCount; i++)
        {
            HandCardDisplay cardDisplay = transform.GetChild(i).GetComponent<HandCardDisplay>();
            if (cardDisplay != null)
            {
                cardsInHand.Add(cardDisplay.card);
            }
        }
        return cardsInHand;
    }

    IEnumerator AdjustCardPositions()
    {
        yield return new WaitForSeconds(0.01f);

        int cardCount = transform.childCount;
        float totalCardsWidth = cardCount * cardWidth;
        bool fits = totalCardsWidth - handWidth < 0;
        float spacing = fits ? 20f : (handWidth - cardCount * cardWidth) / cardCount;
        float startX = (handWidth - (totalCardsWidth + spacing * (cardCount - 1))) / 2f;

        for (int i = 0; i < cardCount; i++)
        {
            Transform card = transform.GetChild(i);
            float xPosition = startX + i * (cardWidth + spacing);
            card.localPosition = new Vector3(xPosition, 0, (-i - 1) * 10);
        }
    }
}
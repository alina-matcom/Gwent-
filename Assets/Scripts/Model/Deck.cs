using UnityEngine;
using System.Collections.Generic;
using System;

[CreateAssetMenu(fileName = "NewDeck", menuName = "Deck")]
public class Deck : ScriptableObject
{
    public List<CardOld> originalCards = new List<CardOld>(); 
    public List<CardOld> cards = new List<CardOld>();

    public void Reset()
    {
        cards.Clear();
        foreach (CardOld card in originalCards)
        {
            card.Reset();
            cards.Add(card);
        }
    }

    public CardOld DrawRandomCard()
    {
        if (cards.Count == 0)
        {
            return null;
        }

        int randomIndex = UnityEngine.Random.Range(0, cards.Count);
        CardOld drawnCard = cards[randomIndex];
        cards.RemoveAt(randomIndex);

        return drawnCard;
    }
}
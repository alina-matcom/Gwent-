using System.Collections;
using UnityEngine;

public class DeckController : MonoBehaviour
{
    public HandManager handManager;
    public Deck deck;

    void Start()
    {
        deck.Reset();
    }

    public IEnumerator DrawCoroutine(int amount)
{
    for (int i = 0; i < amount; i++)
    {
        CardOld drawnCard = deck.DrawRandomCard();
        if (!drawnCard) yield break;
        handManager.AddCard(drawnCard);
        yield return new WaitForSeconds(0.1f); // Ajusta este valor según sea necesario
    }
}
}

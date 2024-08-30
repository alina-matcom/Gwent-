using UnityEngine;

public class CardDetails : Singleton<CardDetails>
{
    public CardDisplay cardDisplay;

    public void Show(CardOld card)
{
    if (card == null)
    {
        Debug.LogError("Show was called with a null Card.");
        return;
    }

    cardDisplay.SetCard(card);
    cardDisplay.gameObject.SetActive(true);
}
}

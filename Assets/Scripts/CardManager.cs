public class CardManager : Singleton<CardManager>
{
    public CardDisplay selectedCard;

    public void Show(CardDisplay card)
    {
        selectedCard = card;
    }
}

using System.Collections;
using GwentInterpreters;
using UnityEngine;
using UnityEngine.UI;

public class CardDisplay : MonoBehaviour
{
    public CardOld card;
    public Text nameText;
    public Text descText;
    public PowerDisplay powerDisplay;
    public Image unitTypeImage;
    public Image kindBorderImage;
    public Image kindBannerImage;
    public Image cardImage;

    protected void Start()
    {
        if (card != null)
        {
            SetCard(card);
        }
    }

    public void SetCard(CardOld newCard)
    {
        if (newCard == null)
        {
            Debug.LogError("SetCard was called with a null Card.");
            return; // Retorna aquí para evitar intentar acceder a propiedades de un objeto null
        }

        card = newCard;

        Debug.Log("Configurando carta: " + card.name);

        kindBorderImage.sprite = Resources.Load<Sprite>("card-border-" + card.kind.ToString().ToLower());

        string borderPath = "card-border-" + card.kind.ToString().ToLower();
        Debug.Log("Loading border sprite from path: " + borderPath); // Registra la ruta intentada
        kindBorderImage.sprite = Resources.Load<Sprite>(borderPath);

        string bannerPath = card.kind.ToString().ToLower();
        Debug.Log("Loading banner sprite from path: " + bannerPath); // Registra la ruta intentada
        kindBannerImage.sprite = Resources.Load<Sprite>(bannerPath);

        string cardImagePath = "card-images/" + card.Image;
        Debug.Log("Loading card image from path: " + cardImagePath); // Registra la ruta intentada
        cardImage.sprite = Resources.Load<Sprite>(cardImagePath);

        if (card is UnitCard unitCard)
        {
            unitTypeImage.sprite = Resources.Load<Sprite>(unitCard.type.ToString().ToLower());
            powerDisplay.SetPower(unitCard.power);
            powerDisplay.gameObject.SetActive(true);
        }
        else if (card is Card cardInstance)
        {
            powerDisplay.SetPower(cardInstance.Power);
            powerDisplay.gameObject.SetActive(true);
        }
        else
        {
            if (card is FieldCard)
            {
                unitTypeImage.sprite = Resources.Load<Sprite>("field");
            }
            else if (card is BuffCard)
            {
                unitTypeImage.sprite = Resources.Load<Sprite>("buff");
            }
            powerDisplay.gameObject.SetActive(false);
        }
    }

    public void SetPower(int power)
    {
        if (card.kind == CardKind.Gold) return;
        if (card is UnitCard unitCard)
        {
            Debug.Log("Actualizando poder de la carta: " + unitCard.name + " a " + power);
            unitCard.power = power;
            powerDisplay.SetPower(power);
        }
        else if (card is Card cardInstance)
        {
            Debug.Log("Actualizando poder de la carta: " + cardInstance.name + " a " + power);
            cardInstance.Power = power;
            powerDisplay.SetPower(power);
        }
    }
}

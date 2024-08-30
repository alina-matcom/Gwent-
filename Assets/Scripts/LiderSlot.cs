using UnityEngine;

public class LiderSlot : Slot
{
    public CardDisplay liderCardDisplay;
    public Zone targetZone; // Añade esta línea para especificar la zona de ataque

    public override void Start() { }
    public override void OnMouseDown()
    {
        _highlight.SetActive(true);

        if (liderCardDisplay != null && liderCardDisplay.card is LiderCard liderCard)
        {
            liderCard.ApplyEffect(targetZone); // Pasa la zona de ataque al método ApplyEffect
        }
    }

    public void OnMouseUp()
    {
        _highlight.SetActive(false);
    }
}
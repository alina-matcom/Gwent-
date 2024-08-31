using System;
using System.Collections;
using System.Collections.Generic;
using GwentInterpreters;
using Unity.VisualScripting;
using UnityEngine;

public class Zone : MonoBehaviour
{
    public double rowPower;
    public ZoneSlot unitsSlot;
    public BuffSlot buffSlot;
    public PowerDisplay powerDisplay;
    public UnitType rowType;
    private bool hasEffectBeenApplied = false;

    public void Start()
    {
        powerDisplay.SetPower(0);
    }

    public virtual void PlayCard(CardOld card)
    {
        if (card is UnitCard unitCard)
        {
            unitsSlot.PlayCard(unitCard);
        }
        else if (card is Card newCard)
        {
            unitsSlot.PlayCard(newCard);
        }
        else
        {
            buffSlot.PlayCard(card);
        }
    }

    public double GetRowPower()
    {
        return rowPower;
    }

    public void UpdateRowPower()
    {
        if (!hasEffectBeenApplied && buffSlot.card)
        {
            unitsSlot.ApplyEffect(
                PowerModifier.Increment,
                (buffSlot.cardDisplay.card as BuffCard).powerIncrease
            );

            hasEffectBeenApplied = true;
        }

        rowPower = unitsSlot.GetRowPower();
        powerDisplay.SetPower(rowPower);
    }

    public void ResetRowPower()
    {
        rowPower = 0;
        powerDisplay.SetPower(rowPower);
        hasEffectBeenApplied = false; // Asegúrate de restablecer el estado del efecto aplicado
    }
}

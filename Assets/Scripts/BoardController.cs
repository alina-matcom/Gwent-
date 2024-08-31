using System;
using GwentInterpreters;
using UnityEngine;

public class BoardController : Singleton<BoardController>
{
    public Zone playerMeleeZone;
    public Zone playerRangedZone;
    public Zone playerSiegeZone;
    public Zone enemyMeleeZone;
    public Zone enemyRangedZone;
    public Zone enemySiegeZone;
    public FieldZone fieldZone;
    public PowerDisplay playerScore;
    public PowerDisplay enemyScore;
    public TurnIndicator playerTurnIndicator;
    public TurnIndicator enemyTurnIndicator;

    public static event System.Action OnScoreUpdateNeeded;

    public void Start()
    {
        playerScore.SetPower(0);
        enemyScore.SetPower(0);
        OnScoreUpdateNeeded += UpdateScore;
    }

    public static void TriggerScoreUpdateNeeded()
    {
        OnScoreUpdateNeeded?.Invoke();
    }

    public void PlayCard(CardOld card, Slot slot, int turn)
{
    if (card is UnitCard unitCard)
    {
        switch (unitCard.type)
        {
            case UnitType.Melee:
                (turn == 0 ? playerMeleeZone : enemyMeleeZone).PlayCard(unitCard);
                break;

            case UnitType.Ranged:
                (turn == 0 ? playerRangedZone : enemyRangedZone).PlayCard(unitCard);
                break;

            case UnitType.Siege:
                (turn == 0 ? playerSiegeZone : enemySiegeZone).PlayCard(unitCard);
                break;
        }
    }
    else if (card is FieldCard)
    {
        if (slot is FieldZone)
        {
            slot.PlayCard(card);
        }
        else
        {
            Debug.Log("FieldCard can only be played in FieldZone.");
        }
    }
    else if (card is Card newCard)
    {
        switch (newCard.GetBoardSlot())
        {
            case BoardSlot.MeleeZone:
                (turn == 0 ? playerMeleeZone : enemyMeleeZone).PlayCard(newCard);
                break;

            case BoardSlot.RangedZone:
                (turn == 0 ? playerRangedZone : enemyRangedZone).PlayCard(newCard);
                break;

            case BoardSlot.SiegeZone:
                (turn == 0 ? playerSiegeZone : enemySiegeZone).PlayCard(newCard);
                break;
        }
    }
    else if (slot is BuffSlot)
    {
        slot.PlayCard(card);
    }

    UpdateScore();
}

    public void UpdateZonesPower()
    {
        playerMeleeZone.UpdateRowPower();
        playerRangedZone.UpdateRowPower();
        playerSiegeZone.UpdateRowPower();
        enemyMeleeZone.UpdateRowPower();
        enemyRangedZone.UpdateRowPower();
        enemySiegeZone.UpdateRowPower();
    }

    public void UpdateScore()
    {
        UpdateZonesPower();
        playerScore.SetPower(GetPlayerScore());
        enemyScore.SetPower(GetEnemyScore());
    }

    public double GetPlayerScore()
    {
        return
            playerMeleeZone.GetRowPower() +
            playerRangedZone.GetRowPower() +
            playerSiegeZone.GetRowPower();
    }

    public double  GetEnemyScore()
    {
        return
            enemyMeleeZone.GetRowPower() +
            enemyRangedZone.GetRowPower() +
            enemySiegeZone.GetRowPower();
    }

    public void UpdateTurnIndicator(int turn)
    {
        playerTurnIndicator.Show(turn == 0);
        enemyTurnIndicator.Show(turn == 1);
    }

    public void SendAllCardsToGraveyard()
    {
        foreach (Zone zone in new[] { playerMeleeZone, playerRangedZone, playerSiegeZone, enemyMeleeZone, enemyRangedZone, enemySiegeZone })
        {
            foreach (CardDisplay card in zone.unitsSlot.GetCards())
            {
                Destroy(card.gameObject);
            }
        }
    }

    public void ResetPlayerPowers()
    {
        playerScore.SetPower(0);
        enemyScore.SetPower(0);

        playerMeleeZone.ResetRowPower();
        playerRangedZone.ResetRowPower();
        playerSiegeZone.ResetRowPower();
        enemyMeleeZone.ResetRowPower();
        enemyRangedZone.ResetRowPower();
        enemySiegeZone.ResetRowPower();
    }
}
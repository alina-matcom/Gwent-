using UnityEngine;
using GwentInterpreters;
using System.Collections.Generic;
using System.IO;
using System.Collections;

public class GameController : Singleton<GameController>
{
    public HandManager playerHandManager;
    public DeckController playerDeck;
    public HandManager enemyHandManager;
    public DeckController enemyDeck;

    public int playerWinnedRounds = 0;
    public int enemyWinnedRounds = 0;
    public int round = 0;
    public int currentTurn = 0;
    public bool playerPassed = false;
    public bool enemyPassed = false;

    public delegate void HighlightHandler(BoardSlot slot, int player);
    public static event HighlightHandler OnHighlight;
    public CardDisplay playerLiderCardDisplay;
    public CardDisplay enemyLiderCardDisplay;

    public UIManager uiManager; // Añade una referencia a UIManager

    void Start()
    {
        // Cargar cartas desde el archivo DSL
        string dslFilePath = @"C:\Gwent ++\Gwent-test\Assets\StreamingAssets\deck.dsl";
        // Leer y procesar el contenido del archivo
        List<CardOld> cards = LoadCardsFromDSL(dslFilePath);

        // Asignar las cartas a los mazos de los jugadores
        playerDeck.deck = ScriptableObject.CreateInstance<Deck>();
        enemyDeck.deck = ScriptableObject.CreateInstance<Deck>();

        playerDeck.deck.originalCards = new List<CardOld>(cards);
        enemyDeck.deck.originalCards = new List<CardOld>(cards);

        playerDeck.deck.Reset();
        enemyDeck.deck.Reset();

        if (playerLiderCardDisplay.card is LiderCard playerLiderCard)
        {
            playerLiderCard.ResetCharges();
        }
        if (enemyLiderCardDisplay.card is LiderCard enemyLiderCard)
        {
            enemyLiderCard.ResetCharges();
        }

        HandCardDisplay.OnPlayCard += PlayCard;
        Slot.OnSlotSelected += PlaceCard;
        StartCoroutine(DrawCardsAndCheckHands());
        BoardController.Instance.UpdateTurnIndicator(currentTurn);

    }
    private IEnumerator DrawCardsAndCheckHands()
    {
        // Iniciar las corrutinas de dibujo de cartas
        yield return StartCoroutine(playerDeck.DrawCoroutine(10));
        yield return StartCoroutine(enemyDeck.DrawCoroutine(10));
    }

    private List<CardOld> LoadCardsFromDSL(string filePath)
    {

        // Leer el contenido del archivo
        string dslCode = File.ReadAllText(filePath);
        // Parsear el código DSL
        Parser parser = new Parser(new Scanner(dslCode).ScanTokens());
        List<Stmt> statements = parser.Parse();

        // Crear una instancia del intérprete
        Interpreter interpreter = new Interpreter();

        // Interpretar las declaraciones parseadas
        List<CardOld> cards = interpreter.Interpret(statements);

        return cards;
    }
    public void OnDestroy()
    {
        HandCardDisplay.OnPlayCard -= PlayCard;
    }

    public void PlayCard(CardDisplay card)
    {
        if (CardManager.Instance.selectedCard == card)
        {
            return;
        }
        CardManager.Instance.selectedCard = card;
        if (CardManager.Instance.selectedCard != null)
        {
            OnHighlight?.Invoke(card.card.GetBoardSlot(), currentTurn);
        }
        else
        {
            Debug.LogError("No se ha seleccionado una carta válida.");
        }
    }

    public void PlaceCard(Slot slot)
    {
        if (!CardManager.Instance.selectedCard) return;

        BoardController.Instance.PlayCard(CardManager.Instance.selectedCard.card, slot, currentTurn);
        playerHandManager.RemoveCard(CardManager.Instance.selectedCard);
        CardManager.Instance.selectedCard = null;
        OnHighlight?.Invoke(BoardSlot.None, currentTurn);
        NextTurn();
    }

    public bool CheckRoundWinner()
    {
        int playerScore = BoardController.Instance.GetPlayerScore();
        int enemyScore = BoardController.Instance.GetEnemyScore();

        if (playerPassed && enemyPassed)
        {
            if (playerScore > enemyScore)
            {
                if (++playerWinnedRounds < 2)
                {
                    DeclareRoundWinner(0);
                    NextRound(0);
                }
                else
                {
                    DeclareGameWinner(0);
                }
            }
            else if (enemyScore > playerScore)
            {
                if (++enemyWinnedRounds < 2)
                {
                    DeclareRoundWinner(1);
                    NextRound(1);
                }
                else
                {
                    DeclareGameWinner(1);
                }
            }
            else
            {
                playerWinnedRounds++;
                enemyWinnedRounds++;
            }

            return true;
        }

        return false;
    }

    public void Forfeit()
    {
        if (currentTurn == 0) playerPassed = true;
        else enemyPassed = true;
        NextTurn();
    }

    public void NextTurn()
    {
        if (CheckRoundWinner()) return;

        if (playerPassed && currentTurn == 1) return;
        if (enemyPassed && currentTurn == 0) return;

        currentTurn = (currentTurn == 0) ? 1 : 0;

        playerHandManager.Hide(currentTurn == 1);
        enemyHandManager.Hide(currentTurn == 0);
        BoardController.Instance.UpdateTurnIndicator(currentTurn);
    }

    public void NextRound(int winner)
    {
        round++;
        currentTurn = winner;

        // Enviar todas las cartas al cementerio y reiniciar el poder
        BoardController.Instance.SendAllCardsToGraveyard();
        BoardController.Instance.ResetPlayerPowers();

        // Reiniciar las cargas de las cartas líder
        if (playerLiderCardDisplay.card is LiderCard playerLiderCard)
        {
            playerLiderCard.ResetCharges();
        }
        if (enemyLiderCardDisplay.card is LiderCard enemyLiderCard)
        {
            enemyLiderCard.ResetCharges();
        }

        playerDeck.DrawCoroutine(2);
        enemyDeck.DrawCoroutine(2);
        // Asegurarse de que el turno se maneje correctamente
        NextTurn();
    }

    public void DeclareRoundWinner(int winner)
    {
        Debug.Log("Player " + winner + " won this round!");
        StartCoroutine(uiManager.ShowWinnerMessage($"Player {winner} won this round!"));
    }

    public void DeclareGameWinner(int winner)
    {
        Debug.Log("Player " + winner + " wins!!");
        StartCoroutine(uiManager.ShowWinnerMessage($"Player {winner} wins the game!"));
    }
}

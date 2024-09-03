using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;

namespace GwentInterpreters
{
    public class Card : CardOld
    {
        public string Type { get; set; }
        public double Power { get; set; }
        public List<string> Range { get; set; }
        public List<EffectActionResult> OnActivation { get; set; }  // Cambiado de EffectAction a EffectActionResult

        private readonly double powerOriginal;

        public Card(string type, string name, string faction, double power, List<string> range, List<EffectActionResult> onActivation, int owner)
        {
            Type = type;
            Name = name;  // Asignar el valor del parámetro name a la propiedad name de CardOld
            Faction = faction;
            Power = power;
            Range = range;
            OnActivation = onActivation;
            Owner = owner;  // Asignar el valor del parámetro owner a la propiedad owner de CardOld

            // Inicializar las propiedades heredadas con valores por defecto
            description = "Carta creada por mi compilador";

            // Inicializar kind basado en el valor de Type
            switch (type)
            {
                case "Oro":
                    kind = CardKind.Gold;
                    break;
                case "Plata":
                    kind = CardKind.Silver;
                    break;
                default:
                    kind = CardKind.Bronze;
                    break;
            }

            // Inicializar Image con el valor por defecto "gwent"
            Image = "gwent";

            // Inicializar powerOriginal con el valor de power
            powerOriginal = power;
        }

        public override void Reset()
        {
            // Restaurar el valor de Power a su valor original
            Power = powerOriginal;
        }

        public override BoardSlot GetBoardSlot()
        {
            // Implementar GetBoardSlot basado en el rango de la carta
            if (Range.Contains("Melee"))
            {
                return BoardSlot.MeleeZone;
            }
            else if (Range.Contains("Ranged"))
            {
                return BoardSlot.RangedZone;
            }
            else if (Range.Contains("Siege"))
            {
                return BoardSlot.SiegeZone;
            }
            else
            {
                return BoardSlot.None;
            }
        }
        public void ApplyEffects()
        {
            foreach (var effectActionResult in OnActivation)
            {
                ApplyEffectActionResult(effectActionResult, null);
            }
        }
        private void ApplyEffectActionResult(EffectActionResult effectActionResult, Iterable parentTargets)
        {
            // Obtener las cartas objetivo usando el selector
            Iterable targets = GetCardsFromSelectorResult(effectActionResult.SelectorResult, parentTargets);

            // Invocar el efecto
            effectActionResult.EffectInstance.Invoke(new Interpreter(), targets, new Context());

            // Si hay un post-action, aplicarlo recursivamente
            if (effectActionResult.PostActionResult != null)
            {
                ApplyEffectActionResult(effectActionResult.PostActionResult, targets);
            }
        }

        private Iterable GetCardsFromSelectorResult(SelectorResult selectorResult, Iterable parentTargets)
        {
            if (selectorResult == null)
            {
                throw new ArgumentNullException(nameof(selectorResult), "El selectorResult no puede ser null.");
            }

            if (selectorResult.Source == null)
            {
                throw new ArgumentNullException(nameof(selectorResult.Source), "El Source del selectorResult no puede ser null.");
            }

            if (selectorResult.Predicate == null)
            {
                throw new ArgumentNullException(nameof(selectorResult.Predicate), "El Predicate del selectorResult no puede ser null.");
            }
            // Obtener la fuente de cartas según el Source del SelectorResult
            Iterable cardsSource;
            switch (selectorResult.Source)
            {
                case "hand":
                    cardsSource = Context.HandOfPlayer(0);
                    break;
                case "otherHand":
                    cardsSource = Context.HandOfPlayer(1);
                    break;
                case "deck":
                    cardsSource = Context.DeckOfPlayer(0);
                    break;
                case "otherDeck":
                    cardsSource = Context.DeckOfPlayer(1);
                    break;
                case "field":
                    cardsSource = Context.FieldOfPlayer(0);
                    break;
                case "otherField":
                    cardsSource = Context.FieldOfPlayer(1);
                    break;
                case "parent":
                    cardsSource = parentTargets;
                    break;
                case "board":
                    cardsSource = Context.Board;
                    break;
                default:
                    throw new ArgumentException($"Source '{selectorResult.Source}' no es válido.");
            }

            // Aplicar el predicado para filtrar las cartas
            Iterable filteredCards = cardsSource.Find(selectorResult.Predicate);


            // Si Single es true, devolver solo una lista con el primer elemento que cumpla el predicado
            if (selectorResult.Single && filteredCards.Count > 0)
            {
                return new Iterable(new List<CardOld> { filteredCards[0] });
            }

            // Devolver la lista completa de cartas que cumplan el predicado
            return filteredCards;
        }

    public override CardOld Clone()
    {
        // Clonar superficialmente las propiedades de CardOld
        Card clonedCard = (Card)base.Clone();

        // Clonar profundamente las propiedades específicas de Card
        clonedCard.Range = new List<string>(Range);

        // OnActivation se comparte entre las instancias clonadas
        clonedCard.OnActivation = OnActivation;

        return clonedCard;
    }
    public override string ToString()
    {
        return $"Card: {name}, Type: {Type}, Faction: {Faction}, Power: {Power}, Range: [{string.Join(", ", Range)}], Owner: {Owner}";
    }
}

public class Context
{
    public static int TriggerPlayer
    {
        get { return GameController.Instance.TriggerPlayer; }
    }

    public static Iterable Board
    {
        get { return new Iterable(GameController.Instance.Board); }
    }

    public static Iterable HandOfPlayer(int player)
    {
        return new Iterable(GameController.Instance.HandOfPlayer(player));
    }

    public static Iterable FieldOfPlayer(int player)
    {
        return new Iterable(GameController.Instance.FieldOfPlayer(player));
    }

    public static Iterable GraveyardOfPlayer(int player)
    {
        return new Iterable(GameController.Instance.GraveyardOfPlayer(player));
    }

    public static Iterable DeckOfPlayer(int player)
    {
        return new Iterable(GameController.Instance.DeckOfPlayer(player));
    }
}
public class Iterable : IList<CardOld>
{
    private List<CardOld> cards;

    public Iterable()
    {
        cards = new List<CardOld>();
    }
    // Nuevo constructor que recibe una lista de cartas
    public Iterable(List<CardOld> initialCards)
    {
        cards = new List<CardOld>(initialCards);
    }

    // Implementación de IList<Card>
    public CardOld this[int index] { get => cards[index]; set => cards[index] = value; }
    public int Count => cards.Count;
    public bool IsReadOnly => false;

    public void Add(CardOld card) => cards.Add(card);
    public void Clear() => cards.Clear();
    public bool Contains(CardOld card) => cards.Contains(card);
    public void CopyTo(CardOld[] array, int arrayIndex) => cards.CopyTo(array, arrayIndex);
    public IEnumerator<CardOld> GetEnumerator() => cards.GetEnumerator();
    public int IndexOf(CardOld card) => cards.IndexOf(card);
    public void Insert(int index, CardOld card) => cards.Insert(index, card);
    public bool Remove(CardOld card) => cards.Remove(card);
    public void RemoveAt(int index) => cards.RemoveAt(index);
    IEnumerator IEnumerable.GetEnumerator() => cards.GetEnumerator();

    // Métodos adicionales
    public Iterable Find(Func<CardOld, bool> predicate)
    {
        if (predicate == null)
        {
            throw new ArgumentNullException(nameof(predicate), "El predicado no puede ser null.");
        }

        return new Iterable(cards.Where(predicate).ToList());
    }
    public void Push(CardOld card) => cards.Add(card);

    public void SendBottom(CardOld card) => cards.Insert(0, card);

    public CardOld Pop()
    {
        if (cards.Count == 0)
            throw new InvalidOperationException("No hay cartas en la colección.");

        CardOld card = cards[cards.Count - 1];
        cards.RemoveAt(cards.Count - 1);
        return card;
    }

    public void Shuffle()
    {
        System.Random rng = new System.Random();
        int n = cards.Count;
        while (n > 1)
        {
            n--;
            int k = rng.Next(n + 1);
            CardOld value = cards[k];
            cards[k] = cards[n];
            cards[n] = value;
        }
    }
}

public class CallableMethod
{
    private readonly object _instance;
    private readonly MethodInfo _method;

    public CallableMethod(object instance, MethodInfo method)
    {
        _instance = instance;
        _method = method;
    }

    public bool CanInvoke(List<object> arguments, out string errorMessage)
    {
        var parameters = _method.GetParameters();
        if (parameters.Length != arguments.Count)
        {
            errorMessage = $"Se esperaban {parameters.Length} argumentos, pero se obtuvieron {arguments.Count}.";
            return false;
        }

        for (int i = 0; i < arguments.Count; i++)
        {
            var arg = arguments[i];
            var paramType = parameters[i].ParameterType;

            if (arg != null)
            {
                if (arg.GetType() == typeof(double) && IsNumericType(paramType))
                {
                    try
                    {
                        Convert.ChangeType(arg, paramType);
                    }
                    catch
                    {
                        errorMessage = $"El argumento {i + 1} no puede ser convertido al tipo {paramType.Name}.";
                        return false;
                    }
                }
                else if (!paramType.IsAssignableFrom(arg.GetType()))
                {
                    errorMessage = $"El argumento {i + 1} no puede ser convertido al tipo {paramType.Name}.";
                    return false;
                }
            }
        }

        errorMessage = null;
        return true;
    }

    private bool IsNumericType(Type type)
    {
        switch (Type.GetTypeCode(type))
        {
            case TypeCode.Byte:
            case TypeCode.Decimal:
            case TypeCode.Double:
            case TypeCode.Int16:
            case TypeCode.Int32:
            case TypeCode.Int64:
            case TypeCode.SByte:
            case TypeCode.Single:
            case TypeCode.UInt16:
            case TypeCode.UInt32:
            case TypeCode.UInt64:
                return true;
            default:
                return false;
        }
    }

    public object Call(List<object> arguments)
    {
        var parameters = _method.GetParameters();
        var convertedArgs = new object[arguments.Count];
        for (int i = 0; i < arguments.Count; i++)
        {
            var arg = arguments[i];
            var paramType = parameters[i].ParameterType;
            try
            {
                Debug.Log($"Intentando convertir el argumento '{arg}' al tipo '{paramType}'");

                // Verificar si el tipo del argumento es asignable al tipo del parámetro
                if (paramType.IsAssignableFrom(arg.GetType()))
                {
                    convertedArgs[i] = arg;
                }
                else
                {
                    convertedArgs[i] = Convert.ChangeType(arg, paramType);
                }
            }
            catch (InvalidCastException e)
            {
                Debug.LogError($"Error de conversión: No se puede convertir el argumento '{arg}' al tipo '{paramType}'. Detalles: {e.Message}");
            }
            catch (FormatException e)
            {
                Debug.LogError($"Error de formato: El argumento '{arg}' no tiene el formato correcto para el tipo '{paramType}'. Detalles: {e.Message}");
            }
            catch (ArgumentNullException e)
            {
                Debug.LogError($"Error de argumento nulo: El argumento es nulo. Detalles: {e.Message}");
            }
            catch (Exception e)
            {
                Debug.LogError($"Error inesperado: {e.Message}");
            }
        }

        return _method.Invoke(_instance, convertedArgs);
    }
}

}

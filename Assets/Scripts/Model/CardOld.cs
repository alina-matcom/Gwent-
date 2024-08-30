using System.Threading;
using UnityEngine;

public abstract class CardOld : ScriptableObject
{
    public new string name;
    public string description;
    public CardKind kind;
    public string Image;
    public string Faction;
    public int owner;
    public abstract void Reset();
    public abstract BoardSlot GetBoardSlot();
}

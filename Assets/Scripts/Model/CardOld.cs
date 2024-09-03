using System.Threading;
using UnityEngine;

public abstract class CardOld : ScriptableObject
{
    public string Name { get; set; }
    public string description;
    public CardKind kind;
    public string Image;
    public string Faction { get; set; }
    public int Owner { get; set; }
    public abstract void Reset();
    public abstract BoardSlot GetBoardSlot();
    public virtual CardOld Clone()
    {
        return (CardOld)MemberwiseClone();
    }
}

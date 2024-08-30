using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class Slot : MonoBehaviour
{
    [SerializeField] private GameObject _hover;
    [SerializeField] protected GameObject _highlight;
    public BoardSlot slotType;
    public int owner;

    public virtual void PlayCard(CardOld card) { }

    public delegate void SlotSelectedHandler(Slot slot);
    public static event SlotSelectedHandler OnSlotSelected;

    public virtual void Start()
    {
        GameController.OnHighlight += Highlight;
    }

    public virtual void OnMouseDown()
    {
        if (_highlight.activeInHierarchy)
        {
            OnSlotSelected?.Invoke(this);
        }
    }

    public virtual void Highlight(BoardSlot slot, int player)
    {
        _highlight.SetActive(slotType == slot && owner == player);
    }


    void OnMouseEnter()
    {
        _hover.SetActive(true);
    }

    void OnMouseExit()
    {
        _hover.SetActive(false);
    }
}

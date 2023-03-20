using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Drag : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
    public PlayerInventory _playerInventory;
    public Item _item;
    public string ownerItem;
    public int countItem;
    [SerializeField] private bool isQACell;
    [SerializeField] private bool isJournalCell;

    public Image image;
    public Sprite defaultSprite;
    public Text count;

    public Text descriptionCell;

    void IPointerEnterHandler.OnPointerEnter(PointerEventData eventData)
    {
    }

    void IPointerExitHandler.OnPointerExit(PointerEventData eventData)
    {
    }
    public void OnPointerClick(PointerEventData eventData)
    {
        if (ownerItem != "")
        {
            if (eventData.button == PointerEventData.InputButton.Right)
            {
                if (!isQACell)
                    _playerInventory.RemoveItem(this);
                else return;
            }
            else if (eventData.button == PointerEventData.InputButton.Left)
            {
                _playerInventory.UseItem(this);
            }
        }
    }
}

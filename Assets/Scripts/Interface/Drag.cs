using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Drag : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
    public PlayerInventory _playerInventory;
    public PlayerJournal _playerJournal;
    public Item _item;
    public string _ownerItem;
    public int _countItem;
    [SerializeField] private bool _isQACell;
    [SerializeField] private bool _isJournalCell;

    public Image _image;
    public Sprite _defaultSprite;
    public Text _nameItem;
    public Text _count;
    public Text _descriptionCell;

    void IPointerEnterHandler.OnPointerEnter(PointerEventData eventData)
    {
    }

    void IPointerExitHandler.OnPointerExit(PointerEventData eventData)
    {
    }
    public void OnPointerClick(PointerEventData eventData)
    {
        if (_ownerItem != "")
        {
            if (eventData.button == PointerEventData.InputButton.Right)
            {
                if (!_isQACell || !_isJournalCell)
                    _playerInventory.RemoveItem(this);
                else return;
            }
            else if (eventData.button == PointerEventData.InputButton.Left)
            {
                if(!_isJournalCell)
                    _playerInventory.UseItem(this);
            }
        }
    }
}

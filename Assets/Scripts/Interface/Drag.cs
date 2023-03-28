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
    [SerializeField] private bool _isNotRemovingCell;
    [SerializeField] private bool _isJourCell;

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
            if (eventData.button == PointerEventData.InputButton.Right && !_isNotRemovingCell)
            {
                _playerInventory.RemoveItem(this);
            }
            else if (eventData.button == PointerEventData.InputButton.Left && !_isJourCell)
            {
                _playerInventory.UseItem(this);
            }
        }
    }
}

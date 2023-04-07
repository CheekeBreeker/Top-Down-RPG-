using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Drag : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
    public PlayerInventory _playerInventory;
    public PlayerJournal _playerJournal;
    public Dialog _trading;
    public Item _item;
    public string _ownerItem;
    public int _countItem;
    [SerializeField] private bool _isNotRemovingCell;
    [SerializeField] private bool _isJourCell;
    [SerializeField] private bool _isSkillCell;

    public Image _image;
    public Sprite _defaultSprite;
    public Text _nameItem;
    public Text _count;
    public Text _descriptionCell;
    public GameObject _descriptionObj;
    public Text _descriptionItem;

    [Multiline(10)] public string _discriptionSkill;

    void IPointerEnterHandler.OnPointerEnter(PointerEventData eventData)
    {
        if (!_isJourCell && !_isSkillCell)
        {
            _image.color = new Color(200f / 255, 200f / 255, 200f / 255, 255f / 255);
            if ((_ownerItem != "myJourItem" || _ownerItem != "myWeapon" || _ownerItem != "TraderItem" || _ownerItem != "myItemInBarter") && !_isNotRemovingCell)
            {
                _descriptionObj.SetActive(true);
                _descriptionItem.text = _item._description;
            }
        }
        if (_isSkillCell)
        {
            _descriptionItem.text = _discriptionSkill;
        }
    }

    void IPointerExitHandler.OnPointerExit(PointerEventData eventData)
    {
        if (!_isJourCell && !_isSkillCell)
        {
            _image.color = new Color(255f / 255, 255f / 255, 255f / 255, 255f / 255);
            if ((_ownerItem != "myJourItem" || _ownerItem != "myWeapon" || _ownerItem != "TraderItem" || _ownerItem != "myItemInBarter") && !_isNotRemovingCell)
            {
                _descriptionObj.SetActive(false);
                _descriptionItem.text = "";
            }
        }
        if (_isSkillCell)
        {
            _descriptionItem.text = "";
        }
    }
    public void OnPointerClick(PointerEventData eventData)
    {
        if (_ownerItem != "")
        {
            if (eventData.button == PointerEventData.InputButton.Right && !_isNotRemovingCell)
            {
                if(_ownerItem == "myItem")
                    _playerInventory.RemoveItem(this);
            }
            else if (eventData.button == PointerEventData.InputButton.Left && !_isJourCell)
            {
                if ((_ownerItem == "myItem" || _ownerItem == "myWeapon") && !_playerInventory._characterStatus.isTrade)
                    _playerInventory.UseItem(this);
                else if (_ownerItem == "myItem" && _playerInventory._characterStatus.isTrade)
                    _playerInventory.PlayerToBarter(this);
                else if (_ownerItem == "TraderItem")
                    _trading.NpcToBarter(this);
            }
        }
    }
}

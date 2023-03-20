using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerInventory : MonoBehaviour
{
    public PlayerStats _playerStats;
    public CharacterStatus _characterStatus;
    public PlayerJournal _playerJournal;
    public InterfaceManager _interfaceManager;

    public List<Item> consumables = new List<Item>();
    public List<Item> weapon = new List<Item>();
    public List<Item> expItems = new List<Item>();
    public List<Drag> _drags = new List<Drag>();

    public int typeOutput;

    public Drag _mainWeapon;
    public GameObject _weaponInHand;

    public float _weight;
    public float _maxWeight;

    public GameObject _inventory;

    public GameObject _cell;
    public Transform _cellParent;

    public Transform _rightHand;

    private void Start()
    {
        typeOutput = 1;
        _playerStats = GetComponent<PlayerStats>();
        _playerJournal = GetComponent<PlayerJournal>();
    }

    private void Update()
    {
        InventoryActive();
    }

    public void InventoryActive()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            if (_inventory.activeSelf)
                InventoryDisable();
            else InventoryEnabled();
        }
    }

    public void InventoryDisable()
    {
        foreach (Drag drag in _drags)
            Destroy(drag.gameObject);
        _drags.Clear();
    }

    public void InventoryEnabled()
    {
        foreach (Drag drag in _drags)
            Destroy(drag.gameObject);
        _drags.Clear();

        for (int i = 0; i < consumables.Count + weapon.Count; i++)
        {
            GameObject newCell = Instantiate(_cell);
            newCell.transform.SetParent(_cellParent, false);
            _drags.Add(newCell.GetComponent<Drag>());
        }

        if(typeOutput == 1)
        {
            OutputType(consumables);
        }
        else if (typeOutput == 2)
        {
            OutputType(weapon);
        }
        else if (typeOutput == 3)
        {
            OutputType(expItems);
        }

        for (int i = _drags.Count - 1; i >= 0; i--)
        {
            if (_drags[i].ownerItem == "")
            {
                Destroy(_drags[i].gameObject);
                _drags.RemoveAt(i);
            }
        }
    }

    private void OutputType(List<Item> type)
    {
        for (int i = 0; i < type.Count; i++)
        {
            Item it = type[i];

            for (int j = 0; j < _drags.Count; j++)
            {
                if (_drags[j].ownerItem != "")
                {
                    if (type[i].isStackable)
                    {
                        if (_drags[j]._item.nameItem == it.nameItem)
                        {
                            _drags[j].countItem++;
                            _drags[j].count.text = _drags[j].countItem.ToString();
                            break;
                        }
                    }
                    else continue;

                }
                else
                {
                    _drags[j]._item = it;
                    _drags[j].image.sprite = Resources.Load<Sprite>(type[i].pathSprite);
                    _drags[j].ownerItem = "myItem";
                    _drags[j].countItem++;
                    _drags[j].count.text = "" + _drags[j].countItem;
                    _drags[j]._playerInventory = this;
                    break;
                }
            }
        }
    }

    public void OutputConsumables()
    {
        typeOutput = 1;
        InventoryEnabled();
    }

    public void OutputWeapon()
    {
        typeOutput = 2;
        InventoryEnabled(); 
    }

    public void OutputExpItems()
    {
        typeOutput = 3;
        InventoryEnabled();
    }

    public void RemoveItem(Drag drag)
    {
        Item it = drag._item;
        GameObject newObj = Instantiate<GameObject>(Resources.Load<GameObject>(it.pathPrefab));
        newObj.transform.position = transform.position + transform.forward + transform.up;
        _weight -= it.mass;
        _interfaceManager.WeightInterface();
        consumables.Remove(it);
        weapon.Remove(it);
        InventoryEnabled();
    }

    public void UseItem(Drag drag)
    {
        Item it = drag._item;

        if (it.typeItem == "Consumables")
        {
            _playerStats.AddHealth(drag._item.addHealth);
            if(drag._item.addExp > 0)
                _playerStats.AddExp(drag._item.addExp);
            consumables.Remove(drag._item);
        }
        else if (it.typeItem == "Weapon")
        {
            if (drag.ownerItem == "myItem" && _weaponInHand == null)
            {
                GameObject weaponObj = Instantiate<GameObject>(Resources.Load<GameObject>(it.pathPrefab));
                weaponObj.transform.SetParent(_rightHand);
                weaponObj.transform.localPosition = it._posWeapAttack;
                weaponObj.transform.localRotation = Quaternion.Euler(it._rotWeapAttack);

                weaponObj.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
                weaponObj.GetComponent<BoxCollider>().isTrigger = true;
                _weaponInHand = weaponObj;

                _mainWeapon._item = it;
                _mainWeapon.image.sprite = Resources.Load<Sprite>(it.pathSprite);
                _mainWeapon.ownerItem = "myWeapon";
                _mainWeapon.countItem++;
                _mainWeapon.count.text = "";
                _mainWeapon._playerInventory = this;

                weapon.Remove(it);
            }
            else if (drag.ownerItem == "myWeapon")
            {
                weapon.Add(drag._item);

                Destroy(_weaponInHand);
                _weaponInHand = null;

                _mainWeapon._item = null;
                _mainWeapon.image.sprite = _mainWeapon.defaultSprite;
                _mainWeapon.ownerItem = "";
                _mainWeapon.countItem = 0;
                _mainWeapon.count.text = "";
                _mainWeapon._playerInventory = null;
            }
            else return;
        }
        else if (it.typeItem == "ExpItems")
        {
            _playerStats.AddExp(drag._item.addExp);
            _playerJournal.AddItem(drag, it);
            expItems.Remove(drag._item);
        }
        InventoryEnabled();
    }
}

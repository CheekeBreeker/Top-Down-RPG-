using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerInventory : MonoBehaviour
{
    public PlayerStats _playerStats;
    public CharacterStatus _characterStatus;
    public PlayerJournal _playerJournal;
    public Dialog _trading;
    public InterfaceManager _interfaceManager;

    public List<Item> consumables = new List<Item>();
    public List<Item> weapon = new List<Item>();
    public List<Item> expItems = new List<Item>();
    public List<Drag> _drags = new List<Drag>();

    public int typeOutput;

    public Drag _mainWeapon;
    public GameObject _weaponInHand;
    public string _weaponInHandID;

    public float _weight;
    public float _maxWeight;

    public GameObject _inventory;

    public GameObject _cell;
    public Transform _cellParent;

    public Transform _rightHand;

    public GameObject _descriptionObj;
    public Text _descriptionItem;

    private bool _isInvActive;

    public List<string> _consItemsIDs;
    public List<string> _weapItemsIDs;
    public List<string> _expItemsIDs;

    private void Start()
    {
        typeOutput = 1;
        _playerStats = GetComponent<PlayerStats>();
        _playerJournal = GetComponent<PlayerJournal>();
        _trading = GetComponent<Dialog>();
    }

    private void Update()
    {
        InventoryActive();
    }

    public void InventoryActive()
    {
        if (Input.GetKeyDown(KeyCode.I) && !_characterStatus.isTrade)
        {
            if (!_isInvActive)
                InventoryDisable();
            else InventoryEnabled();
        }
    }

    public void InventoryDisable()
    {
        foreach (Drag drag in _drags)
            Destroy(drag.gameObject);
        _drags.Clear();

        _isInvActive = false;
        _descriptionObj.SetActive(false);
    }

    public void InventoryEnabled()
    {
        _isInvActive = true;
        foreach (Drag drag in _drags)
            Destroy(drag.gameObject);
        _drags.Clear();

        for (int i = 0; i < consumables.Count + weapon.Count + expItems.Count; i++)
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
            if (_drags[i]._ownerItem == "")
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
                if (_drags[j]._ownerItem != "")
                {
                    if (type[i].isStackable)
                    {
                        if (_drags[j]._item.nameItem == it.nameItem)
                        {
                            _drags[j]._countItem++;
                            _drags[j]._count.text = _drags[j]._countItem.ToString();
                            break;
                        }
                    }
                    else continue;

                }
                else
                {
                    _drags[j]._item = it;
                    _drags[j]._image.sprite = Resources.Load<Sprite>(type[i].pathSprite);
                    _drags[j]._ownerItem = "myItem";
                    _drags[j]._countItem++;
                    _drags[j]._count.text = "" + _drags[j]._countItem;
                    _drags[j]._playerInventory = this;
                    _drags[j]._descriptionObj = _descriptionObj;
                    _drags[j]._descriptionItem = _descriptionItem;
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
        //GameObject newObj = Instantiate<GameObject>(Resources.Load<GameObject>(it.pathPrefab));
        it.transform.gameObject.SetActive(true);
        it.transform.position = transform.position + transform.forward + transform.up;
        _weight -= it.mass;
        _interfaceManager.WeightInterface();
        consumables.Remove(it);
        weapon.Remove(it);
        expItems.Remove(it);
        _consItemsIDs.Remove(it._itemID);
        _weapItemsIDs.Remove(it._itemID);
        _expItemsIDs.Remove(it._itemID);
        InventoryEnabled();
    }

    public void UseItem(Drag drag)
    {
        Item it = drag._item;

        if (it.typeItem == "Consumables")
        {
            if (it.isWendingFluid)
            {
                if (GetComponent<LevelUpgrade>()._isHaveWelderSkill)
                    _playerStats.WendingSkillhpRegeneration(drag._item.addHealth, 1);
                return;
            }
            else if (it.isJellyPlate)
            {
                _playerStats.AddMaxHealth(it.addHealth, it.timeToWork);
            }
            else if(it.isDieselFuel)
            {
                _playerStats.AddDamage(it.addHealth, it.timeToWork);
            }
            else if (it.isMetalPlate)
            {
                _playerStats.AddBlockHP(it.addBlock);
                _playerStats.AddHealth(it.addHealth);
            }
            else if (it.isAntiShockFiber)
            {
                _playerStats.AddBlockHP(it.addBlock);
                _playerStats.AddHealth(it.addHealth);
            }
            else if (it.isImprovedProcessor)
            {
                _playerStats.BoostSpeed(it.addHealth, it.addHealth / 5, it.timeToWork);
            }

            _weight -= it.mass;
            consumables.Remove(drag._item);
            _consItemsIDs.Remove(it._itemID);
            _descriptionObj.SetActive(false);
        }
        else if (it.typeItem == "Weapon")
        {
            if (drag._ownerItem == "myItem" && _weaponInHand == null)
            {
                TakeWeapon(it);
            }
            else if (drag._ownerItem == "myWeapon")
            {
                weapon.Add(drag._item);
                _weaponInHand.GetComponent<Item>()._owner = "Player";
                _weapItemsIDs.Add(_weaponInHand.GetComponent<Item>()._itemID);

                Destroy(_weaponInHand);
                _weaponInHand = null;

                _mainWeapon._item = null;
                _mainWeapon._image.sprite = _mainWeapon._defaultSprite;
                _mainWeapon._ownerItem = "";
                _mainWeapon._countItem = 0;
                _mainWeapon._count.text = "";
                _mainWeapon._playerInventory = null;

                _weaponInHandID = null;

                _weight += it.mass / 2;
            }
            else return;
        }
        else if (it.typeItem == "ExpItems")
        {
            for (var i = 0; i < _playerJournal._expItem.Count; i++)
            {
                Item expItem = _playerJournal._expItem[i];
                if (expItem.nameItem == drag._item.nameItem)
                {
                    foreach (var drags in _playerJournal._drags)
                    {
                        //if (int.Parse(drags._count.text) >= drag._item.maxCountExpItems)
                        if(drags._countItem >= drag._item.maxCountExpItems)
                        {
                            drag._item.addExp /= 2;
                            break;
                        }
                    }
                    break;
                }
            }
            _playerStats.AddExp(drag._item.addExp);
            _playerJournal.AddItem(drag, it);

            _weight -= it.mass;
            expItems.Remove(drag._item);
            _playerJournal._itemsIDs.Add(it._itemID);
            _expItemsIDs.Remove(it._itemID);
            _descriptionObj.SetActive(false);
        }
        InventoryEnabled();
    }

    public void TakeWeapon(Item it)
    {
        GameObject weaponObj = Instantiate<GameObject>(Resources.Load<GameObject>(it.pathPrefab));
        weaponObj.transform.SetParent(_rightHand);
        weaponObj.transform.localPosition = it._posWeapAttack;
        weaponObj.transform.localRotation = Quaternion.Euler(it._rotWeapAttack);

        weaponObj.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
        weaponObj.GetComponent<BoxCollider>().isTrigger = true;
        _weaponInHand = weaponObj;

        _mainWeapon._item = it;
        _weaponInHand.GetComponent<Item>()._owner = "Player";
        _mainWeapon._image.sprite = Resources.Load<Sprite>(it.pathSprite);
        _mainWeapon._ownerItem = "myWeapon";
        _mainWeapon._countItem++;
        _mainWeapon._count.text = "";
        _mainWeapon._playerInventory = this;

        _weight -= it.mass / 2;
        weapon.Remove(it);
        _weapItemsIDs.Remove(it._itemID);
        _weaponInHandID = it._itemID;
        _descriptionObj.SetActive(false);
    }

    public void PlayerToBarter(Drag drag)
    {
        Item it = drag._item;

        _trading._npcInventory._reputation += it.price;
        _trading._npcInventory._items.Add(it);
        consumables.Remove(it);
        weapon.Remove(it);
        expItems.Remove(it);
        _consItemsIDs.Remove(it._itemID);
        _weapItemsIDs.Remove(it._itemID);
        _expItemsIDs.Remove(it._itemID);
        _trading.InventoryUpdate();
    }

    public void LoadItems()
    {
        foreach (string id in _consItemsIDs)
        {
            if (id == "1")
            {
                GameObject itemObj = Instantiate<GameObject>(Resources.Load<GameObject>("Prefabs/Items/Jelly plate"));
                Item it = itemObj.GetComponent<Item>();
                it._partsDescr = it._description.Split('$');
                consumables.Add(it);
                itemObj.SetActive(false);
            }
            if (id == "2")
            {
                GameObject itemObj = Instantiate<GameObject>(Resources.Load<GameObject>("Prefabs/Items/Welding fluid"));
                Item it = itemObj.GetComponent<Item>();
                it._partsDescr = it._description.Split('$');
                consumables.Add(it);
                itemObj.SetActive(false);
            }
            if (id == "3")
            {
                GameObject itemObj = Instantiate<GameObject>(Resources.Load<GameObject>("Prefabs/Items/Diesel Fuel"));
                Item it = itemObj.GetComponent<Item>();
                it._partsDescr = it._description.Split('$');
                consumables.Add(it);
                itemObj.SetActive(false);
            }
            if (id == "4")
            {
                GameObject itemObj = Instantiate<GameObject>(Resources.Load<GameObject>("Prefabs/Items/Metal Plate"));
                Item it = itemObj.GetComponent<Item>();
                it._partsDescr = it._description.Split('$');
                consumables.Add(it);
                itemObj.SetActive(false);
            }
            if (id == "5")
            {
                GameObject itemObj = Instantiate<GameObject>(Resources.Load<GameObject>("Prefabs/Items/AntiShock Fiber"));
                Item it = itemObj.GetComponent<Item>();
                it._partsDescr = it._description.Split('$');
                consumables.Add(it);
                itemObj.SetActive(false);
            }
            if (id == "6")
            {
                GameObject itemObj = Instantiate<GameObject>(Resources.Load<GameObject>("Prefabs/Items/Improved Processor"));
                Item it = itemObj.GetComponent<Item>();
                it._partsDescr = it._description.Split('$');
                consumables.Add(it);
                itemObj.SetActive(false);
            }
        }
        foreach (var id in _weapItemsIDs)
        {
            if (id == "1")
            {
                GameObject itemObj = Instantiate<GameObject>(Resources.Load<GameObject>("Prefabs/Items/Iron pipe"));
                Item it = itemObj.GetComponent<Item>();
                it._partsDescr = it._description.Split('$');
                weapon.Add(it);
                itemObj.SetActive(false);
            }
            if (id == "2")
            {
                GameObject itemObj = Instantiate<GameObject>(Resources.Load<GameObject>("Prefabs/Items/Steel pipe"));
                Item it = itemObj.GetComponent<Item>();
                it._partsDescr = it._description.Split('$');
                weapon.Add(it);
                itemObj.SetActive(false);
            }
            if (id == "3")
            {
                GameObject itemObj = Instantiate<GameObject>(Resources.Load<GameObject>("Prefabs/Items/Board"));
                Item it = itemObj.GetComponent<Item>();
                it._partsDescr = it._description.Split('$');
                weapon.Add(it);
                itemObj.SetActive(false);
            }
            if (id == "4")
            {
                GameObject itemObj = Instantiate<GameObject>(Resources.Load<GameObject>("Prefabs/Items/Axe"));
                Item it = itemObj.GetComponent<Item>();
                it._partsDescr = it._description.Split('$');
                weapon.Add(it);
                itemObj.SetActive(false);
            }
        }
        foreach (var id in _expItemsIDs)
        {
            if (id == "1")
            {
                GameObject itemObj = Instantiate<GameObject>(Resources.Load<GameObject>("Prefabs/Items/ExpChip"));
                Item it = itemObj.GetComponent<Item>();
                it._partsDescr = it._description.Split('$');
                expItems.Add(it);
                itemObj.SetActive(false);
            }
        }
    }
}

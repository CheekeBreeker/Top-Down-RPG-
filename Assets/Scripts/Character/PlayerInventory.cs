using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    PlayerStats _playerStats;

    public List<Item> consumables = new List<Item>();
    public List<Item> weapon = new List<Item>();

    public int typeOutput;

    public List<Drag> _drags = new();
    public GameObject _inventory;

    public GameObject _cell;
    public Transform _cellParent;

    private void Start()
    {
        typeOutput = 1;
        _playerStats = GetComponent<PlayerStats>();
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

        for (int i = 0; i < consumables.Count; i++)
        {
            GameObject newCell = Instantiate(_cell);
            newCell.transform.SetParent(_cellParent, false);
            _drags.Add(newCell.GetComponent<Drag>());
        }

        if(typeOutput == 1)
        {
            for (int i = 0; i < consumables.Count; i++)
            {
                Item it = consumables[i];

                for (int j = 0; j < _drags.Count; j++)
                {
                    if (_drags[j].ownerItem != "")
                    {
                        if (consumables[i].isStackable)
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
                        _drags[j].image.sprite = Resources.Load<Sprite>(consumables[i].pathSprite);
                        _drags[j].ownerItem = "myItem";
                        _drags[j].countItem++;
                        _drags[j].count.text = "" + _drags[j].countItem;
                        _drags[j]._playerInventory = this;
                        break;
                    }
                }
            }
        }
        else if (typeOutput == 2)
        {
            for (int i = 0; i < weapon.Count; i++)
            {
                Item it = weapon[i];

                for (int j = 0; j < _drags.Count; j++)
                {
                    if (_drags[j].ownerItem != "")
                    {
                        if (weapon[i].isStackable)
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
                        _drags[j].image.sprite = Resources.Load<Sprite>(consumables[i].pathSprite);
                        _drags[j].ownerItem = "myItem";
                        _drags[j].countItem++;
                        _drags[j].count.text = "" + _drags[j].countItem;
                        _drags[j]._playerInventory = this;
                        break;
                    }
                }
            }
        }

        for(int i = _drags.Count - 1; i > 0; i--)
        {
            if (_drags[i].ownerItem == "")
            {
                Destroy(_drags[i].gameObject);
                _drags.RemoveAt(i);
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

    public void RemoveItem(Drag drag)
    {
        Item it = drag._item;
        GameObject newObj = Instantiate<GameObject>(Resources.Load<GameObject>(it.pathPrefab));
        newObj.transform.position = transform.position + transform.forward + transform.up;
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
            consumables.Remove(drag._item);
        }
        else if (it.typeItem == "Weapon")
        {
            Debug.Log("UseWeapon");
        }
        InventoryEnabled();
    }
}

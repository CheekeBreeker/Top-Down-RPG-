using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    public List<Item> item = new List<Item>();
    public List<Drag> _drags = new();
    public GameObject _inventory;

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
            drag.RemoveCell();
    }
    public void InventoryEnabled()
    {
        foreach (Drag drag in _drags)
            drag.RemoveCell();

        for (int i = 0; i < item.Count; i++)
        {
            Item it = item[i];

            for (int j = 0; j < _drags.Count; j++)
            {
                if (_drags[j].ownerItem != "")
                {
                    if (item[i].isStackable)
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
                    _drags[j].image.sprite = Resources.Load<Sprite>(item[i].pathSprite);
                    _drags[j].ownerItem = "myItem";
                    _drags[j].countItem++;
                    break;
                }
            }
        }
    }

    public void RemoveItem(Drag drag)
    {
        Item it = drag._item;
        GameObject newObj = Instantiate<GameObject>(Resources.Load<GameObject>(it.pathPrefab));
        newObj.transform.position = transform.position + transform.forward + transform.up;
        item.Remove(it);
        InventoryEnabled();
    }

    public void UseItem(Drag drag)
    {
        Debug.Log("UseItem");
    }
}

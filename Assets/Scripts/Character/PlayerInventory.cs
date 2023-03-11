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
            _drags[i]._item = item[i];
            _drags[i].image.sprite = Resources.Load<Sprite>(item[i].pathSprite);
            _drags[i].ownerItem = "myItem";
        }
    }

    public void RemoveItem(Drag drag)
    {
        Debug.Log("RemoveItem");
    }

    public void UseItem(Drag drag)
    {
        Debug.Log("UseItem");
    }
}

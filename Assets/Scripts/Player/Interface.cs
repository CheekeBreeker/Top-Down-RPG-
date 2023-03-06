using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interface : MonoBehaviour
{
    [SerializeField] private Transform _cursor;
    [SerializeField] private GameObject _inventory;

    private void Start()
    {
        var cursorTarger = _cursor.transform.rotation;
    }

    private void Update()
    {
        CursorMove();
        InventorySetActive();
    }

    private void CursorMove()
    {
        _cursor.transform.position = Input.mousePosition;
        _cursor.transform.Rotate(0f, 0f, 50f * Time.deltaTime);
    }

    private void InventorySetActive()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            if (!_inventory.activeSelf)
                _inventory.SetActive(true);
            else
                _inventory.SetActive(false);
        }
    }
}

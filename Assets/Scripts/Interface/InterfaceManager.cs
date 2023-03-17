using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InterfaceManager : MonoBehaviour
{
    [SerializeField] private Transform _cursor;
    [SerializeField] private GameObject _inventory;
    [SerializeField] private Image _imgWeight;
    [SerializeField] private Text _txtWeight;
    [SerializeField] private Text _txtMaxWeight;

    public PlayerInventory _playerInventory;

    private void Start()
    {
        var cursorTarger = _cursor.transform.rotation;
    }

    private void Update()
    {
        CursorMove();
        InventoryActive();
        WeightInterface();
    }

    private void CursorMove()
    {
        _cursor.transform.position = Input.mousePosition;
        _cursor.transform.Rotate(0f, 0f, 50f * Time.deltaTime);
    }

    private void InventoryActive()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            if (!_inventory.activeSelf)
                _inventory.SetActive(true);
            else
                _inventory.SetActive(false);
        }
    }

    public void WeightInterface()
    {
        _imgWeight.fillAmount = _playerInventory._weight / _playerInventory._maxWeight;
        _txtWeight.text = _playerInventory._weight.ToString();
        _txtMaxWeight.text = (_playerInventory._maxWeight - _playerInventory._weight).ToString();
    }
}

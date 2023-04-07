using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InterfaceManager : MonoBehaviour
{
    [SerializeField] private CharacterStatus _characterStatus;
    [SerializeField] private Transform _cursor;
    [SerializeField] private GameObject _inventory;
    [SerializeField] private GameObject _journal;
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

        if (Input.GetKeyDown(KeyCode.I) && !_characterStatus.isTrade)
        {
            InventoryActive();
        }
        if (Input.GetKeyDown(KeyCode.J) && !_characterStatus.isTrade)
        {
            JournalActive();
        }
        WeightInterface();
    }

    private void CursorMove()
    {
        _cursor.transform.position = Input.mousePosition;
        _cursor.transform.Rotate(0f, 0f, 50f * Time.deltaTime);
    }

    public void InventoryActive()
    {
        if (!_inventory.activeSelf)
        {
            _inventory.SetActive(true);
            Time.timeScale = 0f;
        }
        else
        {
            _inventory.SetActive(false);
            if (!_journal.activeSelf)
                Time.timeScale = 1f;
        }
    }

    public void JournalActive()
    {
        if (!_journal.activeSelf)
        {
            _journal.SetActive(true);
            Time.timeScale = 0f;
        }
        else
        {
            _journal.SetActive(false);
            if (!_inventory.activeSelf)
                Time.timeScale = 1f;
        }
    }

    public void WeightInterface()
    {
        _imgWeight.fillAmount = _playerInventory._weight / _playerInventory._maxWeight;
        _txtWeight.text = _playerInventory._weight.ToString();
        _txtMaxWeight.text = (_playerInventory._maxWeight - _playerInventory._weight).ToString();
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InterfaceManager : MonoBehaviour
{
    [SerializeField] private Animator _animator;
    [SerializeField] private InterfaceAnims _InventoryAnim;
    [SerializeField] private InterfaceAnims _JourAnim;
    [SerializeField] private InterfaceAnims _SkillsAnim;
    [SerializeField] private InterfaceAnims _dialogAnim;
    [SerializeField] private InterfaceAnims _TradeAnim;
    [SerializeField] private CharacterStatus _characterStatus;
    [SerializeField] private Transform _cursor;
    [SerializeField] private GameObject _inventory;
    [SerializeField] private GameObject _journal;
    [SerializeField] private Image _imgWeight;
    [SerializeField] private Text _txtWeight;
    [SerializeField] private Text _txtMaxWeight;

    public PlayerInventory _playerInventory;
    public int _countTimeScaleZero;

    private void Start()
    {
        _animator = GetComponent<Animator>();
        var cursorTarger = _cursor.transform.rotation;
    }

    private void Update()
    {
        CursorMove();

        if (Input.GetKeyDown(KeyCode.I) && !_characterStatus.isTrade)
        {
            _InventoryAnim.ActiveAnim();
        }
        if (Input.GetKeyDown(KeyCode.J) && !_characterStatus.isTrade)
        {
            _JourAnim.ActiveAnim();
        }
        WeightInterface();
    }

    public void DialogTrigger()
    {
        _dialogAnim.ActiveAnim();
    }

    public void TradeTrigger()
    {
        _InventoryAnim.ActiveAnim();
        _TradeAnim.ActiveAnim();
    }

    private void CursorMove()
    {
        _cursor.transform.position = Input.mousePosition;
        _cursor.transform.Rotate(0f, 0f, 50f * Time.deltaTime);
    }

    public void WeightInterface()
    {
        _imgWeight.fillAmount = _playerInventory._weight / _playerInventory._maxWeight;
        _txtWeight.text = _playerInventory._weight.ToString();
        _txtMaxWeight.text = (_playerInventory._maxWeight - _playerInventory._weight).ToString();
    }
}

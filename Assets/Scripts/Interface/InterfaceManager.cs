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
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab) && !_characterStatus.isTrade)
        {
            _InventoryAnim.ActiveAnim();
        }
        if (Input.GetKeyDown(KeyCode.J) && !_characterStatus.isTrade)
        {
            _JourAnim.ActiveAnim();
        }
        if (Input.GetKeyDown(KeyCode.L) && !_characterStatus.isTrade)
        {
            _SkillsAnim.ActiveAnim();
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

    public void WeightInterface()
    {
        _imgWeight.fillAmount = _playerInventory._weight / _playerInventory._maxWeight;
        _txtWeight.text = _playerInventory._weight.ToString();
        _txtMaxWeight.text = (_playerInventory._maxWeight - _playerInventory._weight).ToString();
    }
}

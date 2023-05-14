using UnityEngine;

public class Controller : MonoBehaviour
{
    private PlayerMovement _playerMovement;
    private CharacterAnimation _characterAnimation;
    private PlayerInput _playerInput;

    [SerializeField] private FieldOfView _fieldOfHear;
    [SerializeField] public GameObject _fade;
    [SerializeField] private CharacterStatus _characterStatus;

    [SerializeField] private float _screamTime = 3f;

    public void Start()
    {
        _playerMovement = GetComponent<PlayerMovement>();
        _characterAnimation = GetComponent<CharacterAnimation>();
        _playerInput = GetComponent<PlayerInput>();

        _fade.SetActive(true);

        _characterStatus.isAiming = false;
        _characterStatus.isNormal = false;
        _characterStatus.isSprint = false;
        _characterStatus.isBlock = false;
        _characterStatus.isDodge = false;
        _characterStatus.isAiming = false;
        _characterStatus.isAttack = false;
        _characterStatus.isAttackDamaging = false;
        _characterStatus.isUsing = false;
        _characterStatus.isTalk = false;
        _characterStatus.isTrade = false;
        _characterStatus.isScream = false;
    }

    public void Update()
    {
        _characterAnimation.AnimationUpdate();
        _playerInput.InputUpdate();

        if (_playerMovement._characterStatus.isAttackDamaging || _fieldOfHear.visibleTargets.Count != 0)
        {
            _playerMovement._characterStatus.isScream = true;
        }
        if (_playerMovement._characterStatus.isScream)
        {
            if (_screamTime > 0) _screamTime -= Time.deltaTime;
            else
            {
                _playerMovement._characterStatus.isScream = false;
                _screamTime = 3;
            }
        }
    }

    private void FixedUpdate()
    {
        _playerMovement.MoveUpdate();
    }
}

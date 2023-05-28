using System.Collections;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    public CharacterStatus _characterStatus;
    private PlayerMovement _playerMovement;
    private Animator _animator;

    private bool isNormal;
    private bool isSprint;
    private bool isBlock;
    private bool isDodge;
    private bool isAiming;
    private bool isAttack;
    private bool isUsing;

    private bool debagNormal;
    private bool debagSprint;
    private bool debagBlock;
    private bool debagDodge;
    private bool debagAiming;
    private bool debagAttack;
    private bool debagUsing;

    private void Awake()
    {
        _playerMovement = GetComponent<PlayerMovement>();
        _animator = GetComponent<Animator>();
    }

    public void InputUpdate()
    {
        if (!debagNormal &&
            !_characterStatus.isDodge && !_characterStatus.isAttack && !_characterStatus.isSprint)
            _characterStatus.isNormal = true;
        else _characterStatus.isNormal = isNormal;

        if (!debagSprint &&
            !_characterStatus.isDodge && !_characterStatus.isBlock && !_characterStatus.isAiming) 
            _characterStatus.isSprint = Input.GetKey(KeyCode.LeftShift);
        else _characterStatus.isSprint = isSprint;

        if (!debagBlock && 
            !_characterStatus.isSprint) 
            _characterStatus.isBlock = Input.GetMouseButton(1);
        else _characterStatus.isBlock = isBlock;

        if (!debagDodge &&
            !_characterStatus.isBlock && !_characterStatus.isAiming)
            IsDodging();
        else _characterStatus.isDodge = isDodge;

        if (!debagAiming && 
            !_characterStatus.isDodge && !_characterStatus.isSprint && !_characterStatus.isAttack) 
            _characterStatus.isAiming = Input.GetMouseButton(2);
        else _characterStatus.isAiming = isAiming;

        if (!debagUsing &&
            !_characterStatus.isDodge && !_characterStatus.isSprint && !_characterStatus.isAiming && !_characterStatus.isAttack)
            _characterStatus.isUsing = Input.GetKeyDown(KeyCode.E);
        else _characterStatus.isUsing = isUsing;

        if (!_characterStatus.isDodge && !_characterStatus.isSprint && !_characterStatus.isAiming && !_characterStatus.isAttack
            && Input.GetKeyDown(KeyCode.Z) && _playerMovement._levelUpgrade._isHaveMetallistSkill) _animator.SetTrigger("down");

        if (!Input.GetMouseButton(0)) _characterStatus.isAttack = false;
    }

    private bool IsAnimPlaying(string animName)
    {
        var animStateInfo = _animator.GetCurrentAnimatorStateInfo(0);
        if (animStateInfo.IsName(animName)) return true;
        else return false;
    }
    private void IsDodging()
    {
        if (IsAnimPlaying("Dodge.Dodge") || Input.GetKeyDown(KeyCode.Space))
        {
            _characterStatus.isDodge = true;
            StartCoroutine(TimerDodgeCor());
        }
        StopCoroutine(TimerDodgeCor());
    }

    IEnumerator TimerDodgeCor()
    {
        yield return new WaitForSeconds(_playerMovement._dodgeActiveTime);
        _characterStatus.isDodge = false;
    }
}

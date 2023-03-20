using System.Collections;
using System.Collections.Generic;
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

        if (!debagAttack && 
            !_characterStatus.isDodge) 
            isAttacking();
        else _characterStatus.isAttack = isAttack;

        if (!debagUsing &&
            !_characterStatus.isDodge && !_characterStatus.isSprint && !_characterStatus.isAiming && !_characterStatus.isAttack)
            _characterStatus.isUsing = Input.GetKeyDown(KeyCode.E);
        else _characterStatus.isUsing = isUsing;
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

    private void isAttacking()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            if (!_characterStatus.isAttack) _characterStatus.isAttack = true;
            else _characterStatus.isAttack = false;
        }

        //_characterStatus.isAttack = false;

        //if (IsAnimPlaying("FirstAttack.FirstAttack") || IsAnimPlaying("SecondAttack.SecondAttack") || IsAnimPlaying("ThirdAttack.ThirdAttack") || Input.GetMouseButtonDown(0))
        //{
        //    _characterStatus.isAttack = true;
        //    StartCoroutine(TimerAttackCor());
        //}
        //StopCoroutine(TimerAttackCor());

        //IEnumerator TimerAttackCor()
        //{
        //    yield return new WaitForSeconds(0.8f);
        //    _characterStatus.isAttack = false;
        //}

        //if (Input.GetMouseButtonDown(0))
        //{
        //    _characterStatus.isAttack = true;
        //    StartCoroutine(AttackCor());
        //}
        //StopCoroutine(AttackCor());
    }

    IEnumerator AttackCor()
    {
        _characterStatus.isAttack = false;
        yield return new WaitForSeconds(0.8f);
        _characterStatus.isAttack = true;
    }
}

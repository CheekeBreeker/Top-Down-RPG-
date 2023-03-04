using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    public CharacterStatus _characterStatus;
    private PlayerMovement _playerMovement;

    private bool isNormal;
    private bool isSprint;
    private bool isBlock;
    private bool isDodge;
    private bool isAiming;
    private bool isAttack;

    private bool debagNormal;
    private bool debagSprint;
    private bool debagBlock;
    private bool debagDodge;
    private bool debagAiming;
    private bool debagAttack;

    private void Awake()
    {
        _playerMovement = GetComponent<PlayerMovement>();
    }

    public void InputUpdate()
    {
        if (!debagNormal &&
            (!Input.GetKeyDown(KeyCode.Space) && !Input.GetKey(KeyCode.LeftShift) && 
            !_characterStatus.isDodge))
            _characterStatus.isNormal = true;
        else _characterStatus.isNormal = isNormal;

        if (!debagSprint &&
            (!Input.GetKeyDown(KeyCode.Space) && !Input.GetMouseButton(2) && !Input.GetMouseButtonDown(1)) &&
            !_characterStatus.isDodge) 
            _characterStatus.isSprint = Input.GetKey(KeyCode.LeftShift);
        else _characterStatus.isSprint = isSprint;

        if (!debagBlock && 
            !Input.GetKey(KeyCode.LeftShift)) 
            _characterStatus.isBlock = Input.GetMouseButtonDown(1);
        else _characterStatus.isBlock = isBlock;

        if (!debagDodge && 
            (!Input.GetMouseButtonDown(1))) 
            Dodging();
        else _characterStatus.isDodge = isDodge;

        if (!debagAiming && 
            !Input.GetKey(KeyCode.LeftShift)) 
            _characterStatus.isAiming = Input.GetMouseButton(2);
        else _characterStatus.isAiming = isAiming;

        if (!debagAttack && 
            (!Input.GetKeyDown(KeyCode.Space) && !Input.GetKey(KeyCode.LeftShift) && !Input.GetMouseButtonDown(1) &&
            !_characterStatus.isDodge)) 
            isAttacking();
        else _characterStatus.isAttack = isAttack;
    }
    private void Dodging()
    {
        if (Input.GetKeyDown(KeyCode.Space))
            StartCoroutine(DodgeCor());
        StopCoroutine(DodgeCor());
    }

    IEnumerator DodgeCor()
    {
        _characterStatus.isDodge = true;
        yield return new WaitForSeconds(_playerMovement._dodgeActiveTime);
        _characterStatus.isDodge = false;
    }

    private void isAttacking()
    {
        _characterStatus.isAttack = false;
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

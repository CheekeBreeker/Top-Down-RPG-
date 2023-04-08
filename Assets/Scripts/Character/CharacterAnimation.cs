using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class CharacterAnimation : MonoBehaviour
{
    private Animator _anim;
    private PlayerMovement _playerMovement;
    public CharacterStatus _characterStatus;

    private void Awake()
    {
        _anim = GetComponent<Animator>();
        _playerMovement = GetComponent<PlayerMovement>();
    }

    public void AnimationUpdate()
    {
        _anim.SetBool("sprint", _characterStatus.isSprint);
        _anim.SetBool("block", _characterStatus.isBlock);
        _anim.SetBool("dodge", _characterStatus.isDodge);
        _anim.SetBool("use", _characterStatus.isUsing);
        _anim.SetBool("attack", _characterStatus.isAttack);

        if (_characterStatus.isNormal) AnimationNormal();
        if (_characterStatus.isSprint) AnimationSprint();
        if (_characterStatus.isDodge) AnimationDodge();

        AttackController();
    }

    void AnimationNormal()
    {
        _anim.SetFloat("vertical", Vector3.Dot(new Vector3(_playerMovement.horizontal, 0f, _playerMovement.vertical), 
            _playerMovement._playerModel.transform.forward), 0.15f, Time.deltaTime);
        _anim.SetFloat("horizontal", Vector3.Dot(new Vector3(_playerMovement.horizontal, 0f, _playerMovement.vertical),
            _playerMovement._playerModel.transform.right), 0.15f, Time.deltaTime);
    }

    void AttackController()
    {
        if (_anim.GetCurrentAnimatorStateInfo(0).IsName("FirstAttack") &&
            !_anim.GetCurrentAnimatorStateInfo(0).IsName("SecondAttack") &&
            !_anim.GetCurrentAnimatorStateInfo(0).IsName("ThirdAttack"))
            _characterStatus.isAttackDamaging = true;
        else if (!_anim.GetCurrentAnimatorStateInfo(0).IsName("FirstAttack") &&
            _anim.GetCurrentAnimatorStateInfo(0).IsName("SecondAttack") &&
            !_anim.GetCurrentAnimatorStateInfo(0).IsName("ThirdAttack"))
            _characterStatus.isAttackDamaging = true;
        else if (!_anim.GetCurrentAnimatorStateInfo(0).IsName("FirstAttack") &&
            !_anim.GetCurrentAnimatorStateInfo(0).IsName("SecondAttack") &&
            _anim.GetCurrentAnimatorStateInfo(0).IsName("ThirdAttack"))
            _characterStatus.isAttackDamaging = true;
        else if (!_anim.GetCurrentAnimatorStateInfo(0).IsName("FirstAttack") &&
            !_anim.GetCurrentAnimatorStateInfo(0).IsName("SecondAttack") &&
            !_anim.GetCurrentAnimatorStateInfo(0).IsName("ThirdAttack")) 
            _characterStatus.isAttackDamaging = false;
    }

    void AnimationSprint()
    {
        //_anim.SetFloat("vertical", Mathf.Clamp01(Mathf.Abs(_playerMovement.vertical) 
        //    + Mathf.Abs(_playerMovement.horizontal)), 0.15f, Time.deltaTime);
        //_anim.SetFloat("vertical", Vector3.Dot(new Vector3(_playerMovement.horizontal, 0f, _playerMovement.vertical),
        //    _playerMovement._playerModel.transform.forward), 0.15f, Time.deltaTime);
        //_anim.SetFloat("horizontal", Vector3.Dot(new Vector3(_playerMovement.horizontal, 0f, _playerMovement.vertical),
        //    _playerMovement._playerModel.transform.right), 0.15f, Time.deltaTime);
        _anim.SetFloat("vertical", 0f);
        _anim.SetFloat("horizontal", 0f);
    }

    void AnimationDodge()
    {
        _anim.SetFloat("vertical", 0f);
        _anim.SetFloat("horizontal", 0f);
    }
}

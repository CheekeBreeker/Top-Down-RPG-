using UnityEngine;

public class CharacterAnimation : MonoBehaviour
{
    private Animator _anim;
    private PlayerMovement _playerMovement;
    public CharacterStatus _characterStatus;

    public Animator _interfaceAnim;

    private void Start()
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
    }

    void AnimationNormal()
    {
        _anim.SetFloat("vertical", Vector3.Dot(new Vector3(_playerMovement.horizontal, 0f, _playerMovement.vertical), 
            _playerMovement._playerModel.transform.forward), 0.15f, Time.deltaTime);
        _anim.SetFloat("horizontal", Vector3.Dot(new Vector3(_playerMovement.horizontal, 0f, _playerMovement.vertical),
            _playerMovement._playerModel.transform.right), 0.15f, Time.deltaTime);
    }

    void AttackControllerTrue_AnimEvent()
    {
        _characterStatus.isAttackDamaging = true;
    }
    void AttackControllerFalls_AnimEvent()
    {
        _characterStatus.isAttackDamaging = false;
    }
    void DodgeControllerTrue_AnimEvent()
    {
        _playerMovement._isCanLook = false;
        GetComponent<CapsuleCollider>().enabled = false;
        GetComponentInChildren<BoxCollider>().enabled = false;
    }
    void DodgeControllerFalls_AnimEvent()
    {
        _playerMovement._isCanLook = true;
        GetComponent<CapsuleCollider>().enabled = true;
        GetComponentInChildren<BoxCollider>().enabled = true;
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

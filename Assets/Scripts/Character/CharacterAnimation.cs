using UnityEngine;

public class CharacterAnimation : MonoBehaviour
{
    private Animator _anim;
    private PlayerMovement _playerMovement;
    public CharacterStatus _characterStatus;
    public AudioManager _audioManager;

    [SerializeField] private FieldOfView _fieldOfView;

    public Animator _interfaceAnim;

    private void Start()
    {
        _anim = GetComponent<Animator>();
        _playerMovement = GetComponent<PlayerMovement>();
        _audioManager = GetComponent<AudioManager>();
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
        _audioManager.PlayDodgeClip();
        _playerMovement._isCanLook = false;
        _playerMovement._isCanMove = false;
        //GetComponentInChildren<BoxCollider>().enabled = false;
    }
    void DodgeControllerFalls_AnimEvent()
    {
        _playerMovement._isCanLook = true;
        _playerMovement._isCanMove = true;
        //GetComponentInChildren<BoxCollider>().enabled = true;
    }
    void InvisibleControllerTrue_AnimEvent()
    {
        _audioManager.PlayMetallCrashClip();
        _playerMovement._isCanLook = false;
        _playerMovement._isCanMove = false;
        _playerMovement._isActiveSkill = true;
        _playerMovement.vertical = 0f;
        _playerMovement.horizontal = 0f;
        _fieldOfView.viewRadius = 3;
        _fieldOfView.viewAngle = 360;
        GetComponent<CapsuleCollider>().enabled = false;
        //GetComponentInChildren<BoxCollider>().enabled = false;
    }
    void InvisibleControllerFalls_AnimEvent()
    {
        _audioManager.PlayMetallCrashClip();
        _playerMovement._isCanLook = true;
        _playerMovement._isCanMove = true;
        _playerMovement._isActiveSkill = false;
        _playerMovement.vertical = 0f;
        _playerMovement.horizontal = 0f;
        _fieldOfView.viewRadius = 12.9f;
        _fieldOfView.viewAngle = 124;
        GetComponent<CapsuleCollider>().enabled = true;
        //GetComponentInChildren<BoxCollider>().enabled = true;
    }
    void FallsControllerTrue_AnimEvent()
    {
        _audioManager.PlayDodgeClip();
        _playerMovement._isCanLook = false;
        _playerMovement._isCanMove = false;
        _playerMovement.vertical = 0f;
        _playerMovement.horizontal = 0f;
        _fieldOfView.viewRadius = 3;
        _fieldOfView.viewAngle = 360;
    }
    void FallsControllerFalls_AnimEvent()
    {
        _audioManager.PlayDodgeClip();
        _playerMovement._isCanLook = true;
        _playerMovement._isCanMove = true;
        _playerMovement.vertical = 0f;
        _playerMovement.horizontal = 0f;
        _fieldOfView.viewRadius = 12.9f;
        _fieldOfView.viewAngle = 124;
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

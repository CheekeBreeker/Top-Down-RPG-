using UnityEngine;
using UnityEngine.AI;

public class NpcStats : MonoBehaviour
{
    private Animator _anim;
    private Collider _collider;

    [SerializeField] private GameObject _spine;
    [SerializeField] private NpcController _npcController;
    [SerializeField] private NpcStatus _npcStatus;
    [SerializeField] private NpcInventory _npcInventory;


    public float _health;
    public float _maxHealth;
    public float _healthToStan;
    public bool _isHurt;

    public float _handDamage;

    public Transform[] RagdollElem;

    private FieldOfView _fieldOfView;
    private FieldOfView _fieldOfHear;

    private void Start()
    {
        _maxHealth = _health;
        _anim = GetComponent<Animator>();
        _collider = GetComponent<Collider>();
        _npcController = GetComponent<NpcController>();
        _npcStatus = GetComponent<NpcStatus>();
        _npcInventory = GetComponent<NpcInventory>();
        _fieldOfView = GetComponent<NpcMovenment>()._fieldOfView;
        _fieldOfHear = GetComponent<NpcMovenment>()._fieldOfHear;
    }

    private void Update()
    {
        SpeedControl();
    }

    public void TakeAwayHealth(float takeAway)
    {
        _health -= takeAway;
        if (_health < _healthToStan)
        {
            GetComponentInParent<NpcAudioManager>().PlayDamagedClip();
            _anim.SetTrigger("impact");
            _npcStatus.isHurt = true;
        }    
        if (_health <= 0)
            Die();
    }

    public void Die()
    {
        _npcStatus.isDead = true;
        if (!_npcStatus.isBiomass)
            _anim.enabled = false;
        _npcController.enabled = false;
        _collider.enabled = false;
        _spine.SetActive(false);

        foreach (Transform body in RagdollElem)
            body.GetComponent<Rigidbody>().isKinematic = false;
        Destroy(_npcController);
        Destroy(GetComponent<CapsuleCollider>());
        Destroy(GetComponent<NavMeshAgent>());
        Destroy(GetComponent<NpcMovenment>());
        Destroy(_fieldOfView);
        Destroy(_fieldOfHear);
        Destroy(_npcInventory._objWeapon);
        _npcInventory.RemoveAllItems();
        Destroy(_npcInventory);
        Destroy(GetComponent<NpcAnimation>());
        if (_npcStatus.isBiomass)
            _anim.SetTrigger("dead");
        else Destroy(_anim);
    }

    public void SpeedControl()
    {
        if (_health <= _maxHealth * 0.35f && !_isHurt)
        {
            _isHurt = true;
            _npcStatus.isWounded = true;
        }
        if (_health > _maxHealth * 0.35f && _isHurt)
        {
            _isHurt = false;
            _npcStatus.isWounded = false;
        }
    }

    public void AttackSpeed()
    {
        if (_npcStatus.isAttackDamage)
            _anim.speed = _npcInventory._mainWeapon._attackSpeed;
        else _anim.speed = 1;
    }

    public void BecomeAnEnemy()
    {
        _npcStatus.isFreandly = false;
        gameObject.tag = "Enemy";
        _npcInventory._mainWeapon = _npcInventory._hiddenWeapon;
        _npcInventory.TakeWeapon();

        if(gameObject.TryGetComponent<NpcDialogs>(out NpcDialogs npcDialogs))
        {
            if (npcDialogs._isQuest) Destroy(gameObject.GetComponent<QuestGiver>());
            Destroy(npcDialogs);
        }
    }
}

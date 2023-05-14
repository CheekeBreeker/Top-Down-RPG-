using UnityEngine;

public class DealingDamage : MonoBehaviour
{
    public Rigidbody _weaponRig;
    public CharacterStatus _characterStatus;
    public PlayerStats _playerStats;
    public NpcStatus _npcStatus;
    public PlayerMovement _playerMovement;
    public NpcStats _npcStats;
    public Item _item;
    public float _damage;
    public float _plusDamage;
    public bool _isWasOnSpineCollider;

    [SerializeField] private bool _isHand;

    private void Start()
    {
        _weaponRig = GetComponent<Rigidbody>();
        if (!_isHand)
        {
            _item = GetComponent<Item>();
            _damage = _item._weaponDamage;
        }
        else _npcStats = GetComponentInParent<NpcStats>();

        _npcStatus = GetComponentInParent<NpcStatus>();

    }

    private void Update()
    {
    }

    private void OnTriggerEnter(Collider other)
    {
        if (_item != null)
        {
            if (_item._owner == "Player")
            {
                if (_characterStatus.isAttackDamaging)
                {
                    if (other.gameObject.CompareTag("Enemy"))
                    {
                        GetComponentInParent<AudioManager>().PlayDamagingClip();
                        Attacking(true, other);
                        return;
                    }
                    else if (other.gameObject.CompareTag("Trader") || other.gameObject.CompareTag("Freandly Npc"))
                    {
                        GetComponentInParent<AudioManager>().PlayDamagingClip();
                        Attacking(true, other);

                        other.gameObject.GetComponent<NpcStats>().BecomeAnEnemy();
                        return;
                    }
                }
            }

            else if (_item._owner == "Npc")
            {
                if (_npcStatus.isAttackDamage)
                {
                    if (other.gameObject.CompareTag("Player"))
                    {
                        GetComponentInParent<NpcAudioManager>().PlayDamagingClip();
                        Attacking(false, other);
                        return;
                    }
                }
            }
            else return;
        }
        else
        {
            if (_npcStatus.isAttackDamage)
            {
                if (other.gameObject.CompareTag("Player"))
                {
                    GetComponentInParent<NpcAudioManager>().PlayDamagingClip();
                    _playerStats = other.gameObject.GetComponent<PlayerStats>();

                    if (_playerStats._wendingFluidUseTime > 0)
                        _playerStats.TakeAwayHealth(GetComponent<Item>().wendingFluidDamage);

                    _playerStats.TakeAwayHealth(_npcStats._handDamage);
                    Debug.Log("enemy damage " + _npcStats._handDamage);
                    if (_isWasOnSpineCollider)
                    {
                        _plusDamage -= _item._weaponDamage * 1.3f;
                        _isWasOnSpineCollider = false;
                    }
                    return;
                }
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (_item != null)
        {
            _npcStats = null;
            _playerStats = null;
        }
        else
        {
            _playerStats = null;
        }
    }

    private void Attacking(bool isPlayer, Collider other)
    {
        if (isPlayer)
        {
            _npcStats = other.gameObject.GetComponent<NpcStats>();
            float plusDmg = GetComponentInParent<PlayerStats>()._plusDamage;

            if (GetComponentInParent<LevelUpgrade>()._isHaveProrabSkill
                && other.GetComponent<NpcStats>()._health <= other.GetComponent<NpcStats>()._maxHealth * 0.25f)
                _npcStats.TakeAwayHealth((_damage + _plusDamage + plusDmg) * 1.25f);
            else _npcStats.TakeAwayHealth(_damage + _plusDamage + plusDmg);
            Debug.Log("damage " + _item._weaponDamage);
        }
        else if (!isPlayer)
        {
            _playerStats = other.gameObject.GetComponent<PlayerStats>();

            if (_playerStats._wendingFluidUseTime > 0)
                _playerStats.TakeAwayHealth(GetComponent<Item>().wendingFluidDamage);

            _playerStats.TakeAwayHealth(_damage);
            Debug.Log("enemy damage " + _damage);
        }
    }
}
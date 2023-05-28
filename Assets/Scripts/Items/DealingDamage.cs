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
    public bool _isWasOnBodyCollider;
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
                    if (other.gameObject.CompareTag("Enemy") && !_isWasOnSpineCollider)
                    {
                        GetComponentInParent<AudioManager>().PlayDamagingClip();
                        Attacking(true, false, other);
                        _isWasOnBodyCollider = true;
                    }
                    else if ((other.gameObject.CompareTag("Trader") || other.gameObject.CompareTag("Freandly Npc")) && !_isWasOnSpineCollider)
                    {
                        GetComponentInParent<AudioManager>().PlayDamagingClip();
                        Attacking(true, false, other);

                        other.gameObject.GetComponent<NpcStats>().BecomeAnEnemy();

                        _isWasOnBodyCollider = true;
                    }
                    else if (other.gameObject.CompareTag("EnemySpine") && !_isWasOnBodyCollider)
                    {
                        GetComponentInParent<AudioManager>().PlayDamagingClip();
                        Attacking(true, true, other);

                        if (!other.gameObject.GetComponentInParent<Spine>()._isEnemySpine)
                        {
                            other.gameObject.GetComponentInParent<NpcStats>().BecomeAnEnemy();
                        }

                        _isWasOnSpineCollider = true;
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
                        Attacking(false, false, other);
                        return;
                    }
                    else
                    if (other.gameObject.CompareTag("PlayerSpine"))
                    {
                        GetComponentInParent<NpcAudioManager>().PlayDamagingClip();
                        Attacking(false, true, other);
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
        if ((other.gameObject.CompareTag("Trader") || other.gameObject.CompareTag("Freandly Npc") || other.gameObject.CompareTag("Enemy")) && !_isWasOnSpineCollider)
            _isWasOnBodyCollider = false;
        else if (other.gameObject.CompareTag("EnemySpine") && !_isWasOnBodyCollider)
            _isWasOnSpineCollider = false;
    }

    private void Attacking(bool isPlayer, bool isSpine, Collider other)
    {
        if (isPlayer)
        {
            if (!isSpine)
            {
                _npcStats = other.gameObject.GetComponent<NpcStats>();

                if (GetComponentInParent<LevelUpgrade>()._isHaveProrabSkill
                    && other.GetComponent<NpcStats>()._health <= other.GetComponent<NpcStats>()._maxHealth * 0.25f)
                    _npcStats.TakeAwayHealth(_damage * 1.25f);
                else _npcStats.TakeAwayHealth(_damage);
                Debug.Log("damage " + _item._weaponDamage);
            }
            else
            {
                _npcStats = other.gameObject.GetComponentInParent<NpcStats>();

                if (GetComponentInParent<LevelUpgrade>()._isHaveProrabSkill
                    && other.GetComponent<NpcStats>()._health <= other.GetComponent<NpcStats>()._maxHealth * 0.25f)
                    _npcStats.AttackSpine(_damage * 1.25f);
                else _npcStats.AttackSpine(_damage);
                Debug.Log("damage " + _item._weaponDamage);
            }
        }
        else if (!isPlayer)
        {
            if (!isSpine)
            {
                _playerStats = other.gameObject.GetComponent<PlayerStats>();
                if (_playerStats._wendingFluidUseTime > 0)
                    _playerStats.TakeAwayHealth(GetComponent<Item>().wendingFluidDamage);

                _playerStats.TakeAwayHealth(_damage);
                Debug.Log("enemy damage " + _damage);
            }
            else
            {
                _playerStats = other.gameObject.GetComponentInParent<PlayerStats>();
                if (_playerStats._wendingFluidUseTime > 0)
                    _playerStats.TakeAwayHealth(GetComponent<Item>().wendingFluidDamage);

                _playerStats.AttackSpine(_damage);
                Debug.Log("enemy damage " + _damage);
            }
        }
        else return;
    }
}
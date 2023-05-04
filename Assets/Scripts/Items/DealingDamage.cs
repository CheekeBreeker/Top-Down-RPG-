using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Progress;
using static UnityEngine.UI.GridLayoutGroup;

public class DealingDamage : MonoBehaviour
{
    public Rigidbody _weaponRig;
    public CharacterStatus _characterStatus;
    public PlayerStats _playerStats;
    public NpcStatus _npcStatus;
    public PlayerMovement _playerMovement;
    public NpcStats _npcStats;
    public Item _item;
    public bool _isWasOnSpineCollider;

    [SerializeField] private bool _isHand;

    private void Start()
    {
        _weaponRig = GetComponent<Rigidbody>();
        if (!_isHand) _item = GetComponent<Item>();
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
                if (_characterStatus.isAttackDamaging && other.gameObject.CompareTag("EnemySpine"))
                {
                    _isWasOnSpineCollider = true;
                    if (!_isWasOnSpineCollider)
                        _item._weaponDamage *= 2;
                    return;
                }
                else if (_characterStatus.isAttackDamaging && other.gameObject.CompareTag("Enemy"))
                {
                    GetComponentInParent<AudioManager>().PlayDamagingClip();
                    Attacking(true, other);
                    return;
                }
                else if (_characterStatus.isAttackDamaging && (other.gameObject.CompareTag("Trader") || other.gameObject.CompareTag("Freandly Npc")))
                {
                    GetComponentInParent<AudioManager>().PlayDamagingClip();
                    Attacking(true, other);

                    other.gameObject.GetComponent<NpcStats>().BecomeAnEnemy();
                    return;
                }
            }

            else if (_item._owner == "Npc")
            {
                if (_npcStatus.isAttackDamage && other.gameObject.CompareTag("PlayerSpine"))
                {
                    _isWasOnSpineCollider = true;
                    if (!_isWasOnSpineCollider)
                        _item._weaponDamage *= 1.3f;
                    return;
                }
                else if (_npcStatus.isAttackDamage && other.gameObject.CompareTag("Player"))
                {
                    GetComponentInParent<NpcAudioManager>().PlayDamagingClip();
                    Attacking(false, other);
                    return;
                }
            }
            else return;
        }
        else
        {
            if (_npcStatus.isAttackDamage && other.gameObject.CompareTag("PlayerSpine"))
            {
                _isWasOnSpineCollider = true;
                if (!_isWasOnSpineCollider)
                    _item._weaponDamage *= 1.3f;
                return;
            }
            else if (_npcStatus.isAttackDamage && other.gameObject.CompareTag("Player"))
            {
                GetComponentInParent<NpcAudioManager>().PlayDamagingClip();
                _playerStats = other.gameObject.GetComponent<PlayerStats>();

                if (_playerStats._wendingFluidUseTime > 0)
                    _playerStats.TakeAwayHealth(GetComponent<Item>().wendingFluidDamage);

                _playerStats.TakeAwayHealth(_npcStats._handDamage);
                Debug.Log("enemy damage " + _npcStats._handDamage);
                if (_isWasOnSpineCollider)
                {
                    _item._weaponDamage /= 1.3f;
                    _isWasOnSpineCollider = false;
                }
                return;
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

            if (GetComponentInParent<LevelUpgrade>()._isHaveProrabSkill
                && other.GetComponent<NpcStats>()._health <= other.GetComponent<NpcStats>()._maxHealth * 0.25f)
                _item._weaponDamage *= 1.25f;

            _npcStats.TakeAwayHealth(_item._weaponDamage);
            Debug.Log("damage " + _item._weaponDamage);

            if (_isWasOnSpineCollider)
            {
                _item._weaponDamage /= 2;
                _isWasOnSpineCollider = false;
            }
        }
        else if (!isPlayer)
        {
            _playerStats = other.gameObject.GetComponent<PlayerStats>();

            if (_playerStats._wendingFluidUseTime > 0)
                _playerStats.TakeAwayHealth(GetComponent<Item>().wendingFluidDamage);

            _playerStats.TakeAwayHealth(_item._weaponDamage);
            Debug.Log("enemy damage " + _item._weaponDamage);
            if (_isWasOnSpineCollider)
            {
                _item._weaponDamage /= 1.3f;
                _isWasOnSpineCollider = false;
            }
        }
    }
}
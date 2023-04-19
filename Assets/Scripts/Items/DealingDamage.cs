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

    private void Start()
    {
        _weaponRig = GetComponent<Rigidbody>();
        _item = GetComponent<Item>();

        if (_item == null)
            _npcStats = GetComponentInParent<NpcStats>();
        else if (_item._owner == "Npc")
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
                if (other.gameObject.CompareTag("EnemySpine"))
                {
                    _item._weaponDamage *= 2;
                    Debug.Log("damage 2x " + _item._weaponDamage);
                }

                if (_characterStatus.isAttackDamaging && other.gameObject.CompareTag("Enemy"))
                {
                    GetComponentInParent<AudioManager>().PlayDamagingClip();
                    _npcStats = other.gameObject.GetComponent<NpcStats>();

                    if (GetComponentInParent<LevelUpgrade>()._isHaveProrabSkill
                        && other.GetComponent<NpcStats>()._health <= other.GetComponent<NpcStats>()._maxHealth * 0.25f)
                        _item._weaponDamage *= 1.25f;

                    _npcStats.TakeAwayHealth(_item._weaponDamage);
                    Debug.Log("damage " + _item._weaponDamage);
                }
                else if (_characterStatus.isAttackDamaging && (other.gameObject.CompareTag("Trader") || other.gameObject.CompareTag("Freandly Npc")))
                {
                    GetComponentInParent<AudioManager>().PlayDamagingClip();
                    _npcStats = other.gameObject.GetComponent<NpcStats>();

                    if (GetComponentInParent<LevelUpgrade>()._isHaveProrabSkill
                        && other.GetComponent<NpcStats>()._health <= other.GetComponent<NpcStats>()._maxHealth * 0.25f)
                        _item._weaponDamage *= 1.25f;

                    _npcStats.TakeAwayHealth(_item._weaponDamage);
                    Debug.Log("damage " + _item._weaponDamage);

                    other.gameObject.GetComponent<NpcStats>().BecomeAnEnemy();
                }
            }

            else if (_item._owner == "Npc")
            {
                if (other.gameObject.CompareTag("PlayerSpine"))
                {
                    _item._weaponDamage *= 2;
                    Debug.Log("enemy damage 2x " + _item._weaponDamage);
                }

                if (_npcStatus.isAttackDamage && other.gameObject.CompareTag("Player"))
                {
                    GetComponentInParent<NpcAudioManager>().PlayDamagingClip();
                    _playerStats = other.gameObject.GetComponent<PlayerStats>();

                    if (_playerStats._wendingFluidUseTime > 0)
                        _playerStats.TakeAwayHealth(GetComponent<Item>().wendingFluidDamage);

                    _playerStats.TakeAwayHealth(_item._weaponDamage);
                    Debug.Log("enemy damage " + _item._weaponDamage);
                }
            }
            else return;
        }
        else
        {
            if (other.gameObject.CompareTag("PlayerSpine"))
            {
                GetComponentInParent<NpcAudioManager>().PlayDamagingClip();
                _npcStats._handDamage *= 2;
                Debug.Log("enemy damage 2x " + _npcStats._handDamage);
            }

            if (_npcStatus.isAttackDamage && other.gameObject.CompareTag("Player"))
            {
                GetComponentInParent<NpcAudioManager>().PlayDamagingClip();
                _playerStats = other.gameObject.GetComponent<PlayerStats>();

                if (_playerStats._wendingFluidUseTime > 0)
                    _playerStats.TakeAwayHealth(GetComponent<Item>().wendingFluidDamage);

                _playerStats.TakeAwayHealth(_npcStats._handDamage);
                Debug.Log("enemy damage " + _npcStats._handDamage);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (_item != null)
        {
            _npcStats = null;
            _playerStats = null;

            if (other.gameObject.CompareTag("EnemySpine"))
            {
                _item._weaponDamage /= 2;
            }

            if (other.gameObject.CompareTag("PlayerSpine"))
            {
                _item._weaponDamage /= 2;
            }
        }
        else
        {
            _playerStats = null;

            if (other.gameObject.CompareTag("PlayerSpine"))
            {
                _npcStats._handDamage /= 2;
            }
        }
    }

    //private bool IsAttacking()
    //{
    //    if (_playerMovement._attackNumber > 0)
    //        return true;
    //    else return false;
    //}
}

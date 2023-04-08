using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Progress;

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
    }

    private void Update()
    {
        _playerMovement = GetComponentInParent<PlayerMovement>();
        _npcStatus = GetComponentInParent<NpcStatus>();
    }

    private void OnTriggerEnter(Collider other)
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
                _npcStats = other.gameObject.GetComponent<NpcStats>();

                if (GetComponentInParent<LevelUpgrade>()._isHaveProrabSkill
                    && other.GetComponent<NpcStats>()._health <= other.GetComponent<NpcStats>()._maxHealth * 0.25f)
                    _item._weaponDamage *= 1.25f;

                _npcStats.TakeAwayHealth(_item._weaponDamage);
                Debug.Log("damage " + _item._weaponDamage);
            }
        }
        else if (_item._owner == "Npc")
        {

            if (other.gameObject.CompareTag("PlayerSpine"))
            {
                _item._weaponDamage *= 2;
                Debug.Log("enemy damage 2x " + _item._weaponDamage);
            }

            if (_npcStatus.isAttack && other.gameObject.CompareTag("Player"))
            {
                _playerStats = other.gameObject.GetComponent<PlayerStats>();

                if (_playerStats._wendingFluidUseTime > 0)
                    _playerStats.TakeAwayHealth(GetComponent<Item>().wendingFluidDamage);

                _playerStats.TakeAwayHealth(_item._weaponDamage);
                Debug.Log("enemy damage " + _item._weaponDamage);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        _npcStats = null;

        if (other.gameObject.CompareTag("EnemySpine"))
        {
            _item._weaponDamage /= 2;
        }

        if (other.gameObject.CompareTag("PlayerSpine"))
        {
            _item._weaponDamage /= 2;
        }
    }

    //private bool IsAttacking()
    //{
    //    if (_playerMovement._attackNumber > 0)
    //        return true;
    //    else return false;
    //}
}

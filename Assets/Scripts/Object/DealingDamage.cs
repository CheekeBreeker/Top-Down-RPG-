using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DealingDamage : MonoBehaviour
{
    public Rigidbody _weaponRig;
    public CharacterStatus _characterStatus;
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
    }

    private void OnTriggerEnter(Collider other)
    {
        if (_playerMovement != null)
        {
            if (other.gameObject.CompareTag("EnemySpine"))
            {
                _item._weaponDamage *= 2;
                Debug.Log("damage 2x " + _item._weaponDamage);
            }

            if (_characterStatus.isAttack && other.gameObject.CompareTag("Enemy") && _playerMovement._attackNumber != 0 && _playerMovement._attackNumber != 4)
            {
                _npcStats = other.gameObject.GetComponent<NpcStats>();
                _npcStats.TakeAwayHealth(_item._weaponDamage);
                Debug.Log("damage " + _item._weaponDamage);
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
    }

    //private bool IsAttacking()
    //{
    //    if (_playerMovement._attackNumber > 0)
    //        return true;
    //    else return false;
    //}
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    public bool _isHurt;

    public float _handDamage;

    public Transform[] RagdollElem;

    private void Start()
    {
        _maxHealth = _health;
        _anim = GetComponent<Animator>();
        _collider = GetComponent<Collider>();
        _npcController = GetComponent<NpcController>();
        _npcStatus = GetComponent<NpcStatus>();
        _npcInventory = GetComponent<NpcInventory>();
    }

    private void Update()
    {
        SpeedControl();
    }

    public void TakeAwayHealth(float takeAway)
    {
        _health -= takeAway;
        _anim.SetTrigger("impact");
        if (_health < _maxHealth / 2)
            GetComponentInParent<NpcAudioManager>().PlayDamagedClip();
        if (_isHurt)
        {
            _npcStatus.isHurt = true;
        }    
        if (_health <= 0)
            Die();
    }

    public void Die()
    {
        _npcStatus.isDead = true;
        _anim.enabled = false;
        _npcController.enabled = false;
        _collider.enabled = false;
        _spine.SetActive(false);

        foreach (Transform body in RagdollElem)
            body.GetComponent<Rigidbody>().isKinematic = false;
        
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
}

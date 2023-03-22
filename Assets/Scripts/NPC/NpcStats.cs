using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NpcStats : MonoBehaviour
{
    private Animator _anim;
    private Collider _collider;

    [SerializeField] private GameObject _spine;
    [SerializeField] private NpcMovenment _npcMovenment;


    public float _health;
    public float _maxHealth;
    public bool _isHurt;

    public Transform[] RagdollElem;
    public Transform weapon;

    private void Start()
    {
        _maxHealth = _health;
        _anim = GetComponent<Animator>();
        _collider = GetComponent<Collider>();
        _npcMovenment = GetComponent<NpcMovenment>();
    }

    public void TakeAwayHealth(float takeAway)
    {
        _health -= takeAway;
        if (_isHurt)
            _anim.Play("WalkBackward");
        if (_health <= 0)
            Die();
    }

    public void Die()
    {
        _anim.enabled = false;
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
            _npcMovenment._walkSpeed = _npcMovenment._walkSpeed / 2;
            _anim.speed = 0.5f;
        }
        if (_health > _maxHealth * 0.35f && _isHurt)
        {
            _isHurt = false;
            _npcMovenment._walkSpeed = _npcMovenment._walkSpeed * 2;
            _anim.speed = 1f;
        }
    }

}

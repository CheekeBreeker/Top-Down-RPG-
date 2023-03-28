using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerStats : MonoBehaviour
{
    private PlayerMovement _playerMovement;
    private PlayerInventory _playerInventory;

    public float _health;
    public float _maxHealth;
    public Image _imgHealth;
    public float _regenHP = 0.1f;


    public int _level;
    public float _exp;
    public float _curExp;
    public Image _imgExp;

    public bool _isOverload;
    public bool _isHurt;

    private void Start()
    {
        _playerMovement = GetComponent<PlayerMovement>();
        _playerInventory = GetComponent<PlayerInventory>();

        _exp = 100 * _level;
    }

    private void Update()
    {
        hpRegeneration();
        SpeedControl();
        InterfaceUpdate();
    }

    public void AddHealth(float add)
    {
        _health += add;

        _health = Mathf.Clamp(_health, 0f, _maxHealth);
    }


    public void hpRegeneration()
    {
        if (_health < _maxHealth)
            _health += _regenHP * Time.deltaTime;
    }

    public void TakeAwayHealth(float damage)
    {
        _health -= damage;

        if (_health < 0)
            Die();
    }

    public void Die()
    {
        Time.timeScale = 0;
    }

    public void SpeedControl()
    {
        if (_playerInventory._weight > _playerInventory._maxWeight && !_isOverload)
        {
            _isOverload = true;
            _playerMovement._walkSpeed = _playerMovement._walkSpeed / 2;
            _playerMovement._sprintSpeed = _playerMovement._sprintSpeed / 2;
        }
        if (_playerInventory._weight <= _playerInventory._maxWeight && _isOverload)
        {
            _isOverload = false;
            _playerMovement._walkSpeed = _playerMovement._walkSpeed * 2;
            _playerMovement._sprintSpeed = _playerMovement._walkSpeed * 2;
        }

        if (_health <= _maxHealth * 0.35f && !_isHurt)
        {
            _isHurt = true;
            _playerMovement._walkSpeed = _playerMovement._walkSpeed / 2;
            _playerMovement._sprintSpeed = _playerMovement._walkSpeed / 2;
        }
        if (_health > _maxHealth * 0.35f && _isHurt)
        {
            _isHurt = false;
            _playerMovement._walkSpeed = _playerMovement._walkSpeed * 2;
            _playerMovement._sprintSpeed = _playerMovement._walkSpeed * 2;
        }
    }    

    public void AddExp(float add)
    {
        _curExp += add;

        if (_curExp == _exp)
        {
            _level += 1;
            _exp = 100 * _level;
            _curExp = 0;
        }    

        else if(_curExp > _exp)
        {
            _curExp -= _exp;
            _level += 1;
            _exp = 100 * _level;
        }
    }

    public void InterfaceUpdate()
    {
        _imgHealth.fillAmount = _health / _maxHealth;

        _imgExp.fillAmount = _curExp / _exp; 
    }
}

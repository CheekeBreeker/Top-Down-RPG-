using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerStats : MonoBehaviour
{
    public float _health;
    public float _maxHealth;
    public Image _imgHealth;
    public float _regenHP = 0.1f;


    public int _level;
    public float _exp;
    public float _curExp;
    public Image _imgExp;


    private void Start()
    {
        _exp = 100 * _level;
    }

    private void Update()
    {
        hpRegeneration();
        InterfaceUpdate();
    }

    public void AddHealth(int add)
    {
        _health += add;

        _health = Mathf.Clamp(_health, 0f, _maxHealth);
    }


    public void hpRegeneration()
    {
        if (_health < _maxHealth)
            _health += _regenHP * Time.deltaTime;
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

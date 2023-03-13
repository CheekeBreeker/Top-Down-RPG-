using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    public float _health;
    public float _maxHealth;
    
    public void AddHealth(int add)
    {
        _health += add;

        _health = Mathf.Clamp(_health, 0f, _maxHealth);
    }
}

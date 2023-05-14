using UnityEngine;

public class PlayerSaves : ScriptableObject
{
    public float _health;
    public float _maxHealth;
    [Space]
    public int _weaponInHandID;
    public string _itemsIDInInv;
    public string _itemsIDInJour;
    [Space]
    public bool _isTakedSelfDefenceSkill;
    public bool _isTakedProrabSkill;
    public bool _isTakedProletarianSkill;
    public bool _isTakedWelderSkill;
    public bool _isTakedMetallistSkill;
}

using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class Item : MonoBehaviour
{
    [SerializeField] private PlayerInventory _playerInventory;
    [SerializeField] private NpcInventory _npcInventory;


    public string nameItem;
    public string typeItem;
    public string _owner;

    [Multiline(10)] public string _description;

    public int price;
    public float mass;
    public int addHealth;
    public float addExp;
    public int maxCountExpItems;

    public string pathSprite;
    public string pathPrefab;

    public bool isStackable;
    public bool isWendingFluid;

    public float wendingFluidUseTime;
    public float wendingFluidDamage;

    public Vector3 _posWeapAttack;
    public Vector3 _rotWeapAttack;
    public float _weaponDamage;
    [Range (0.1f,1)]
    public float _attackSpeed;


    [SerializeField] private GameObject activeView;

    private void Start()
    {
        _playerInventory = GetComponentInParent<PlayerInventory>();
        _npcInventory = GetComponentInParent<NpcInventory>();

        if (_playerInventory != null)
            _owner = "Player";
        else if (_npcInventory != null)
            _owner = "Npc";
        else _owner = "";
    }

    private void Update()
    {
        ActiveMaterial();
    }

    private void ActiveMaterial()
    {
        if(Input.GetKey(KeyCode.LeftAlt))
        {
            activeView.SetActive(true);
        }
        else activeView.SetActive(false);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    public string nameItem;
    public string typeItem;

    public int price;
    public float mass;
    public int addHealth;
    public int addExp;

    public string pathSprite;
    public string pathPrefab;

    public bool isStackable;

    public Vector3 _posWeapAttack;
    public Vector3 _rotWeapAttack;
    public float _weaponDamage; 

    [SerializeField] private GameObject activeView;

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

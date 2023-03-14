using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    public string nameItem;
    public string typeItem;
    public int price;
    public int mass;
    public int addHealth;
    public string pathSprite;
    public string pathPrefab;
    public bool isStackable;
    public Vector3 weaponPosition;
    public Vector3 weaponRotation;
    public float weaponDamage; 

    private MeshRenderer _renderer;

    [SerializeField] private GameObject activeView;

    private void Start()
    {
        _renderer = GetComponent<MeshRenderer>();
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

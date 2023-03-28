using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Progress;

public class NpcInventory : MonoBehaviour
{
    private NpcStatus _npcStatus;
    public List<Item> _items = new List<Item>();
    public Item _mainWeapon;
    private GameObject _objWeapon;

    [SerializeField] private Transform _rightHand;
    [SerializeField] private Transform _model;

    private void Start()
    {
        _npcStatus = GetComponent<NpcStatus>();

        _mainWeapon._owner = "Npc";
        _items.Add(_mainWeapon);
        _objWeapon = Instantiate<GameObject>(Resources.Load<GameObject>(_mainWeapon.pathPrefab));
        _objWeapon.transform.SetParent(_rightHand);
        _objWeapon.transform.localPosition = _mainWeapon._posWeapAttack;
        _objWeapon.transform.localRotation = Quaternion.Euler(_mainWeapon._rotWeapAttack);
        _objWeapon.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
        _objWeapon.GetComponent<BoxCollider>().isTrigger = true;
    }

    private void Update()
    {
        if (_npcStatus.isDead) RemoveAllItems();
    }

    private void RemoveAllItems()
    {
        for (var i = 0; i < _items.Count; i ++)
        {
            GameObject item = Instantiate<GameObject>(Resources.Load<GameObject>(_items[i].pathPrefab));
            item.transform.position = new Vector3 (_model.position.x + 0.5f, _model.position.y, _model.position.z);
            _items.Remove(_items[i]);
        }

        _mainWeapon = null;
        Destroy(_objWeapon);
    }
}

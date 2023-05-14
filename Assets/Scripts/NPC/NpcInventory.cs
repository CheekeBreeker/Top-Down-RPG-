using System.Collections.Generic;
using UnityEngine;

public class NpcInventory : MonoBehaviour
{
    private NpcStatus _npcStatus;
    public List<Item> _items = new List<Item>();
    public Item _mainWeapon;
    public Item _hiddenWeapon;
    public GameObject _objWeapon;
    public int _reputation;
    public QuestGiver _questGiver;
    public bool _isDeleteQuestGiver;

    [SerializeField] private Transform _rightHand;
    [SerializeField] private Transform _model;

    private void Start()
    {
        TakeWeapon();
    }

    public void TakeWeapon()
    {
        if (_mainWeapon != null)
        {
            _mainWeapon._owner = "Npc";
            _objWeapon = Instantiate<GameObject>(Resources.Load<GameObject>(_mainWeapon.pathPrefab));
            _objWeapon.transform.SetParent(_rightHand);
            _objWeapon.transform.localPosition = _mainWeapon._posWeapAttack;
            _objWeapon.transform.localRotation = Quaternion.Euler(_mainWeapon._rotWeapAttack);
            _objWeapon.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
            _objWeapon.GetComponent<BoxCollider>().isTrigger = true;
        }
    }

    public void DeleteQuestGiver()
    {
        _questGiver = null;
    }

    public void RemoveAllItems()
    {
        for (var i = 0; i < _items.Count; i ++)
        {
            GameObject item = Instantiate<GameObject>(Resources.Load<GameObject>(_items[i].pathPrefab));
            item.transform.position = new Vector3 (_model.position.x + 0.5f, _model.position.y, _model.position.z);
            _items.Remove(_items[i]);
        }

        _mainWeapon = null;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Progress;

public class QuestGiver : MonoBehaviour
{
    public NpcInventory _npcInventory;
    public NpcDialogs _npcDIalogs;
    public int _questType;
    public Item _itemTarget;
    public List<NpcStatus> _killTargets = new List<NpcStatus>();
    public bool _isWasOnScoutTarget;
    public GameObject _scoutTarget;

    public bool _isActive;
    public bool _isDone;
    public int _plusRep;
    public Item _itemReward;

    private void Start()
    {
        _npcInventory = GetComponent<NpcInventory>();
        _npcDIalogs = GetComponent<NpcDialogs>();
        _npcDIalogs._questDescription = _npcDIalogs._questActualDescription;
    }

    public void QuestAccept(PlayerInventory playerInventory, Transform playerTransform)
    {
        if (_questType == 1) QuestTypeMessenger(playerInventory);
        else if (_questType == 2) QuestTypeKiller();
        else if (_questType == 3) QuestTypeScout(playerTransform);
    }

    public void QuestTypeMessenger(PlayerInventory playerInventory)
    {
        if (!_isActive)
            _isActive = true;
        else
        {
            if (_itemTarget.typeItem == "Consumables")
            {
                foreach (Item item in playerInventory.consumables)
                {
                    if (item.nameItem == _itemTarget.nameItem)
                    {
                        _npcInventory._items.Add(item);
                        _isDone = true;
                    }
                }
            }

            if (_itemTarget.typeItem == "Weapon")
            {
                foreach (Item item in playerInventory.weapon)
                {
                    if (item.nameItem == _itemTarget.nameItem)
                    {
                        _npcInventory._items.Add(item);
                        _isDone = true;
                    }
                }
            }

            if (_itemTarget.typeItem == "ExpItems")
            {
                foreach (Item item in playerInventory.expItems)
                {
                    if (item.nameItem == _itemTarget.nameItem)
                    {
                        _npcInventory._items.Add(item);
                        _isDone = true;
                    }
                }
            }
        }
    }

    public void QuestTypeKiller()
    {
        if (!_isActive)
            _isActive = true;

        if (_killTargets.Count > 0)
        {
            for (int i = 0; i < _killTargets.Count; i++)
            {
                if (_killTargets[i].isDead)
                {
                    Destroy(_killTargets[i]);
                    _killTargets.RemoveAt(i);
                    i--;
                }
            }
        }
        if (_killTargets.Count == 0) _isDone = true;
    }


    public void QuestTypeScout(Transform playerTransform)
    {
        if (!_isActive)
        {
            _isActive = true;
            _scoutTarget.SetActive(true);
        }
        if (_isWasOnScoutTarget)
        {
            _isDone = true;
            Destroy(_scoutTarget);
        }
    }
}

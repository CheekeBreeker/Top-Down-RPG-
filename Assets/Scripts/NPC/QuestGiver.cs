using System.Collections.Generic;
using UnityEngine;

public class QuestGiver : MonoBehaviour
{
    private NpcStatus _npcStatus;
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

    public QuestGiver _questCompetitor;
    public bool _isQuestGiverWillBeEnemy;

    private void Start()
    {
        _npcStatus = GetComponent<NpcStatus>();

        _npcInventory = GetComponent<NpcInventory>();
        _npcDIalogs = GetComponent<NpcDialogs>();
        _npcDIalogs._questDescription = _npcDIalogs._questActualDescription;
        if (_scoutTarget != null)
            _scoutTarget.GetComponent<ScoutTarget>()._questGiver.Add(this);
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
                        _isActive = false;
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
                        _isActive = false;
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
                        _isActive = false;
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
        if (_killTargets.Count == 0)
        {
            _isDone = true;
            _isActive = false;
        }
    }

    public void QuestTypeScout(Transform playerTransform)
    {
        if (!_isActive && !_isDone)
        {
            _isActive = true;
            _scoutTarget.SetActive(true);
        }
        if (_isWasOnScoutTarget)
        {
            _isDone = true;
            _isActive = false;
            Destroy(_scoutTarget);
        }
    }
}

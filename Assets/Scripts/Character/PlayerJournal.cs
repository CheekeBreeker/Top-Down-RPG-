using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using static UnityEditor.Timeline.Actions.MenuPriority;

public class PlayerJournal : MonoBehaviour
{
    public PlayerStats _playerStats;

    public List<Item> _expItem = new List<Item>();
    public List<Drag> _drags = new List<Drag>();
    public List<QuestGiver> _quests = new List<QuestGiver>();

    public GameObject _journal;
    public GameObject _journalCell;
    public Transform _cellParent;

    private void Start()
    {
        _playerStats = GetComponent<PlayerStats>();
    }

    void Update()
    {
        //JournalActive();
        foreach (Drag drag in _drags)
            Destroy(drag.gameObject);
        _drags.Clear();

        for (int i = 0; i < _expItem.Count; i++)
        {
            GameObject newCell = Instantiate(_journalCell);
            newCell.transform.SetParent(_cellParent, false);
            _drags.Add(newCell.GetComponent<Drag>());
        }

        for (int i = 0; i < _expItem.Count; i++)
        {
            Item it = _expItem[i];

            for (int j = 0; j < _drags.Count; j++)
            {
                if (_drags[j]._ownerItem != "")
                {
                    if (_expItem[i].isStackable)
                    {
                        if (_drags[j]._item.nameItem == it.nameItem)
                        {
                            _drags[j]._countItem++;
                            _drags[j]._count.text = _drags[j]._countItem.ToString();
                            _drags[j]._descriptionCell.text += " " + "321";
                            break;
                        }
                    }
                    else continue;

                }
                else
                {
                    _drags[j]._item = it;
                    _drags[j]._image.sprite = Resources.Load<Sprite>(_expItem[i].pathSprite);
                    _drags[j]._ownerItem = "myJourItem";
                    _drags[j]._nameItem.text = _expItem[i].nameItem;
                    _drags[j]._descriptionCell.text = "123";
                    _drags[j]._countItem++;
                    _drags[j]._count.text = "" + _drags[j]._countItem;
                    _drags[j]._playerJournal = this;
                    break;
                }
            }
        }

        for (int i = _drags.Count - 1; i >= 0; i--)
        {
            if (_drags[i]._ownerItem == "")
            {
                Destroy(_drags[i].gameObject);
                _drags.RemoveAt(i);
            }
        }
    }

    public void JournalActive()
    {
        if (_journal.activeSelf)
            JournalDisable();
        else JournalEnabled();
    }

    public void JournalDisable()
    {
        foreach (Drag drag in _drags)
            Destroy(drag.gameObject);
        _drags.Clear();
    }

    public void JournalEnabled()
    {
        foreach (Drag drag in _drags)
            Destroy(drag.gameObject);
        _drags.Clear();

        for (int i = 0; i < _expItem.Count; i ++)
        {
            GameObject newCell = Instantiate(_journalCell);
            newCell.transform.SetParent(_cellParent, false);
            _drags.Add(newCell.GetComponent<Drag>());
        }

        for (int i = 0; i < _expItem.Count; i++)
        {
            Item it = _expItem[i];

            for (int j = 0; j < _drags.Count; j++)
            {
                if (_drags[j]._ownerItem != "")
                {
                    if (_expItem[i].isStackable)
                    {
                        if (_drags[j]._item.nameItem == it.nameItem)
                        {
                            _drags[j]._countItem++;
                            _drags[j]._count.text = _drags[j]._countItem.ToString();
                            _drags[j]._descriptionCell.text += " " + "321";
                            break;
                        }
                    }
                    else continue;

                }
                else
                {
                    _drags[j]._item = it;
                    _drags[j]._image.sprite = Resources.Load<Sprite>(_expItem[i].pathSprite);
                    _drags[j]._ownerItem = "myItem";
                    _drags[j]._nameItem.text = _expItem[j].nameItem;
                    _drags[j]._descriptionCell.text = "123";
                    _drags[j]._countItem++;
                    _drags[j]._count.text = "" + _drags[j]._countItem;
                    _drags[j]._playerJournal = this;
                    break;
                }
            }
        }

        for (int i = _drags.Count - 1; i >= 0; i--)
        {
            if (_drags[i]._ownerItem == "")
            {
                Destroy(_drags[i].gameObject);
                _drags.RemoveAt(i);
            }
        }
    }

    public void AddItem(Drag drag, Item item)
    {
        _expItem.Add(drag._item);
    }
}

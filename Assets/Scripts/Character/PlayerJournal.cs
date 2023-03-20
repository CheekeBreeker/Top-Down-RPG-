using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Timeline.Actions.MenuPriority;

public class PlayerJournal : MonoBehaviour
{
    public List<Item> _expItem = new List<Item>();
    public List<Drag> _drags = new List<Drag>();

    public GameObject _journal;
    public GameObject _journalCell;
    public Transform _cellParent;

    void Update()
    {
        JournalActive();
    }

    public void JournalActive()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            if (_journal.activeSelf)
                JournalDisable();
            else JournalEnabled();
        }
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

        for(int i = 0; i < _expItem.Count; i ++)
        {
            GameObject newCell = Instantiate(_journalCell);
            newCell.transform.SetParent(_cellParent, false);
        }

        for (int i = _drags.Count - 1; i >= 0; i--)
        {
            if (_drags[i].ownerItem == "")
            {
                Destroy(_drags[i].gameObject);
                _drags.RemoveAt(i);
            }
        }
    }

    public void AddItem(Drag drag, Item item)
    {
        _drags.Add(drag);
        _expItem.Add(item);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoutTarget : MonoBehaviour
{
    public QuestGiver _questGiver;
    public float _scoutTime;

    private void Start()
    {
        gameObject.SetActive(false);    
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            _scoutTime -= Time.deltaTime;
            if (_questGiver._isActive && _scoutTime < 0)
                _questGiver._isWasOnScoutTarget = true;
        }
    }
}

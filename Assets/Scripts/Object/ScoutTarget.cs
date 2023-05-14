using System.Collections.Generic;
using UnityEngine;

public class ScoutTarget : MonoBehaviour
{
    public List<QuestGiver> _questGiver;
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
            foreach (QuestGiver giver in _questGiver)
            {
                if (giver._isActive && _scoutTime < 0)
                    giver._isWasOnScoutTarget = true;
            }
        }
    }
}

using System.Collections.Generic;
using UnityEngine;

public class SpawnNpcs : MonoBehaviour
{
    [SerializeField] private List<GameObject> _npcs;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            foreach (var npc in _npcs)
            {
                npc.SetActive(true);

                NpcStatus npcStatus = npc.GetComponent<NpcStatus>();
                if (npcStatus.isDead) npc.SetActive(false);
            }
        }
    }
}

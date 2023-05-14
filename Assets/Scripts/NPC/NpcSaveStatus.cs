using UnityEngine;

[CreateAssetMenu(menuName = "Npc/Save Status")]
public class NpcSaveStatus : ScriptableObject
{
    public NpcStatus[] _npcStatuses = new NpcStatus[15];
    public NpcInventory[] _npcInventories = new NpcInventory[15];
    public QuestGiver[] _questGivers = new QuestGiver[15];
}

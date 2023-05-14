using System.Collections.Generic;
using UnityEngine;

public class NpcLoader : MonoBehaviour
{
    public List<NpcSaver> _npcSavers = new List<NpcSaver>();

    public void LoadData()
    {
        foreach (NpcSaver saver in _npcSavers)
            saver.Loader();
    }

    public void SaveData()
    {
        foreach (NpcSaver saver in _npcSavers)
            saver.Saver();
    }
}

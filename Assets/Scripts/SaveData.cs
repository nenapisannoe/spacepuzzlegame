using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class FactionRelationSave
{
    public FactionType factionType;
    public int relationLevel;

    public FactionRelationSave(FactionType _type, int _relation)
    {
        factionType = _type;
        relationLevel = _relation;
    }
}

[System.Serializable]
public class SaveData
{
    public Quest currentQuest;
    public int currentQuestStep;
    public List<FactionRelationSave> factionRelations;
    public List<int> unlockedPuzzles = new List<int>();

    public (int, int) currentZone;
}


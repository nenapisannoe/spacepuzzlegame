using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public enum FactionType
{
    Workers,
    Smugglers,
    Security
}

[System.Serializable]
public class FactionEffectRule
{
    public FactionType triggerFaction;
    public int threshold; 
    public List<AffectedFaction> affectedFactions;
}

[System.Serializable]
public class AffectedFaction
{
    public FactionType faction;
    public int relationshipChange;
}

public class FactionManager : MonoBehaviour
{
    [System.Serializable]
    public class FactionRelation
    {
        public FactionType factionType;
        public Faction faction;

        public FactionRelation(FactionType type, Faction _faction)
        {
            factionType = type;
            faction = _faction;
        }
    }

    public static FactionManager Instance;
    [SerializeField] private List<FactionRelation> factions;

    public static event Action<FactionType, int> onRelationshipChanged;

    [SerializeField] private List<FactionEffectRule> effectRules;



    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            InitializeFactions();
        }
        else
        {
            Destroy(gameObject);
        }

    }

    void Start()
    {
        InitializeFactions();
    }

    private void InitializeFactions()
    {
        factions = new List<FactionRelation>()
        {
            {new FactionRelation(FactionType.Workers, new Faction("Workers", 0))},
            {new FactionRelation(FactionType.Smugglers, new Faction("Smugglers", 0))},
            {new FactionRelation(FactionType.Security, new Faction("Security", 0))}
        };
        foreach (var relationFromSave in SaveManager.Instance.saveData.factionRelations)
        {
            var currentFactionRelation = factions.FirstOrDefault(fr => fr.factionType == relationFromSave.factionType);
            if (currentFactionRelation != null)
            {
                currentFactionRelation.faction.Relationship = relationFromSave.relationLevel;
                onRelationshipChanged?.Invoke(currentFactionRelation.factionType, currentFactionRelation.faction.Relationship);
            }
        }
    }

    public void UpdateRelationship(FactionType factionType, int amount)
    {
        var relation = factions.FirstOrDefault(fr => fr.factionType == factionType);
        if (relation != null)
        {
            relation.faction.ChangeRelationship(amount);
            onRelationshipChanged?.Invoke(factionType, relation.faction.Relationship);
            CheckFactionEffects(factionType, relation.faction.Relationship);
            var saveRelation = SaveManager.Instance.saveData.factionRelations.FirstOrDefault(fr => fr.factionType == factionType);
            saveRelation.relationLevel = relation.faction.Relationship;

        }
        else
        {
            Debug.LogWarning($"Faction {factionType} not found!");
        }
    }

    private void CheckFactionEffects(FactionType changedFaction, int newRelationshipValue)
    {
        foreach (var rule in effectRules)
        {
            if (rule.triggerFaction == changedFaction && newRelationshipValue >= rule.threshold)
            {
                foreach (var affected in rule.affectedFactions)
                {
                    var affectedRelation = factions.FirstOrDefault(fr => fr.factionType == affected.faction);
                    if (affectedRelation != null)
                    {
                        affectedRelation.faction.ChangeRelationship(affected.relationshipChange);
                        onRelationshipChanged?.Invoke(affected.faction, affectedRelation.faction.Relationship);
                    }
                }
            }
        }
    }


    public int GetFactionRelationship(FactionType factionType)
    {
        var relation = factions.FirstOrDefault(fr => fr.factionType == factionType);
        return relation != null ? relation.faction.Relationship : -200;
    }

    public string GetFaction(FactionType factionType)
    {
        var relation = factions.FirstOrDefault(fr => fr.factionType == factionType);
        return relation != null ? relation.faction.Name : "";
    }
}

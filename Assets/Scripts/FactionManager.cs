using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FactionManager : MonoBehaviour
{
    public static FactionManager Instance;

    public List<Faction> Factions;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    public Faction GetFaction(string name)
    {
        return Factions.Find(f => f.Name == name);
    }

    public void ModifyRelationship(string factionName, int amount)
    {
        Faction faction = GetFaction(factionName);
        if (faction != null)
        {
            faction.ChangeRelationship(amount);
            Debug.Log($"Relationship with {factionName} changed by {amount}. Current: {faction.Relationship}");

            foreach (var relation in faction.RelationsWithOtherFactions)
            {
                Faction relatedFaction = GetFaction(relation.RelatedFactionName);
                if (relatedFaction != null)
                {
                    relatedFaction.ChangeRelationship(-amount * relation.InfluenceFactor);
                    Debug.Log($"Relationship with {relatedFaction.Name} affected due to {factionName}. New: {relatedFaction.Relationship}");
                }
            }
        }
    }

}

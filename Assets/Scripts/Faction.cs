using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class Faction : MonoBehaviour
{
    public string Name;
    public int Relationship;
    public Color FactionColor;

    [System.Serializable]
    public class FactionRelation
    {
        public string RelatedFactionName;
        public int InfluenceFactor; 
    }
    public List<FactionRelation> RelationsWithOtherFactions;
    public Faction(string name, int initialRelationship)
    {
        Name = name;
        Relationship = initialRelationship;
        RelationsWithOtherFactions = new List<FactionRelation>();
    }

    public void ChangeRelationship(int amount)
    {
        Relationship = Mathf.Clamp(Relationship + amount, -100, 100);
    }

}

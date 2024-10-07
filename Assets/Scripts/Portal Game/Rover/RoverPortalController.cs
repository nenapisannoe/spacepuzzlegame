using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoverPortalController : MonoBehaviour
{
    [SerializeField] private GameObject enterPortal;
    [SerializeField] private List<GameObject> exitPortals;
    
    [SerializeField] private Transform enterPortalSpawnPoint;
    [SerializeField] private List<Transform> exitPortalSpawnPoints;
    
    [SerializeField] private Collider2D enterPortalCollider;
    [SerializeField] private List<Collider2D> exitPortalColliders;

    [SerializeField] private List<GameObject> roverClones;
    
    [SerializeField] private GameObject clone;
    
    void Start()
    {
        enterPortalCollider = enterPortal.GetComponent<BoxCollider2D>();
        
        foreach(var portal in exitPortals)
            exitPortalColliders.Add (portal.GetComponent<BoxCollider2D>());
    }
    
    public void SpawnClones(string portal)
    {
        if (portal == "enter")
        {
            var roverClone = Instantiate(clone, enterPortalSpawnPoint.position, Quaternion.identity);
            roverClone.gameObject.name = "Rover clone";
        }
        else if (portal == "exit")
        {
            int i = 0;
            foreach (var spawnPoint in exitPortalSpawnPoints)
            {
                var roverClone = Instantiate(clone, spawnPoint.position, Quaternion.identity);
                roverClone.gameObject.name = "Rover clone";
                i++;
            }
        }
        
    }
    
    public void DisableCollider(string colliderToDisable)
    {
        if (colliderToDisable == "enter")
        {
            enterPortalCollider.enabled = false;
        }
        else if (colliderToDisable == "exit")
        {
            foreach(var col in exitPortalColliders)
                col.enabled = false;
        }
    }

    public void EnableColliders()
    {
        enterPortalCollider.enabled = true;
        foreach(var col in exitPortalColliders)
            col.enabled = true;
    }
    
}

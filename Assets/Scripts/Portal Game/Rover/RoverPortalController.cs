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

    public void SpawnClones(string portal, Vector3 scale, int roverScale)
    {
        if (portal == "enter")
        {
            if (GameObject.Find("Rover"))
                return;
            var roverClone = Instantiate(clone, enterPortalSpawnPoint.position, Quaternion.identity);
            roverClone.transform.localScale= new Vector3(scale.x*2, scale.y*2, scale.z);
            if(roverClone.transform.localScale.x >= 1f)
                roverClone.gameObject.name = "Rover";
            else
            {
                roverClone.GetComponent<MoveRover>().roverScale = roverScale-1;
            }
        }
        else if (portal == "exit")
        {
            int i = 0;
            foreach (var spawnPoint in exitPortalSpawnPoints)
            {
                var roverClone = Instantiate(clone, spawnPoint.position, Quaternion.identity);
                roverClone.transform.localScale= new Vector3(scale.x/2, scale.y/2, scale.z);
                roverClone.gameObject.name = "Rover clone";
                roverClone.GetComponent<MoveRover>().roverScale = roverScale+1;
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

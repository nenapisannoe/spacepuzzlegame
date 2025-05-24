using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class RoverPortalController : MonoBehaviour
{
    [SerializeField] private GameObject enterPortal;
    [SerializeField] private List<GameObject> exitPortals;

    [SerializeField] private Transform enterPortalSpawnPoint;
    [SerializeField] private List<Transform> exitPortalSpawnPoints;

    [SerializeField] private Collider2D enterPortalCollider;
    [SerializeField] private List<Collider2D> exitPortalColliders;
    [SerializeField] private GameObject roverPrefab;
    [SerializeField] private List<GameObject> clonesAtExit = new List<GameObject>(); 
    private bool isRecombining = false;

    [SerializeField] private int portalColor;

    [SerializeField] private bool isPortalOn;

    public Action OnClonesRecombined;

    private void Start()
    {
        enterPortalCollider = enterPortal.GetComponent<Collider2D>();
        foreach (var portal in exitPortals)
        {
            exitPortalColliders.Add(portal.GetComponent<Collider2D>());
        }
        if(isPortalOn)
            EnablePortals();
        else
        {
            DisablePortals();
            Field.OnWireConnected += AcceptPortalEnable;
        }
    }

    void AcceptPortalEnable(int color)
    {
        if(color == portalColor)
            EnablePortals();
    }

    void EnablePortals()
    {
        enterPortal.GetComponent<RoverPortal>().SwitchTransparency(false);
        enterPortalCollider.enabled = true;
        foreach(var exitPortal in exitPortals)
        {
            exitPortal.GetComponent<RoverPortal>().SwitchTransparency(false);
        }
        foreach(var exitPortalCollider in exitPortalColliders)
        {
            exitPortalCollider.enabled = true;
        }
    }

    void DisablePortals()
    {
        enterPortal.GetComponent<RoverPortal>().SwitchTransparency(true);
        enterPortalCollider.enabled = false;
        foreach(var exitPortal in exitPortals)
        {
            exitPortal.GetComponent<RoverPortal>().SwitchTransparency(true);
        }
        foreach(var exitPortalCollider in exitPortalColliders)
        {
            exitPortalCollider.enabled = false;
        }
    }



    public void SpawnClonesAtExit(Vector3 scale, int parentId)
    {
        foreach (var spawnPoint in exitPortalSpawnPoints)
        {
            var clone = Instantiate(roverPrefab, spawnPoint.position, Quaternion.identity);
            clone.transform.localScale = scale / exitPortals.Count;

            int cloneId = RoverManager.Instance.RegisterRover(clone, parentId);
            clone.GetComponent<MoveRover>().RoverId = cloneId;
            clone.GetComponent<MoveRover>().ParentId = parentId;
            clone.GetComponent<MoveRover>().halves = exitPortals.Count;
            if(RoverManager.Instance.isControlled)
                clone.GetComponent<MoveRover>().ShowPointer();
        }

        clonesAtExit.Clear(); 
    }

    public void CloneEnteredExitPortal(GameObject clone)
    {
        if (isRecombining || clonesAtExit.Contains(clone))
            return;

        clonesAtExit.Add(clone);
        Debug.Log($"Clone entered exit portal: {clone.name}. Count at exit: {clonesAtExit.Count}");

        if (clonesAtExit.Count == exitPortals.Count)
        {
            TryRecombineClones();
        }
    }

    public void CloneExitedExitPortal(GameObject clone)
    {
        if (clonesAtExit.Contains(clone))
        {
            clonesAtExit.Remove(clone);
            Debug.Log($"Clone exited exit portal: {clone.name}. Remaining: {clonesAtExit.Count}");
        }
    }

    private void TryRecombineClones()
    {
        int clonesParent = clonesAtExit[0].GetComponent<MoveRover>().ParentId;
        bool sameParent = true;
        foreach(var clone in clonesAtExit)
        {
            if(clone.GetComponent<MoveRover>().ParentId != clonesParent)
            {
                sameParent = false;
                break;
            }
        }
        if(sameParent)
            isRecombining = true;
        else 
            return;

        Vector3 combinedScale = Vector3.zero;
        foreach (var clone in clonesAtExit)
        {
            combinedScale += clone.transform.localScale;
        }
        foreach (var clone in clonesAtExit.ToList())
        {
            Destroy(clone);
            clonesAtExit.Remove(clone);
        }


        var recombinedRover = Instantiate(roverPrefab, enterPortalSpawnPoint.position, Quaternion.identity);
        recombinedRover.transform.localScale = combinedScale;
        int cloneId = RoverManager.Instance.RegisterRover(recombinedRover);
        recombinedRover.GetComponent<MoveRover>().RoverId = clonesParent;

        OnClonesRecombined?.Invoke();
        EnableEnterPortal();

        isRecombining = false;
    }

    private void EnableEnterPortal()
    {
        enterPortalCollider.enabled = true;
    }
}


    


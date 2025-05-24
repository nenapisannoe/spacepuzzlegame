using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

[System.Serializable]

public class RoverManager: MonoBehaviour, IControllable
{
    public static RoverManager Instance { get; private set; }
    public List<MoveRover> ActiveRovers = new List<MoveRover>();
    private Transform roverSpawnPoint;
    GameObject RoverPrefab;
    private int nextRoverId = 1;
    public bool isControlled;

    public void SetControl(bool isActive)
    {
        isControlled = isActive;
        if(isControlled)
        {
            foreach (var r in ActiveRovers)
            {
                r.ShowPointer();
            }
        }
        else 
        {
            foreach (var r in ActiveRovers)
            {
                r.HidePointer();
            }
        }
    }

    void SpawnRover()
    {
        Instantiate(RoverPrefab,roverSpawnPoint.position, Quaternion.identity);
    }

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            return;
        }
        Instance = this;
    }
    public int RegisterRover(GameObject rover, int parentId = 0)
    {
        int roverId = nextRoverId++;
        rover.GetComponent<MoveRover>().RoverId = roverId;
        ActiveRovers.Add(rover.GetComponent<MoveRover>());
        return roverId;
    }

    public void DeregisterRover(MoveRover rover)
    {
        if (ActiveRovers.Contains(rover))
        {
            ActiveRovers.Remove(rover);
        }
    }

    public void KillRovers()
    {
        foreach (var r in ActiveRovers.ToList())
        {
            DeregisterRover(r);
            Destroy(r.gameObject);
        }
        SpawnRover();
    }
}
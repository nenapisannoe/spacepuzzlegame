using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Microsoft.Unity.VisualStudio.Editor;
using UnityEngine;

public class MapUI : MonoBehaviour
{
    private ShipZone[,] Zones;

    [SerializeField] private GameObject Map;
    private (int,int) markedZone = (-1,-1);

    private int _dimensionX = 3;
    private int _dimensionY = 3;

    public static MapUI Instance { get; private set; }

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);

        InitializeMap();
        GameController.instance.OnMapOpen += Switch;
        MarkOnMap(0,0);

        Switch(false);
    }

    public int GetCurrentZoneX()
    {
        return markedZone.Item1;
    }

    public int GetCurrentZoneY()
    {
        return markedZone.Item2;
    }

    public ShipZone GetCurrentZone()
    {
        return Zones[GetCurrentZoneX(),GetCurrentZoneY()];
    }

    public void MarkOnMap(int x, int y)
    {
        if(markedZone != (-1,-1))
            Zones[markedZone.Item1, markedZone.Item2].RemoveMark();
        markedZone = (x,y);
        Zones[markedZone.Item1, markedZone.Item2].AddMark();
    }

    void OnEnable()
    {
        foreach (ShipZone zone in Zones)
        {
            if(zone.relationshipWithFactionRequired <= FactionManager.Instance.GetFactionRelationship(zone.HostFaction))
                zone.Unlock();
        }   
    }
    void InitializeMap()
    { 
        Zones = new ShipZone[_dimensionX, _dimensionY];
        for (int y = 0; y < _dimensionX; y++)
        {
            if(transform.GetChild(y).CompareTag("Row"))
            {
                var row = transform.GetChild(y).transform;
                for (int x = 0; x < _dimensionY; x++)
                {
                    if(row.GetChild(x).CompareTag("Room"))
                    {
                        var room = row.GetChild(x).gameObject;
                        Zones[x,y] = room.GetComponent<ShipZone>();
                        Zones[x,y].SetZoneCoordinates(x,y);
                    }
                }
            }
        }
    }

    
    private void Switch(bool val)
    {
        Map.SetActive(val);
    }

    void Update()
    {
        
    }
}

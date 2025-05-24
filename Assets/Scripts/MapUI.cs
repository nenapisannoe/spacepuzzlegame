using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Microsoft.Unity.VisualStudio.Editor;
using UnityEngine;
using TMPro;
using UnityEngine.UI; 

public class MapUI : MonoBehaviour
{
    private ShipZone[,] Zones;

    [SerializeField] private GameObject Map;
    [SerializeField] private GameObject RejectionWindow;
    [SerializeField] private TMP_Text RejectionText;
    [SerializeField] Button RejectionWindowButton;

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

         RejectionWindowButton.onClick.AddListener(HideRejectionWindow);

    }

    void OnDisable()
    {
        RejectionWindowButton.onClick.RemoveListener(HideRejectionWindow);
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

    public void ShowRejectionWindow(string factionName, int relationRequired)
    {
        RejectionWindow.SetActive(true);
        RejectionText.text = $"Фракция {factionName} пока не открыла вам доступ к этой зоне. Необходимый уровень репутации: {relationRequired}";
    }

    public void ShowExitRejectionWindow(string factionName)
    {
        RejectionWindow.SetActive(true);
        RejectionText.text = $"Фракция {factionName} из-за падения репутации закрыла вам выход на уровень выше из этой зоны. Необходимо найти другой путь.";
    }


    public void HideRejectionWindow()
    {
        RejectionWindow.SetActive(false);
    }

    
    private void Switch(bool val)
    {
        Map.SetActive(val);
    }

    void Update()
    {
        
    }
}

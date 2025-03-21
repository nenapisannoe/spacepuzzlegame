using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Microsoft.Unity.VisualStudio.Editor;
using UnityEngine;

public class MapUI : MonoBehaviour
{
    private GameObject[,] Rooms;

    [SerializeField] private GameObject Map;
    private GameObject Mark;

    private int _dimensionX = 3;
    private int _dimensionY = 3;

    void Awake()
    {

        Debug.Log(GameController.instance);
        GameController.instance.OnMapOpen += Switch;
        Door.OnRoomEntered += MarkOnMap;

        Switch(false);
    }

    void MarkOnMap(int x, int y)
    {
        if(Mark != null && Mark.activeSelf)
            Mark.SetActive(false);
        Mark = Rooms[x,y].transform.Find("Mark").gameObject;
        Mark.SetActive(true);
    }
    void Start()
    { 
        Rooms = new GameObject[_dimensionX, _dimensionY];
        for (int y = 0; y < _dimensionX; y++)
        {
            var row = transform.GetChild(y).transform;
            for (int x = 0; x < _dimensionY; x++)
            {
                var room = row.GetChild(x).gameObject;
                Rooms[x,y] = room;
                //Debug.Log($"{x}, {y}, {room.name}");
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

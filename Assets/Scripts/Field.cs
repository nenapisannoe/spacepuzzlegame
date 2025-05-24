using System;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;

public class Field : MonoBehaviour
{

  [SerializeField] ActivatorColorType ActiateObjectColor;
  private Tile[,] _grid;
  private bool _canDrawConnection = false;

  [SerializeField] private List<Tile> _connections = new List<Tile>();
  private Tile _connectionTile;

  private List<int> _solvedConnections = new List<int>();

  public static Action<ActivatorColorType> OnWiresSolved;
  public static Action<int> OnWireConnected;

  Dictionary<int, List<Tile>> connectionsByCid = new Dictionary<int, List<Tile>>();

  int currentCid = 0;



  private int _dimensionX = 0;
  private int _dimensionY = 0;
  private int _solved = 0;
  private Dictionary<int, int> _amountToSolve = new Dictionary<int, int>();

  private Dictionary<int, List<Tile>> connections = new Dictionary<int, List<Tile>>();

  void Start()
  {
    _dimensionX = transform.childCount;
    _dimensionY = transform.GetChild(0).transform.childCount;
    _grid = new Tile[_dimensionY, _dimensionX];

    for (int y = 0; y < _dimensionX; y++)
    {
      var row = transform.GetChild(y).transform;
      row.gameObject.name = "" + y;
      for (int x = 0; x < _dimensionY; x++)
      {
        var tile = row.GetChild(x).GetComponent<Tile>();
        tile.gameObject.name = "" + x;
        tile.onSelected.AddListener(onTileSelected);
        _CollectAmountToSolveFromTile(tile);
        _grid[x, y] = tile;
      }
    }
    //SetGameStatus(_solved, _amountToSolve.Count);
    _OutputGrid();
    foreach (var x in _amountToSolve)
    {
      if (x.Value % 2 != 0)
        Debug.Log("wow ur stupid: " + x.Key);
    }
  }

  void _CollectAmountToSolveFromTile(Tile tile)
  {
    if (tile.cid > Tile.UNPLAYABLE_INDEX)
    {
      if (_amountToSolve.ContainsKey(tile.cid))
        _amountToSolve[tile.cid] += 1;
      else _amountToSolve[tile.cid] = 1;
    }
  }

  void _OutputGrid()
  {
    var results = "";
    int dimension = transform.childCount;
    for (int y = 0; y < dimension; y++)
    {
      results += "{";
      var row = transform.GetChild(y).transform;
      for (int x = 0; x < row.childCount; x++)
      {
        var tile = _grid[x, y];
        if (x > 0) results += ",";
        results += tile.cid;
      }
      results += "}\n";
    }
    //Debug.Log("Main -> Start: _grid: \n" + results);
  }

  Vector3 _mouseWorldPosition;
  int _mouseGridX, _mouseGridY;

  void Update()
  {
    if (_canDrawConnection)
    {
      _mouseWorldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
      Vector3 localPos = transform.InverseTransformPoint(_mouseWorldPosition);
      _mouseGridX = Mathf.FloorToInt(localPos.x);
      _mouseGridY = Mathf.FloorToInt(localPos.y);



      //Debug.Log($"Mouse Grid Y: {_dimensionX}, Dimension Y: {_dimensionY}");

      if (_CheckMouseOutsideGrid()) return;

      Tile hoverTile = _grid[_mouseGridX, _mouseGridY];
      Tile firstTile = _connections[0];
      currentCid = firstTile.cid;
      /*if (hoverTile._isPartOfConnection)
      {
          Debug.Log("Field -> OnMouseDrag: Erasing intersected connection");
          _EraseConnection(hoverTile);
      }*/
      bool isDifferentActiveTile = hoverTile.cid > 0 && hoverTile.cid != firstTile.cid;

      if (hoverTile.isHighlighted || hoverTile.isSolved || isDifferentActiveTile) return;

      Vector2 connectionTilePosition = _FindTileCoordinates(_connectionTile);
      bool isPositionDifferent = IsDifferentPosition(_mouseGridX, _mouseGridY, connectionTilePosition);

      //Debug.Log("Field -> OnMouseDrag(" + isPositionDifferent + "): " + _mouseGridX + "|" + _mouseGridY);

      if (isPositionDifferent)
      {
        var deltaX = System.Math.Abs(connectionTilePosition.x - _mouseGridX);
        var deltaY = System.Math.Abs(connectionTilePosition.y - _mouseGridY);
        bool isShiftNotOnNext = deltaX > 1 || deltaY > 1;
        bool isShiftDiagonal = (deltaX > 0 && deltaY > 0);
        //Debug.Log("Field -> OnMouseDrag: isShiftNotOnNext = " + isShiftNotOnNext + "| isShiftDiagonal = " + isShiftDiagonal);
        if (isShiftNotOnNext || isShiftDiagonal) return;

        hoverTile.Highlight();
        hoverTile.SetConnectionColor(_connectionTile.ConnectionColor);

        _connectionTile.ConnectionToSide(
          _mouseGridY > connectionTilePosition.y,
          _mouseGridX > connectionTilePosition.x,
          _mouseGridY < connectionTilePosition.y,
          _mouseGridX < connectionTilePosition.x
        );
        


        _connectionTile = hoverTile;
        _connections.Add(_connectionTile);
        _connectionTile._isPartOfConnection = true;

        if (_CheckIfTilesMatch(hoverTile, firstTile))
        {
          _connections.ForEach((tile) => tile.isSolved = true);
          _connections.ForEach((tile) => tile.connectionCid = currentCid);
          _canDrawConnection = false;
          _amountToSolve.Remove(firstTile.cid);
          OnWireConnected?.Invoke(firstTile.cid);
          connectionsByCid.Add(currentCid, _connections);
          
          if (_amountToSolve.Keys.Count == 0)
          {
            //Debug.Log("GAME COMPLETE");
            gameObject.SetActive(false);
            Debug.Log("hi");
            OnWiresSolved?.Invoke(ActiateObjectColor);
          }
        }
      }
    }
  }

  bool _CheckIfTilesMatch(Tile tile, Tile another)
  {
    return tile.cid > 0 && another.cid == tile.cid;
  }

  bool _CheckMouseOutsideGrid()
  {
    //Debug.Log("_mouseGridY >= _dimensionY || _mouseGridY < 0 || _mouseGridX >= _dimensionX || _mouseGridX < 0");
    //Debug.Log($"{_mouseGridY} >= {_dimensionY} || {_mouseGridY} < 0 || {_mouseGridX} >= {_dimensionX} || {_mouseGridX} < 0;");
    return _mouseGridY >= _dimensionX || _mouseGridY < 0 || _mouseGridX >= _dimensionY || _mouseGridX < 0;
  }

  void _EraseConnection(Tile tile)
  {
    if (connectionsByCid.ContainsKey(tile.connectionCid))
    {
        foreach (Tile t in connectionsByCid[tile.connectionCid])
        {
            tile.connectionCid = 0;
            t.ResetConnection();
            t._isPartOfConnection = false;
            t.isSolved = false;
            t.HightlightReset();
        }
        connectionsByCid.Remove(tile.connectionCid);
    }
  }


  void onTileSelected(Tile tile)
{
    //Debug.Log("Field -> onTileSelected(" + tile.isSelected + "): " + _FindTileCoordinates(tile));

    if (tile._isPartOfConnection)
    {
        //Debug.Log("Field -> onTileSelected: Erasing connection");
        _EraseConnection(tile);
        return;
    }

    if (tile.isSelected)
    {
        _connectionTile = tile;
        _connections = new List<Tile>();
        _connections.Add(_connectionTile);
        _canDrawConnection = true;
        _connectionTile.Highlight();
    }
    else
    {
        bool isFirstTileInConnection = _connectionTile == tile;
        if (isFirstTileInConnection)
        {
            tile.HightlightReset();
        }
        else if (!_CheckIfTilesMatch(_connectionTile, tile))
        {
            _ResetConnections();
        }
        _canDrawConnection = false;
    }

    //Debug.Log($"[onTileSelected] _connectionTile set to: {_FindTileCoordinates(_connectionTile)}");
}


  public void onRestart()
  {
    //Debug.Log("Field -> onRestart");
    int dimension = transform.childCount;
    for (int y = 0; y < dimension; y++)
    {
      var row = transform.GetChild(y).transform;
      for (int x = 0; x < row.childCount; x++)
      {
        var tile = _grid[x, y];
        tile.ResetConnection();
        tile.HightlightReset();
        _CollectAmountToSolveFromTile(tile);
      }
    }
    _solved = 0;
  }



void _ResetConnections()
{
    Debug.Log($"Field -> _ResetConnections: Clearing connections {currentCid}");
    foreach (Tile tile in _connections)
    {
        tile.ResetConnection();
        tile._isPartOfConnection = false;
        tile.HightlightReset();
    }
    _connections.Clear();
}

  Vector2 _FindTileCoordinates(Tile tile)
  {
    int x = int.Parse(tile.gameObject.name);
    int y = int.Parse(tile.gameObject.transform.parent.gameObject.name);
    return new Vector2(x, y);
  }

  public bool IsDifferentPosition(int gridX, int gridY, Vector2 position)
  {
    return position.x != gridX || position.y != gridY;
  }

  private class Connection
  {
    public Tile tile;
    public Vector2 position;
    public Connection(Tile tile, Vector2 position)
    {
      this.tile = tile;
      this.position = position;
    }

    public bool IsDifferentPosition(int gridX, int gridY)
    {
      return this.position.x != gridX || this.position.y != gridY;
    }
  }
}

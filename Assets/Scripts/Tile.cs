using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Tile : MonoBehaviour
{
  static public int UNPLAYABLE_INDEX = 0;
  static public Color COLOR_HIGHTLIGHT = new Color(1, 1, 0, 0.05f);
  static public string NAME_CONNECTION = "Connection";
  static public string NAME_BACK = "Back";
  static public string NAME_MAK = "Mark";

  public int connectionCid;

  public int cid = 0;
  [HideInInspector]
  public UnityEvent<Tile> onSelected;

  public bool isSelected
  {
    get { return _isSelected; }
    private set { this._isSelected = value; }
  }

  public bool isHighlighted
  {
    get { return _isHighlighted; }
    private set { this._isHighlighted = value; }
  }

  public bool isSolved
  {
    get { return this._isSolved; }
    set { this._isSolved = value; }
  }

  public bool isPlayble
  {
    get { return this._isPlayble; }
    private set { this._isPlayble = value; }
  }

  private SpriteRenderer BackComponentRenderer
  {
    get { return this.transform.Find(NAME_BACK).gameObject.GetComponent<SpriteRenderer>(); }
  }

  public Color ConnectionColor
  {
    get
    {
      return this.ConnectionComponentRenderer.color;
    }
    private set { }
  }

  public SpriteRenderer ConnectionComponentRenderer
  {
    get
    {
      return this.transform.Find(NAME_CONNECTION)
      .gameObject.transform.Find("Pipe")
      .gameObject.GetComponent<SpriteRenderer>();
    }
    private set { }
  }

  private SpriteRenderer MarkComponentRenderer
  {
    get { return this.transform.Find(NAME_MAK).gameObject.GetComponent<SpriteRenderer>(); }
  }

  [SerializeField] private bool _isSolved = false;
  private bool _isHighlighted = false;
   [SerializeField] private bool _isPlayble = false;
  private bool _isSelected = false;
  public bool _isPartOfConnection = false;

  private Color _originalColor;
  void Start()
  {
    _isPlayble = cid > UNPLAYABLE_INDEX;
    _originalColor = BackComponentRenderer.color;
    if (_isPlayble)
      SetConnectionColor(MarkComponentRenderer.color);
    //else
      //Destroy(MarkComponentRenderer.gameObject);
  }


public void ResetConnection()
{
    var connection = this.transform.Find(NAME_CONNECTION).gameObject;
    connection.SetActive(false);
    connection.transform.eulerAngles = Vector3.zero;
    Debug.Log("Tile -> Reset(" + _isSolved + "): " + cid);
    _isSolved = false;
    _isPartOfConnection = false; 
}
  public void HightlightReset()
  {
    _isHighlighted = false;
    BackComponentRenderer.color = _originalColor;
  }

  public void Highlight()
  {
    _isHighlighted = true;
    BackComponentRenderer.color = COLOR_HIGHTLIGHT;
  }

  public void SetConnectionColor(Color color)
  {
    ConnectionComponentRenderer.color = color;
  }

  public void ConnectionToSide(bool top, bool rigth, bool bottom, bool left)
  {
    this.transform.Find(NAME_CONNECTION).gameObject.SetActive(true);
    int angle = rigth ? -90 : bottom ? -180 : left ? -270 : 0;
    this.transform.Find(NAME_CONNECTION).gameObject.transform.Rotate(new Vector3(0, 0, angle));
  }

  void OnMouseUp()
  {
    if (_isPlayble || _isPartOfConnection)
    {
      _isSelected = false;
      InvokeOnSelected();
    }
  }

  void OnMouseDown()
  {
    if (_isPlayble)
    {
      _isSelected = true;
      InvokeOnSelected();
    }
  }

  void InvokeOnSelected()
  {
    if (onSelected != null)
      onSelected.Invoke(this.GetComponent<Tile>());
  }
}

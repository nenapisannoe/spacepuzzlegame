using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WiresField : MonoBehaviour
{
    public GameObject dotPrefab; // Prefab for dot
    public GameObject linePrefab; // Prefab for line

    private int _dimensionX = 0;
    private int _dimensionY = 0;

    [SerializeField] public WiresTile[,] grid; 

    private List<GameObject> dots = new List<GameObject>();
    private List<GameObject> lines = new List<GameObject>();
    private GameObject currentLine;

    void Start()
    {
        CreateGrid();
        PlaceDots();
    }

    void CreateGrid()
    {
        _dimensionX = transform.GetChild(0).transform.childCount;
        _dimensionY = transform.childCount;
        
        grid = new WiresTile[_dimensionY, _dimensionX];
        Debug.Log("X: " + _dimensionX + "Y: " +_dimensionY);

        for (int i = 0; i < _dimensionY; i++)
        {
            var row = transform.GetChild(i).transform;
            for (int j = 0; j < _dimensionX; j++)
            {
                var tile = row.GetChild(j).GetComponent<WiresTile>();
                grid[i, j] = tile;
                tile.onTileSelected += onTilePressed;
            }
        }
    }

    void onTilePressed(WiresTile tile)
    {
        if (tile.isPressed)
        {
            tile.GetComponent<SpriteRenderer>().color = Color.black;
        }
        else
        {
            tile.GetComponent<SpriteRenderer>().color = Color.white;
        }
    }

    void PlaceDots()
    {
        // Place dots randomly on the grid
        for (int i = 0; i < 5; i++) // Adjust number of dots as needed
        {
           // Vector3 dotPosition = new Vector3(Random.Range(0, gridSizeX) * tileSize, Random.Range(0, gridSizeY) * tileSize, 0);
            //GameObject dot = Instantiate(dotPrefab, dotPosition, Quaternion.identity);
            //dots.Add(dot);
        }
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0)) 
        {
            Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Debug.Log(mousePosition.x + "," + mousePosition.y + "," + mousePosition.z);
            mousePosition.z = 0;

            // Check if mouse is over a dot
            Collider2D hit = Physics2D.OverlapPoint(mousePosition);
            if (hit != null && hit.gameObject.tag == "Dot")
            {
                if (currentLine == null) // If no line is being drawn, create a new line
                {
                    currentLine = Instantiate(linePrefab);
                    currentLine.GetComponent<LineRenderer>().positionCount = 2;
                    currentLine.GetComponent<LineRenderer>().SetPosition(0, hit.transform.position);
                    currentLine.GetComponent<LineRenderer>().SetPosition(1, hit.transform.position);
                }
                else // If a line is already being drawn, add this dot to it
                {
                    currentLine.GetComponent<LineRenderer>().SetPosition(1, hit.transform.position);
                }
            }
        }

        if (Input.GetMouseButtonUp(0) && currentLine != null) // Check if left mouse button is released
        {
            // Check if mouse is over a dot
            Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mousePosition.z = 0;
            Collider2D hit = Physics2D.OverlapPoint(mousePosition);
            if (hit != null && hit.gameObject.tag == "Dot")
            {
                // Connect dots with line
                currentLine.GetComponent<LineRenderer>().SetPosition(1, hit.transform.position);
            }
            else // If mouse is not over a dot, delete the line
            {
                Destroy(currentLine);
            }
            currentLine = null;
        }
    }
}

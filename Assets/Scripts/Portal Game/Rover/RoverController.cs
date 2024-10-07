using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoverController : MonoBehaviour
{
    [SerializeField] GameObject rover;
    public RoverPositionData positionData;
    private Vector2 initialPosition;
    private Vector2 currentPosition;
    public int levelIndex;
    

    void Start()
    {
        SetInitialPosition();
    }

    void SetInitialPosition()
    {
        initialPosition = positionData.initialPositions[levelIndex];
    }

    public void RespawnRover()
    {
        /*currentPosition = initialPosition;
        transform.position = currentPosition;*/
        Instantiate(rover, initialPosition, Quaternion.identity);
    }

    public void OnRoverDestroyed()
    {
        RespawnRover();
    }
}

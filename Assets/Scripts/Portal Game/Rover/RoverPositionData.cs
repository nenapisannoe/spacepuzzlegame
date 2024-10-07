using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "RoverPositionData", menuName = "Rover/PositionData")]
public class RoverPositionData : ScriptableObject
{
    public Vector2[] initialPositions;
}

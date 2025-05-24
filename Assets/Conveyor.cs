using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Conveyor : ColorBasedActivatableObject
{
    [SerializeField] public Vector2 moveDirection = new Vector2(1, 0);
    [SerializeField] public float moveSpeed = 5f;
    [SerializeField] bool isDeactivated;

    [SerializeField] private List<MoveRover> RoversOnConveyor = new List<MoveRover>();
    public float scrollSpeed = 0.5f;
    void Start()
    {
        Field.OnWiresSolved += ActivateConveyor;
    }
    void ActivateConveyor(ActivatorColorType incomingColor)
    {
        if (ActivatableObjectColor == incomingColor)
        {   
            isDeactivated = false;
            foreach (var rover in RoversOnConveyor)
            {
                if (moveDirection.x < 0)
                {
                    rover.gameObject.transform.rotation = Quaternion.Euler(0f, 180f, 0f);
                }
                rover.RoverOnConveyor(moveSpeed * moveDirection.x);
            }
        }
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Rover"))
            return;
        var moveRover = collision.GetComponent<MoveRover>();
        RoversOnConveyor.Add(moveRover);
        if (isDeactivated) return;
        if (moveDirection.x < 0)
        {
            collision.gameObject.transform.rotation = Quaternion.Euler(0f, 180f, 0f);
        }
        moveRover.RoverOnConveyor(moveSpeed * moveDirection.x);
    }   

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (!collision.CompareTag("Rover"))
            return;
        var moveRover = collision.GetComponent<MoveRover>();
        RoversOnConveyor.Remove(moveRover);
        moveRover.RoverOffConveyor();
        RoversOnConveyor.Remove(moveRover);
        
    }
    
}

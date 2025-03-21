using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Conveyor : MonoBehaviour
{
    public static Action<int, float> OnRoverStepOnConveyor;
    public static Action<int> OnRoverStepOffConveyor;
    [SerializeField] int ActivatableObjectID;
    [SerializeField] public Vector2 moveDirection = new Vector2(1, 0); 
    [SerializeField] public float moveSpeed = 5f;
    [SerializeField] bool isDeactivated;


    void Start()
    {
        Field.OnWiresSolved += ActivateConveyor;
    }
    void ActivateConveyor(int incomingID)
    {
        if(incomingID == ActivatableObjectID)
            isDeactivated = false;
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(isDeactivated) return;
        if (collision.CompareTag("Rover"))
        {
            OnRoverStepOnConveyor?.Invoke(collision.GetComponent<MoveRover>().RoverId, moveSpeed * moveDirection.x);
        }
    }
    void OnTriggerStay2D(Collider2D other)
    {
     /*   if(!isDeactivated)
            OnRoverStepOnConveyor?.Invoke(other.GetComponent<MoveRover>().RoverId, moveSpeed); */
    }


    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Rover"))
        {
            OnRoverStepOffConveyor?.Invoke(collision.GetComponent<MoveRover>().RoverId);
        }
    }
}

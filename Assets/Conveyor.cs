using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Conveyor : ColorBasedActivatableObject
{
    public static Action<int, float> OnRoverStepOnConveyor;
    public static Action<int> OnRoverStepOffConveyor;
    [SerializeField] public Vector2 moveDirection = new Vector2(1, 0); 
    [SerializeField] public float moveSpeed = 5f;
    [SerializeField] bool isDeactivated;


    void Start()
    {
        Field.OnWiresSolved += ActivateConveyor;
    }
    void ActivateConveyor(ButtonColorType incomingColor)
    {
        if(ActivatableObjectColor == incomingColor)
            isDeactivated = false;
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(isDeactivated) return;
        if(moveDirection.x < 0) 
        {   
            Debug.Log("hello");
            collision.gameObject.transform.rotation = Quaternion.Euler(0f, 180f, 0f);
        }
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

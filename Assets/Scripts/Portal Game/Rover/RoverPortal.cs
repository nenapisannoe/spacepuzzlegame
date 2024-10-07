using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoverPortal : MonoBehaviour
{
    private Rigidbody2D enteredRigidbody;
    private float enterVelocity;
    private float exitVelocity;

    [SerializeField] RoverPortalController _roverPortalController;
    
    private void OnTriggerEnter2D(Collider2D col)
    {
        if (!col.CompareTag("Rover") || GameObject.Find("Rover clone"))
            return;
        enteredRigidbody = col.gameObject.GetComponent<Rigidbody2D>();
        enterVelocity = enteredRigidbody.velocity.x;

        if (gameObject.name == "EnterPortal")
        {
            _roverPortalController.DisableCollider("exit");
            _roverPortalController.SpawnClones("exit");
            //FindObjectOfType<PlayerState>().direction = PlayerState.DirectionEnum.LEFT;
        }
        else if (gameObject.name == "ExitPortal")
        {
            _roverPortalController.DisableCollider("enter");
            _roverPortalController.SpawnClones("enter");
            //FindObjectOfType<PlayerState>().direction =  PlayerState.DirectionEnum.RIGHT;
        }
    }
    
    private void OnTriggerExit2D(Collider2D col)
    {
        if (!col.CompareTag("Rover"))
            return;
        exitVelocity = enteredRigidbody.velocity.x;

        if (enterVelocity != exitVelocity)
        {
            Destroy(GameObject.Find("Rover clone"));
        }
        else if (gameObject.name != "Rover clone")
        {
            Destroy(col.gameObject);
            _roverPortalController.EnableColliders();
            GameObject.Find("Rover clone").name = "Rover";
        }
    }
}

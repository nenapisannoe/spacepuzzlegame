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
        if (!col.CompareTag("Rover"))
            return;
        enteredRigidbody = col.gameObject.GetComponent<Rigidbody2D>();
        Debug.Log(enteredRigidbody);
        enterVelocity = enteredRigidbody.velocity.x;
        var roverScale = col.GetComponent<MoveRover>().roverScale;
        if (gameObject.name == "EnterPortal")
        {
            _roverPortalController.DisableCollider("exit");
            _roverPortalController.SpawnClones("exit",col.gameObject.transform.localScale, roverScale);
        }
        else if (gameObject.name == "ExitPortal")
        {
            _roverPortalController.DisableCollider("enter");
            _roverPortalController.SpawnClones("enter",col.gameObject.transform.localScale,roverScale);
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
        }
    }
}

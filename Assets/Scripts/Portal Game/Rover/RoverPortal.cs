using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoverPortal : MonoBehaviour
{
    [SerializeField] private RoverPortalController portalController;

    private void Start()
    {
        portalController = GetComponentInParent<RoverPortalController>();
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (!col.CompareTag("Rover"))
            return;

        var rover = col.GetComponent<MoveRover>();

        if (gameObject.name == "EnterPortal")
        {
            portalController.SpawnClonesAtExit(col.transform.localScale, rover.roverScale, rover.ParentId);
            RoverManager.Instance.DeregisterRover(rover.RoverId);
            Destroy(col.gameObject);
        }
        else if (gameObject.name == "ExitPortal")
        {
            portalController.CloneEnteredExitPortal(col.gameObject);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (!other.CompareTag("Rover"))
            return;

        if (gameObject.name == "ExitPortal")
        {
            portalController.CloneExitedExitPortal(other.gameObject);
        }
    }
}


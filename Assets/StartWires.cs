using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartWires : MonoBehaviour
{
    [SerializeField] private GameObject wires;
    bool playerNear = false;

    private void OnTriggerStay2D(Collider2D other)
    {
        playerNear = true;
    
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        playerNear = false;
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.E))
           wires.SetActive(true);
    }
}

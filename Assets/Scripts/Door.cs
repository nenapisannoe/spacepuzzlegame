using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Door : MonoBehaviour
{
    public int GoToScene = 0;
    private bool isInTrigger = false;
    private void OnTriggerEnter2D(Collider2D col)
    {
        isInTrigger = true;
    }

    private void OnTriggerExit2D(Collider2D col)
    {
        isInTrigger = false;
    }

    private void EnterDoor()
    {
        if(GameController.instance.PuzzlezAvailable.Contains(GoToScene))
            SceneManager.LoadScene(GoToScene);
    }


    private void Update()
    {
        if (isInTrigger && Input.GetKeyDown(KeyCode.E))
        {
            EnterDoor();
        }
    }

}

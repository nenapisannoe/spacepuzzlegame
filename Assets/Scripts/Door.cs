using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Door : MonoBehaviour
{
    [SerializeField] int x = 0;
    [SerializeField] int y = 0;

    public static Action<int,int> OnRoomEntered;
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
        OnRoomEntered?.Invoke(x,y);
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

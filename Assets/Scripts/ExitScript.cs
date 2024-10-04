using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ExitScript : MonoBehaviour
{
    public int GoToScene = -1;
    
    private void OnTriggerEnter2D(Collider2D col)
    {
        if(GoToScene == -1)
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        else
            SceneManager.LoadScene(GoToScene);
    }
}

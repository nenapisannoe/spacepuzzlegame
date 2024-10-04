using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Door : MonoBehaviour
{
    public int GoToScene = 0;
    
    private void OnTriggerStay2D(Collider2D col)
    {
        if(Input.GetKeyDown(KeyCode.E))
            SceneManager.LoadScene(GoToScene);
    }
}

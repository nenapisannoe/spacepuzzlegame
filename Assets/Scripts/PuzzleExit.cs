using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PuzzleExit : MonoBehaviour
{
    public string PuzzleID;
    private void OnTriggerEnter2D(Collider2D col)
    {
        QuestEvents.OnPuzzleCompleted?.Invoke(PuzzleID); 
        SceneManager.LoadScene(1);
    } 
}

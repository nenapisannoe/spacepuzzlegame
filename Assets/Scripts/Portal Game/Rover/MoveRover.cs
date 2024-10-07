using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveRover : MonoBehaviour
{
    private Rigidbody2D rb;
    [SerializeField] private float moveSpeed = 3f;
    [SerializeField] private float dirX;
    [SerializeField] RoverController roverController;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        roverController = FindObjectOfType<RoverController>();
    }
    void Update()
    {
        var movement = Input.GetAxisRaw("Horizontal");
        
        if (movement < 0)
        {
            transform.rotation = Quaternion.Euler(0f, 180f, 0f);
        }
        else if (movement > 0)
        {
            transform.rotation = Quaternion.Euler(0f, 0f, 0f);
        }
        
        dirX = movement * moveSpeed;
    }

    private void FixedUpdate()
    {
        rb.velocity = new Vector2(dirX, 0f);
    }

    public void KillRover()
    {
        roverController.RespawnRover();
        Destroy(gameObject);
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MoveCharacter : MonoBehaviour, IControllable
{
    private Rigidbody2D rb;
    private PlayerState _playerState;
    [SerializeField] private float moveSpeed = 3f;
    [SerializeField] private float dirX;
    private bool isControlled = true;
    [SerializeField] GameObject pointer;
    
    public Transform currentFloor; 

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Floor"))
        {
            currentFloor = collision.transform;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Floor"))
        {
            if (currentFloor == collision.transform)
                currentFloor = null;
        }
    }

    public void SetControl(bool isActive)
    {
        isControlled = isActive;
        if (isControlled)
            pointer.SetActive(true);
        else
            pointer.SetActive(false);
    }
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        _playerState = FindObjectOfType<PlayerState>();
    }
    void Update()
    {
        if(!isControlled) return;
        dirX = Input.GetAxisRaw("Horizontal") * moveSpeed;
    }

    private void FixedUpdate()
    {
        rb.velocity = new Vector2(dirX, 0f);
    }
}

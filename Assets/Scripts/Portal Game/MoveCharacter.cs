using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveCharacter : MonoBehaviour
{
    private Rigidbody2D rb;
    private PlayerState _playerState;
    [SerializeField] private float moveSpeed = 3f;
    [SerializeField] private float dirX;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        _playerState = FindObjectOfType<PlayerState>();
    }
    void Update()
    {
        if (GameController.instance.SideWalkRuleOn)
        {
            if (_playerState.direction == PlayerState.DirectionEnum.LEFT)
            {
                dirX = -Math.Abs(Input.GetAxisRaw("Horizontal") * moveSpeed);
            }
            else if (_playerState.direction == PlayerState.DirectionEnum.RIGHT)
            {
                dirX = Math.Abs(Input.GetAxisRaw("Horizontal") * moveSpeed);
            }
            else
            {
                dirX = Input.GetAxisRaw("Horizontal") * moveSpeed;
            }
        }
        else
            dirX = Input.GetAxisRaw("Horizontal") * moveSpeed;
    }

    private void FixedUpdate()
    {
        rb.velocity = new Vector2(dirX, 0f);
    }
}

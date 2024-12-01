/*
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    [SerializeField] private CharacterController pC;
    [SerializeField] private float walkSpeed = 2.0f;
    [SerializeField] private float sprintSpeed = 5.0f;
    private float _moveSpeed;

    private Vector3 _playerVelocity;
    private bool _playerGrounded;
    [SerializeField] private float jumpingHeight = 1.0f;
    [SerializeField] private float gravityForce = -9.81f;

    private int _jumpAmount = 0;
    private int _maxJumps = 2;
    private bool _isJumping;
    
    
    private void Update()
    {
        PlayerGrounded();
        //PlayerMovement();
        PlayerJumping();
        
        _playerVelocity.y += gravityForce * Time.deltaTime;
        pC.Move(_playerVelocity * Time.deltaTime);
    }

    /*private void PlayerMovement()
    {
        if (Input.GetKey(KeyCode.LeftShift))
        {
            _moveSpeed = sprintSpeed;
        }
        else
        {
            _moveSpeed = walkSpeed;
        }
        var x = Input.GetAxis("Horizontal");
        var y = Input.GetAxis("Vertical");
        
        Vector3 moveDirection = new Vector3(x, 0f, y).normalized;
        Vector3 move = transform.TransformDirection(moveDirection);
        
        pC.Move(move * (_moveSpeed * Time.deltaTime));
    }#1#

    private void PlayerJumping()
    {
        if (Input.GetKeyDown(KeyCode.Space) && (_playerGrounded || (_jumpAmount < _maxJumps && !_isJumping)))
        {
            _isJumping = true;
            _playerVelocity.y = Mathf.Sqrt(jumpingHeight * -3.0f * gravityForce);
            _jumpAmount++;
        }

        if (Input.GetKeyUp(KeyCode.Space))
        {
            _isJumping = false;
        }
    }

    private void PlayerGrounded()
    {
        _playerGrounded = pC.isGrounded;
        //_playerGrounded = Physics.Raycast(transform.position, Vector3.down, 1.1f);
        if (_playerGrounded)
        {
            _jumpAmount = 0;
        }
    }
}
*/

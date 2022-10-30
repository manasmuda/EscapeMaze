using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class PlayerMovementController : MonoBehaviour {


    public Transform playerTransform;
    public Rigidbody playerRb;
    public Animator playerAnimator;
    public float walkSpeed = 185f, runSpeed = 280f;

    private int _moveXId, _moveYId;
    
    [Inject] private PlayerMovementInputManager _playerMovementInputManager;

    private void Awake() {
        _moveXId = Animator.StringToHash("MoveX");
        _moveYId = Animator.StringToHash("MoveY");
    }

    void Update()
    {
        RotatePlayerByInput();
        MovePlayerByInput();
        AnimatePlayerByInput();
    }

    private void RotatePlayerByInput() {
        playerTransform.Rotate(Vector3.up * _playerMovementInputManager.MouseXAxis);
    }

    private void MovePlayerByInput() {
        float currentSpeed = 0f;
        float yAxis = _playerMovementInputManager.JoyStickYAxis;
        if (yAxis >= 0.7f) {
            currentSpeed = runSpeed;
        } else if (yAxis >= 0.1f) {
            currentSpeed = walkSpeed;
        } else if (yAxis == 0f) {
            currentSpeed = 0f;
        } else if (yAxis <0) {
            currentSpeed = walkSpeed;
        }
        
        Vector2 movementDirection = _playerMovementInputManager.JoystickVector * (currentSpeed * Time.deltaTime);
        playerRb.velocity = playerTransform.right * movementDirection.x + playerTransform.forward * movementDirection.y;
    }
    
    private void AnimatePlayerByInput() {
       
        playerAnimator.SetFloat(_moveXId, _playerMovementInputManager.JoyStickXAxis);
        playerAnimator.SetFloat(_moveYId, _playerMovementInputManager.JoyStickYAxis);
    }
}

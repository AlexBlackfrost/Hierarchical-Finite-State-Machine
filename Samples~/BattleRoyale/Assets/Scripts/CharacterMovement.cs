using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class CharacterMovement {
    [field:SerializeField] public float Speed { get; private set; } = 8;
    [field: SerializeField] public float RotationSpeed { get; private set; } = 8;
    [field: SerializeField] public float Gravity { get; private set; } = -9.8f;

    private CharacterController characterController;
    private Transform transform;
    private float speedY;

    public void Init(CharacterController characterController) {
        this.characterController = characterController;
        transform = characterController.gameObject.transform;
    }

    public void MoveTowards(Vector3 target) {
        Vector3 moveDirection = (target - transform.position);
        moveDirection.y = 0;
        moveDirection.Normalize();
        Vector3 velocity = moveDirection * Speed;

        speedY += Gravity * Time.deltaTime;
        velocity.y = speedY;

        characterController.Move(velocity * Time.deltaTime);


        Quaternion targetRotation = Quaternion.LookRotation(moveDirection);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, RotationSpeed * Time.deltaTime);

        if (characterController.isGrounded) {
            speedY = 0;
        }
    }

    public void SetDetectCollisions(bool detectCollisions) {
        characterController.detectCollisions = detectCollisions;
    }

    public void SetEnableOverlapRecovery(bool enabled) {
        characterController.enableOverlapRecovery = enabled;
    }
}


using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VRHampi
{
    public class PlayerController : MonoBehaviour
    {
        [Header("Movement Settings")]
        [SerializeField] private float moveSpeed = 5f;

        private CharacterController characterController;

        private void Awake()
        {
            characterController = GetComponent<CharacterController>();
            if (characterController == null)
            {
                Debug.LogError("CharacterController is not attached to the Player.");
            }
        }

        private void Update()
        {
            HandleMovement();
        }

        private void HandleMovement()
        {
            float moveX = Input.GetAxis("Horizontal"); 
            float moveZ = Input.GetAxis("Vertical");

            Vector3 moveDirection = new Vector3(moveX, 0, moveZ).normalized;

            if (moveDirection.magnitude >= 0.1f)
            {
                // Move the player
                characterController.Move(moveDirection * moveSpeed * Time.deltaTime);

                // Rotate the player in the direction of movement
                Quaternion toRotation = Quaternion.LookRotation(moveDirection, Vector3.up);
                transform.rotation = Quaternion.Lerp(transform.rotation, toRotation, Time.deltaTime * 10f);
            }
        }
    }
}

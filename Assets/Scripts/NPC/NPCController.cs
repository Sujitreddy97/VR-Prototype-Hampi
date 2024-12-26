using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VRHampi.NPC
{
    public class NPCController : MonoBehaviour
    {
        [Header("CharacterController")]
        [SerializeField] private CharacterController npcCharacterController;

        [Header("Animation Controller")]
        [SerializeField] private NPCAnimationController npcAnimationController;

        [Header("Scriptable Object Reference")]
        [SerializeField] private NPCSO npcSO;

        [Header("Waypoints")]
        [SerializeField] private Transform[] waypoints;

        private int currentWaypointIndex = 0;
        private NPCState currentState;
        private float stateTimer = 0f;

        private void Awake()
        {
            /*characterController = GetComponent<CharacterController>();

            if (npcSO == null)
            {
                Debug.LogError("NPCSO is not assigned to NPCController.");
            }

            if (waypoints == null || waypoints.Length == 0)
            {
                Debug.LogError("Waypoints are not assigned to NPCController.");
            }

            if (npcAnimationController == null)
            {
                npcAnimationController = GetComponent<NPCAnimationController<NPCState>>();
                if (npcAnimationController == null)
                {
                    Debug.LogError("Animation Controller is not assigned to NPCController.");
                }
            }*/
        }

        private void Start()
        {
            currentState = NPCState.Idle;
            stateTimer = npcSO.WaypointWaitTime; // Initialize idle duration
            npcAnimationController.PlayAnimation(NPCState.Idle);
        }

        private void Update()
        {
            switch (currentState)
            {
                case NPCState.Idle:
                    NpcIdleBehaviour();
                    break;

                case NPCState.Walking:
                    NpcWalkBehaviour();
                    break;

                case NPCState.Interacting:
                    NpcInteractBehaviour();
                    break;
            }
        }

        #region NPC Behaviors

        private void NpcIdleBehaviour()
        {
            stateTimer -= Time.deltaTime;

            if (stateTimer <= 0f)
            {
                TransitionToState(NPCState.Walking);
            }
        }

        private void NpcWalkBehaviour()
        {
            if (waypoints == null || waypoints.Length == 0)
            {
                Debug.LogWarning("No waypoints assigned for walking behavior.");
                TransitionToState(NPCState.Idle);
                return;
            }

            // Move towards the current waypoint
            Vector3 targetPosition = waypoints[currentWaypointIndex].position;
            Vector3 direction = (targetPosition - transform.position).normalized;
            npcCharacterController.Move(direction * npcSO.WalkSpeed * Time.deltaTime);

            // Rotate towards the waypoint
            RotateTowards(targetPosition);
            npcAnimationController.PlayAnimation(NPCState.Walking);

            // Check if we reached the waypoint
            if (Vector3.Distance(transform.position, targetPosition) < 0.1f)
            {
                currentWaypointIndex = (currentWaypointIndex + 1) % waypoints.Length; // Loop waypoints
                TransitionToState(NPCState.Idle);
                stateTimer = npcSO.WaypointWaitTime; // Set wait time at waypoint
            }
        }

        private void NpcInteractBehaviour()
        {
            npcAnimationController.PlayAnimation(NPCState.Interacting);
            // Logic for interaction can go here (e.g., interacting with a player or environment)
            Debug.Log("Interacting with the environment or player.");

            // Return to Idle after interaction
            TransitionToState(NPCState.Idle);
        }

        #endregion

        #region Helper Methods

        private void TransitionToState(NPCState newState)
        {
            currentState = newState;
            Debug.Log($"Transitioned to state: {newState}");
            switch (newState)
            {
                case NPCState.Idle:
                    npcAnimationController.PlayAnimation(NPCState.Idle);
                    break;
                case NPCState.Walking:
                    npcAnimationController.PlayAnimation(NPCState.Walking);
                    break;
                case NPCState.Interacting:
                    npcAnimationController.PlayAnimation(NPCState.Interacting);
                    break;
            }
        }

        private void RotateTowards(Vector3 targetPosition)
        {
            Vector3 direction = (targetPosition - transform.position).normalized;
            Quaternion targetRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, npcSO.RotationSpeed * Time.deltaTime);
        }

        #endregion
    }
}

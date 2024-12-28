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
        [SerializeField] private Animator npcAnimator;

        [Header("Scriptable Object Reference")]
        [SerializeField] private NPCSO npcSO;

        [Header("Dialogue System")]
        [SerializeField] private NPCDialogueSO dialogueSO;
        [SerializeField] private UIController uiController;

        [Header("Interaction Settings")]
        [SerializeField] private float interactionRange = 2f;

        [Header("Waypoints")]
        [SerializeField] private Transform[] waypoints;

        private int currentWaypointIndex = 0;
        private NPCState currentState;
        private NPCState previousState;
        private float stateTimer = 0f;
        private bool isPlayerInRange = false;
        private bool isInteracting = false;

        private void Start()
        {
            currentState = NPCState.Idle;
            stateTimer = npcSO.WaypointWaitTime;
            //npcAnimator.SetTrigger("Idle");
            TransitionToState(NPCState.Idle);
        }

        private void Update()
        {
            CheckPlayerInRange();

            if (isInteracting)
            {
                // If interaction is ongoing, do not perform other behaviors like walking
                return;
            }

            switch (currentState)
            {
                case NPCState.Idle:
                    NpcIdleBehaviour();
                    break;

                case NPCState.Walking:
                    NpcWalkBehaviour();
                    break;

                
            }
        }

        #region NPC Behaviors

        private void NpcIdleBehaviour()
        {
            stateTimer -= Time.deltaTime;

            if (stateTimer <= 0f && !isPlayerInRange) // Transition only if no player interaction
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

            if (Vector3.Distance(transform.position, targetPosition) >= 0.1f) // Still walking
            {
                npcCharacterController.Move(direction * npcSO.WalkSpeed * Time.deltaTime);
                RotateTowards(targetPosition); // Rotate towards the waypoint
                //npcAnimator.SetTrigger("Walking");
                TransitionToState(NPCState.Walking);
            }
            else // Reached the waypoint
            {
                Debug.Log($"Reached at the Destination: { currentWaypointIndex}");
                currentWaypointIndex = (currentWaypointIndex + 1) % waypoints.Length; // Loop waypoints
                TransitionToState(NPCState.Idle); // Transition to Idle
                stateTimer = npcSO.WaypointWaitTime; // Set wait time at waypoint
            }
        }

        private void NpcInteractBehaviour()
        {
            Debug.Log("Interacting with the environment or player.");

            uiController.StartDialogue(dialogueSO);

            TransitionToState(NPCState.Interacting);
            isInteracting = true;
        }

        #endregion

        #region Helper Methods

        private void TransitionToState(NPCState newState)
        {
            if (currentState == newState) return; // Avoid redundant transitions
            currentState = newState;

            Debug.Log($"Transitioned to state: {newState}");

            switch (newState)
            {
                case NPCState.Idle:
                    Debug.Log("Triggering Idle animation");
                    npcAnimator.SetTrigger("Idle");
                    break;

                case NPCState.Walking:
                    Debug.Log("Triggering Walking animation");
                    npcAnimator.SetTrigger("Walking");
                    break;

                case NPCState.Interacting:
                    Debug.Log("Triggering Interacting animation");
                    npcAnimator.SetTrigger("Interacting");
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

        #region Player Interaction

        private void CheckPlayerInRange()
        {
            Collider[] hitColliders = Physics.OverlapSphere(transform.position, interactionRange);
            bool playerDetected = false;

            foreach (var hitCollider in hitColliders)
            {
                if (hitCollider.CompareTag("Player"))
                {
                    playerDetected = true;
                    if (!isPlayerInRange)
                    {
                        isPlayerInRange = true;
                        uiController.ShowTalkButton(true);
                    }
                    break;
                }
            }

            if (!playerDetected && isPlayerInRange)
            {
                isPlayerInRange = false;
                uiController.ShowTalkButton(false);
            }
        }
        public void OnTalkButtonPressed()
        {
            if (isPlayerInRange && currentState != NPCState.Interacting)
            {
                previousState = currentState;
                TransitionToState(NPCState.Interacting);
                NpcInteractBehaviour();
            }
        }

        public void FinishInteraction()
        {
            if (isInteracting)
            {
                isInteracting = false; // End interaction

                // Check if the previous state was Idle
                if (previousState == NPCState.Idle)
                {
                    // Restart walking behavior
                    TransitionToState(NPCState.Walking); // Transition to walking
                    //NpcWalkBehaviour();
                }
                else
                {
                    // Return to the previous state (Walking or whatever it was before interacting)
                    TransitionToState(previousState);
                }
            }
        }
        #endregion

        #region GizmosMethod
        private void OnDrawGizmos()
        {
            // Draw the interaction range
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position, interactionRange);

            // Draw waypoints if any
            if (waypoints != null && waypoints.Length > 0)
            {
                Gizmos.color = Color.green;
                for (int i = 0; i < waypoints.Length; i++)
                {
                    Gizmos.DrawSphere(waypoints[i].position, 0.2f);
                    if (i < waypoints.Length - 1)
                    {
                        Gizmos.DrawLine(waypoints[i].position, waypoints[i + 1].position);
                    }
                }
            }
        }

        #endregion
    }
}

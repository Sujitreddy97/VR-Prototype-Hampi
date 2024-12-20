using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using UnityEngine;
using UnityEngine.UI;
using VRHampi.npc_statepattern;

namespace VRHampi.NPC
{
    public enum NPCState
    {
        Idle,
        Walking,
        Interacting,
        Patrolling
    }
    public abstract class NPCBase : MonoBehaviour
    {
        [Header("Component References")]
        [SerializeField] protected Animator animator;
        [SerializeField] protected CharacterController characterController;

        [Header("ScriptableObject")]
        [SerializeField] protected internal NPCSO npcSO;

        [Header("Patrol Settings")]
        [SerializeField] protected Transform[] _waypoints;
        [SerializeField] protected bool loopPatrol = true;
        [SerializeField] protected float waypointTolerance = 0.5f;
        private int _currentWaypointIndex = 0;

        // Current NPC State
        private INPCState _currentState;

        #region Unity Methods

        protected virtual void Awake()
        {
            InitializeComponents();
        }

        protected virtual void Start()
        {
            InitializeNPC();
        }

        protected virtual void Update()
        {
            HandleBehavior();
        }

        protected virtual void OnDestroy()
        {
            CleanupResources();
        }

        #endregion

        #region Initialization

        // Initialize components
        protected virtual void InitializeComponents()
        {
            if (animator == null)
                animator = GetComponent<Animator>();

            if (characterController == null)
                characterController = GetComponent<CharacterController>();

            if (npcSO == null)
                Debug.LogWarning($"{name} is missing an NPC ScriptableObject!");
        }

        // To be implemented by concrete NPCs
        protected abstract void InitializeNPC();

        #endregion

        #region NPC Behavior

        // To be implemented by subclasses
        protected abstract void HandleBehavior();
        public abstract void Interact();

        // Change NPC state
        protected internal virtual void ChangeState(INPCState newState)
        {
            _currentState = newState;
            Debug.Log($"{name} changed to state: {_currentState}");
        }

        // Logic for triggering animations
        protected internal virtual void TriggerAnimation(string animationTrigger)
        {
            if (animator != null && !string.IsNullOrEmpty(animationTrigger))
            {
                ResetAllAnimationTriggers(); // Reset all other triggers to avoid conflicts
                animator.SetTrigger(animationTrigger);
            }
        }

        protected virtual void ResetAllAnimationTriggers()
        {
            animator.ResetTrigger(npcSO._idleAnimationTrigger);
            animator.ResetTrigger(npcSO._walkAnimationTrigger);
            animator.ResetTrigger(npcSO._interactAnimationTrigger);
        }

        // Example method for movement (can be overridden by concrete NPCs)
        protected virtual void Move(Vector3 direction, float speed)
        {
            if (characterController != null)
            {
                Vector3 movement = direction.normalized * speed * Time.deltaTime;
                characterController.Move(movement);
            }
        }

        public virtual bool IsInteractionComplete()
        {
            // Default logic can check if an animation is playing
            if (animator != null)
            {
                AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);
                return !stateInfo.IsName(npcSO._interactAnimationTrigger) || stateInfo.normalizedTime >= 1f;
            }
            return true; // Default to true if no animator exists
        }

        public virtual bool WaypointsAvailable()
        {
            return _waypoints != null && _waypoints.Length > 0;
        }

        protected internal virtual void Patrol()
        {
            if (_waypoints == null || _waypoints.Length == 0) return;

            Transform currentWaypoint = _waypoints[_currentWaypointIndex];
            Vector3 direction = currentWaypoint.position - transform.position;

            if (direction.magnitude <= waypointTolerance)
            {
                AdvanceWaypoint();
            }
            else
            {
                Move(direction, npcSO._moveSpeed);
                RotateTowards(currentWaypoint.position);
                TriggerAnimation(npcSO._walkAnimationTrigger);
            }
        }

        protected internal void AdvanceWaypoint()
        {
            if (_waypoints == null || _waypoints.Length == 0) return;

            _currentWaypointIndex = (_currentWaypointIndex + 1) % _waypoints.Length;
        }

        protected internal void RotateTowards(Vector3 targetPosition)
        {
            Vector3 direction = (targetPosition - transform.position).normalized;
            Quaternion targetRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, npcSO._rotationSpeed * Time.deltaTime);
        }

        #endregion

        #region Cleanup

        protected virtual void CleanupResources()
        {
            // Cleanup logic, such as unsubscribing from events
        }

        #endregion
    }
}


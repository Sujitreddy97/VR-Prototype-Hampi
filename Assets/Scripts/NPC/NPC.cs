using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VRHampi.NPC
{
    public abstract class NPC : MonoBehaviour
    {
        [Header("Component References")]
        [SerializeField] internal Animator animator;
        [SerializeField] internal CharacterController characterController;

        [Header("ScriptableObject")]
        [SerializeField] internal NPCSO npcSO;

        [Header("Patrol Settings")]
        [SerializeField] internal Transform[] _waypoints;
        [SerializeField] internal bool loopPatrol = true;
        [SerializeField] internal float waypointTolerance = 0.5f;

        private int _currentWaypointIndex = 0;
        private bool _patrollingForward = true;

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

        protected virtual void InitializeComponents()
        {
            if (animator == null)
                animator = GetComponent<Animator>();

            if (characterController == null)
                characterController = GetComponent<CharacterController>();

            if (npcSO == null)
                Debug.LogWarning($"{name} is missing an NPC ScriptableObject!");

            if (_waypoints == null || _waypoints.Length == 0)
                Debug.LogWarning($"{name} has no patrol waypoints assigned!");
        }

        protected abstract void InitializeNPC();
        #endregion

        #region NPC Behavior

        protected abstract void HandleBehavior();

        protected virtual void Move(Vector3 direction, float speed)
        {
            if (characterController != null)
            {
                Vector3 movement = direction.normalized * speed * Time.deltaTime;
                characterController.Move(movement);
            }
        }

        protected virtual void Patrol()
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

        protected virtual void RotateTowards(Vector3 targetPosition)
        {
            Vector3 direction = (targetPosition - transform.position).normalized;
            Quaternion targetRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, npcSO._rotationSpeed * Time.deltaTime);
        }

        private void AdvanceWaypoint()
        {
            if (loopPatrol)
            {
                _currentWaypointIndex = (_currentWaypointIndex + 1) % _waypoints.Length;
            }
            else
            {
                if (_patrollingForward)
                {
                    _currentWaypointIndex++;
                    if (_currentWaypointIndex >= _waypoints.Length)
                    {
                        _currentWaypointIndex = _waypoints.Length - 1;
                        _patrollingForward = false;
                    }
                }
                else
                {
                    _currentWaypointIndex--;
                    if (_currentWaypointIndex < 0)
                    {
                        _currentWaypointIndex = 0;
                        _patrollingForward = true;
                    }
                }
            }
        }

        protected virtual void TriggerAnimation(string animationName)
        {
            if (animator != null && !string.IsNullOrEmpty(animationName))
            {
                animator.SetTrigger(animationName);
            }
        }

        public abstract void Interact();
        #endregion

        #region Cleanup

        protected virtual void CleanupResources()
        {
            // Implement cleanup logic, e.g., unsubscribe from events
        }
        #endregion
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

namespace VRHampi.NPC
{
    [CreateAssetMenu(fileName = "NPCSO", menuName = "NPC", order = 1)]
    public class NPCSO : ScriptableObject
    {
        [Header("NPC Name")]
        [SerializeField] internal string _npcName;

        [Header("Movement Settings")]
        [Tooltip("Movement speed of the NPC.")]
        [SerializeField] internal float _moveSpeed = 3.5f;

        [Tooltip("Rotation speed of the NPC.")]
        [SerializeField] internal float _rotationSpeed = 180f;

        [Header("Behavior Settings")]
        [Tooltip("The maximum detection range for NPC interactions.")]
        [SerializeField] internal float _detectionRange = 5f;

        [Tooltip("How far the NPC can patrol from its starting point.")]
        [SerializeField] internal float _patrolRadius = 10f;

        [Header("Animation Settings")]
        [Tooltip("The name of the idle animation trigger.")]
        [SerializeField] internal string _idleAnimationTrigger = "Idle";

        [Tooltip("The name of the walk animation trigger.")]
        [SerializeField] internal string _walkAnimationTrigger = "Walk";

        [Tooltip("The name of the interact animation trigger.")]
        [SerializeField] internal string _interactAnimationTrigger = "Interact";

        [Header("NPC Characteristics")]
        [Tooltip("The type or role of the NPC.")]
        [SerializeField] internal string _npcType = "Default";

        [Header("NPC Description")]
        [SerializeField] [Tooltip("Any dialogue or description associated with this NPC.")]
        internal string _description;
    }
}

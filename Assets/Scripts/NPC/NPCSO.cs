using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VRHampi.NPC
{
    public enum NPCState
    {
        Idle,
        Walking,
        Interacting
    }
    [CreateAssetMenu(fileName = "NPCSO", menuName = "NPC/NPCSO", order = 0)]
    public class NPCSO : ScriptableObject
    {
        [Header("NPC Settings")]
        [Tooltip("Name of the NPC.")]
        [SerializeField] internal string npcName;

        [Tooltip("Walking speed of the NPC.")]
        [SerializeField] internal float WalkSpeed = 3f;

        [Tooltip("Waiting time at waypoints in seconds.")]
        [SerializeField] internal float WaypointWaitTime = 5f;

        [Tooltip("Rotation speed of the NPC in degrees per second.")]
        [SerializeField] internal float RotationSpeed = 180f;
    }
}

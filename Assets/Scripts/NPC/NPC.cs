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

    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VRHampi.NPC
{
    [CreateAssetMenu(fileName = "NPCSO", menuName = "NPC", order = 1)]
    public class NPCSO : ScriptableObject
    {
        [Header("Sped")]
        [SerializeField] internal float _moveSpeed;
        
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRHampi.NPC;

namespace VRHampi.npc_statepattern
{
    public class IdleState : INPCState
    {
        public void EnterState(NPCBase npc)
        {
            npc.TriggerAnimation(npc.npcSO._idleAnimationTrigger);
            Debug.Log($"{npc.name} is now Idle.");
        }

        public void UpdateState(NPCBase npc)
        {
            // Idle behavior logic can be added here (e.g., look around, wait).
        }

        public void ExitState(NPCBase npc)
        {
            Debug.Log($"{npc.name} exited Idle state.");
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRHampi.NPC;

namespace VRHampi.npc_statepattern
{
    public class InteractingState : INPCState
    {
        public void EnterState(NPCBase npc)
        {
            npc.TriggerAnimation(npc.npcSO._interactAnimationTrigger);
            Debug.Log($"{npc.name} entered Interacting state.");
        }

        public void UpdateState(NPCBase npc)
        {
            // Add logic here for ongoing interaction behavior
            // For example, wait until animation ends or player input is received
            if (npc.IsInteractionComplete())
            {
                npc.ChangeState(new IdleState()); // Transition back to Idle state after interaction
            }
        }

        public void ExitState(NPCBase npc)
        {
            Debug.Log($"{npc.name} exited Interacting state.");
        }
    }
}

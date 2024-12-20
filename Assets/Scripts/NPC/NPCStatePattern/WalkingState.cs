using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRHampi.NPC;

namespace VRHampi.npc_statepattern
{
    public class WalkingState : INPCState
    {
        public void EnterState(NPCBase npc)
        {
            npc.TriggerAnimation(npc.npcSO._walkAnimationTrigger);
            Debug.Log($"{npc.name} entered Walking state.");
        }

        public void UpdateState(NPCBase npc)
        {
            // Logic for patrolling or moving
            if (npc.WaypointsAvailable())
            {
                npc.Patrol();
            }
            else
            {
                npc.ChangeState(new IdleState()); // Change to Idle if no waypoints
            }
        }

        public void ExitState(NPCBase npc)
        {
            Debug.Log($"{npc.name} exited Walking state.");
        }
    }
}

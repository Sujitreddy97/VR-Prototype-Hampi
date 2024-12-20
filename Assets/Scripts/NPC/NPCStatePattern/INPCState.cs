
using  VRHampi.NPC;

namespace VRHampi.npc_statepattern
{
    public interface INPCState 
    {
        public void EnterState(NPCBase npc);
        public void UpdateState(NPCBase npc);
        public void ExitState(NPCBase npc);
    }
}
 
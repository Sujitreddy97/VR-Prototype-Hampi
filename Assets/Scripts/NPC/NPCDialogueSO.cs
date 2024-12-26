using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VRHampi.NPC
{
    [CreateAssetMenu(fileName = "NewNPCDialogue", menuName = "Dialogue/NPC Dialogue", order = 1)]
    public class NPCDialogueSO : ScriptableObject
    {
        [Header("NPC Details")]
        public string npcName;

        [Header("Dialogue Lines")]
        public List<string> dialogueLines;
    }
}

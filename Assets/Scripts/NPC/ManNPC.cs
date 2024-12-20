using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRHampi.npc_statepattern;

namespace VRHampi.NPC
{
    public class ManNPC : NPCBase
    {
        private INPCState _currentState;

        protected override void InitializeNPC()
        {
            // Start with IdleState
            ChangeState(new IdleState());
        }

        protected override void HandleBehavior()
        {
            // Delegate behavior handling to the current state
            _currentState?.UpdateState(this);
        }

        public override void Interact()
        {
            // Trigger interaction by transitioning to InteractingState
            ChangeState(new InteractingState());
        }

        public void ChangeState(INPCState newState)
        {
            if (_currentState != null)
            {
                _currentState.ExitState(this); // Exit current state
            }

            _currentState = newState;
            _currentState.EnterState(this); // Enter new state
        }

        public override bool IsInteractionComplete()
        {
            // Check if interaction animation has finished
            if (animator != null)
            {
                AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);
                return !stateInfo.IsName(npcSO._interactAnimationTrigger) || stateInfo.normalizedTime >= 1f;
            }
            return true;
        }

        #region Unity Methods

        protected override void Update()
        {
            base.Update();

            // Example: Transition to InteractingState if player is in range
            /*if (PlayerInRange() && _currentState is not InteractingState)
            {
                Interact();
            }*/
        }

        #endregion
    }
}



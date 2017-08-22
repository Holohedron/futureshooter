using System;
using UnityEngine;

namespace Player
{
    public class JumpState : PlayerBaseState, IPlayerState
    {
        private CharacterController playerCharController;

        public IPlayerState HandleTransition(PlayerCharacter player)
        {
            IPlayerState baseTransition = base.HandleTransition(player);
            if (baseTransition != null)
                return baseTransition;
            if (playerCharController.isGrounded)
                return new MeleeState();
            return null;
        }

        public PlayerActions OnEnter(PlayerCharacter player)
        {
            var baseActions = base.OnEnter(player);
            playerCharController = player.GetComponent<CharacterController>();
            player.Actions.Jump(player);

            return baseActions;
        }

        public void OnExit(PlayerCharacter player)
        {
            base.OnExit(player);
        }

        public void HandleUpdate(PlayerCharacter player)
        {
            base.HandleUpdate(player);

            player.Actions.Move(player);
            player.Actions.Fall(player);
        }

        public void HandleHit(PlayerCharacter player)
        {
            base.HandleHit(player);
        }
    }
}
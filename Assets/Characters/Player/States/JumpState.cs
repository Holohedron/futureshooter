using System;
using UnityEngine;

namespace Player
{
    public class JumpState : PlayerBaseState, IPlayerState
    {
        public new IPlayerState HandleTransition(PlayerCharacter player)
        {
            IPlayerState baseTransition = base.HandleTransition(player);
            if (baseTransition != null)
                return baseTransition;
            if (player.grounded)
                return new MeleeState();
            return null;
        }

        public new PlayerActions OnEnter(PlayerCharacter player)
        {
            var baseActions = base.OnEnter(player);
            player.Actions.Jump(player);

            return baseActions;
        }

        public new void OnExit(PlayerCharacter player)
        {
            base.OnExit(player);
        }

        public new void HandleUpdate(PlayerCharacter player)
        {
            base.HandleUpdate(player);

            player.Actions.Move(player);
        }

        public new void HandleHit(PlayerCharacter player)
        {
            base.HandleHit(player);
        }
    }
}
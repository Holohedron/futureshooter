using UnityEngine;

namespace Player
{
    public class MeleeState : PlayerBaseState, IPlayerState
    {
        private bool hit;

        public override IPlayerState HandleTransition(PlayerCharacter player)
        {
            IPlayerState baseTransition = base.HandleTransition(player);
            if (baseTransition != null)
                return baseTransition;

            if (Input.GetButtonDown("Aim") || Input.GetAxis("AimAxis") > 0)
                return new ShooterState();
            if (Input.GetButtonDown("Jump"))
                return new JumpState();
            return null;
        }

        public override PlayerActions OnEnter(PlayerCharacter player)
        {
            var baseActions = base.OnEnter(player);

            return baseActions;
        }

        public override void OnExit(PlayerCharacter player)
        {
            base.OnExit(player);
        }

        public override void HandleUpdate(PlayerCharacter player)
        {
            base.HandleUpdate(player);

            player.Actions.Move(player);
            player.Actions.Melee(player);
        }

        public override void HandleHit(PlayerCharacter player)
        {
            base.HandleHit(player);
        }
    }
}
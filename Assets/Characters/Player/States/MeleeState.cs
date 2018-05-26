using UnityEngine;

namespace Player
{
    public class MeleeState : PlayerBaseState, IPlayerState
    {
        private bool hit;

        public new IPlayerState HandleTransition(PlayerCharacter player)
        {
            IPlayerState baseTransition = base.HandleTransition(player);
            if (baseTransition != null)
                return baseTransition;

            if (Input.GetButtonDown("Aim") || Input.GetAxis("AimAxis") > 0)
                return new ShooterState();
            if (Input.GetButtonDown("Jump"))
                return new JumpState();
            if (Input.GetButton("Attack"))
                return new AttackState();
            if (Input.GetButtonDown("Dash"))
                return new DashState();
            return null;
        }

        public new PlayerActions OnEnter(PlayerCharacter player)
        {
            var baseActions = base.OnEnter(player);

            return baseActions;
        }

        public new void OnExit(PlayerCharacter player)
        {
            base.OnExit(player);
        }

        public new void HandleUpdate(PlayerCharacter player)
        {
            base.HandleUpdate(player);

            player.Actions.MeleeMove(player);
        }

        public new void HandleHit(PlayerCharacter player)
        {
            base.HandleHit(player);
        }
    }
}
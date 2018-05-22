using UnityEngine;

namespace Player
{
    public class ShooterState : PlayerBaseState, IPlayerState
    {        
        public new IPlayerState HandleTransition(PlayerCharacter player)
        {
            IPlayerState baseTransition = base.HandleTransition(player);
            if (baseTransition != null)
                return baseTransition;
            if (!Input.GetButton("Aim") && Input.GetAxis("AimAxis") == 0)
                return new MeleeState();
            return null;
        }

        public new PlayerActions OnEnter(PlayerCharacter player)
        {
            var baseActions = base.OnEnter(player);

            // zoom camera in
            player.aiming = true;
            return baseActions;
        }

        public new void OnExit(PlayerCharacter player)
        {
            base.OnExit(player);

            // zoom camera out
            player.transform.GetChild(0).rotation = new Quaternion(0, 0, 0, 0);
            player.aiming = false;
        }

        public new void HandleUpdate(PlayerCharacter player)
        {
            base.HandleUpdate(player);

            player.Actions.SetReticlePos(player);
            player.Actions.Aim(player);
            player.Actions.Rotate(player);
            player.Actions.Fire(player);
            player.Actions.Move(player, true);
        }

        public new void HandleHit(PlayerCharacter player)
        {
            base.HandleHit(player);
        }
    }

}
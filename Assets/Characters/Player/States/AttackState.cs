using UnityEngine;

namespace Player
{
    public class AttackState : PlayerBaseState, IPlayerState
    {
        private const int FLASHINTERVAL = 8;

        public new IPlayerState HandleTransition(PlayerCharacter player)
        {
            if (!player.attacking)
            {
                return new MeleeState();
            }
            return null;
        }

        public new PlayerActions OnEnter(PlayerCharacter player)
        {
            player.attacking = true;
            var anim = player.GetComponentInChildren<Animator>();
            anim.SetTrigger("SwingSword");

            return PlayerActions.GetInstance();
        }

        public new void OnExit(PlayerCharacter player)
        {
            base.OnExit(player);
        }

        public new void HandleUpdate(PlayerCharacter player)
        {
            base.HandleUpdate(player);
        }

        public new void HandleHit(PlayerCharacter player)
        {
            base.HandleHit(player);
        }
    }
}
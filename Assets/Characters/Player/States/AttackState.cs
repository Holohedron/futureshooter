using UnityEngine;

namespace Player
{
    public class AttackState : PlayerBaseState, IPlayerState
    {
        private const int FLASHINTERVAL = 8;
        private Animator anim;

        public new IPlayerState HandleTransition(PlayerCharacter player)
        {
            IPlayerState baseTransition = base.HandleTransition(player);
            if (baseTransition != null)
                return baseTransition;

            if (!player.attacking)
            {
                return new MeleeState();
            }
            return null;
        }

        public new PlayerActions OnEnter(PlayerCharacter player)
        {
            player.attacking = true;
            anim = player.sword.GetComponent<Animator>();
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
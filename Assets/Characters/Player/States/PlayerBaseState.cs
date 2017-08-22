using UnityEngine;
using System.Collections;
using System;

namespace Player
{
    public class PlayerBaseState : IPlayerState
    {
        protected bool hit;

        public virtual IPlayerState HandleTransition(PlayerCharacter player)
        {
            if (hit)
                return new HitState();
            return null;
        }

        public virtual PlayerActions OnEnter(PlayerCharacter player)
        {
            hit = false;

            return PlayerActions.GetInstance();
        }

        public virtual void OnExit(PlayerCharacter player)
        {
            // nothing
        }

        public virtual void HandleUpdate(PlayerCharacter player)
        {
            player.Actions.Fall(player);
        }

        public virtual void HandleHit(PlayerCharacter player)
        {
            hit = true;
            player.Actions.GetHit(player);
        }
    }

}
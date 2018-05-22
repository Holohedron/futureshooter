using UnityEngine;
using System.Collections;
using System;

namespace Player
{
    public class PlayerBaseState : IPlayerState
    {
        protected bool hit;

        public IPlayerState HandleTransition(PlayerCharacter player)
        {
            if (hit)
                return new HitState();
            return null;
        }

        public PlayerActions OnEnter(PlayerCharacter player)
        {
            hit = false;

            return PlayerActions.GetInstance();
        }

        public void OnExit(PlayerCharacter player)
        {
            // nothing
        }

        public void HandleUpdate(PlayerCharacter player)
        {
            player.Actions.Fall(player);
        }

        public void HandleHit(PlayerCharacter player)
        {
            hit = true;
            player.Actions.GetHit(player);
        }
    }

}
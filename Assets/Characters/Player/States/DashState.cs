using System;
using UnityEngine;

namespace Player
{
    public class DashState : PlayerBaseState, IPlayerState
    {
        private int timer;
        private Vector3 dashDirection;
        private float dashSpeed;

        public new IPlayerState HandleTransition(PlayerCharacter player)
        {
            IPlayerState baseTransition = base.HandleTransition(player);
            if (baseTransition != null)
                return baseTransition;
            
            if (timer <= 0)
            {
                if (Input.GetButton("Aim") && Input.GetAxis("AimAxis") == 0)
                    return new ShooterState();
                else
                    return new MeleeState();
            }
            return null;
        }

        public new PlayerActions OnEnter(PlayerCharacter player)
        {
            var baseActions = base.OnEnter(player);
            timer = player.dashTime;
            dashDirection = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
            dashDirection = player.transform.TransformDirection(dashDirection).normalized;
            dashSpeed = player.dashDistance / player.dashTime;

            return baseActions;
        }

        public new void OnExit(PlayerCharacter player)
        {
            base.OnExit(player);
        }

        public new void HandleUpdate(PlayerCharacter player)
        {
            base.HandleUpdate(player);
            timer--;
            player.GetComponent<CharacterController>().Move(dashDirection * dashSpeed);
        }

        public new void HandleHit(PlayerCharacter player)
        {
            // nothing, invincible while dashing
        }
    }
}
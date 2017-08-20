using UnityEngine;

namespace Player
{
    public class MeleeState : ScriptableObject, PlayerState
    {
        private const float MOVEMENTDEADZONE = 0.01f;
        private bool hit = false;

        public PlayerState HandleTransition(PlayerCharacter player)
        {
            if (hit)
                return ScriptableObject.CreateInstance<HitState>();
            if (Input.GetButtonDown("Aim") || Input.GetAxis("AimAxis") > 0)
                return ScriptableObject.CreateInstance<ShooterState>();
            return null;
        }

        public PlayerActions OnEnter(PlayerCharacter player)
        {
            return PlayerActions.GetInstance();
        }

        public void OnExit(PlayerCharacter player)
        {
            // do nothing
        }

        public void HandleUpdate(PlayerCharacter player)
        {
            player.Actions.doMovement(player);
            player.Actions.doMelee(player);
        }

        public void HandleHit(PlayerCharacter player)
        {
            --player.health;
            hit = true;
            player.GetComponent<AudioSource>().PlayOneShot(player.hurtSFX);
            if (player.health <= 0)
                player.Die();
        }
    }
}
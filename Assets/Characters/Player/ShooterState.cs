using UnityEngine;

namespace Player
{
    public class ShooterState : ScriptableObject, PlayerState
    {        
        private bool hit = false;

        public PlayerState HandleTransition(PlayerCharacter player)
        {
            if (hit)
                return ScriptableObject.CreateInstance<HitState>();
            if (!Input.GetButton("Aim") && Input.GetAxis("AimAxis") == 0)
                return ScriptableObject.CreateInstance<MeleeState>();
            return null;
        }

        public PlayerActions OnEnter(PlayerCharacter player)
        {
            // zoom camera in
            player.aiming = true;
            return PlayerActions.GetInstance();
        }

        public void OnExit(PlayerCharacter player)
        {
            // zoom camera out
            player.transform.GetChild(0).rotation = new Quaternion(0, 0, 0, 0);
            player.aiming = false;
        }

        public void HandleUpdate(PlayerCharacter player)
        {
            player.Actions.setReticlePos(player);
            player.Actions.doAim(player);
            player.Actions.doRotate(player);
            player.Actions.doFire(player);
            player.Actions.doMovementWithStrafe(player);
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
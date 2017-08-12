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

        public void OnEnter(PlayerCharacter player)
        {
            // do nothing
        }

        public void OnExit(PlayerCharacter player)
        {
            // do nothing
        }

        public void HandleUpdate(PlayerCharacter player)
        {
            doMovement(player);
            doMelee(player);
        }

        public void HandleHit(PlayerCharacter player)
        {
            --player.health;
            hit = true;
            player.GetComponent<AudioSource>().PlayOneShot(player.hurtSFX);
            if (player.health <= 0)
                player.Die();
        }

        private void doMovement(PlayerCharacter player)
        {
            float speed = player.moveSpeed * Time.deltaTime;

            if (Input.GetAxis("Vertical") > MOVEMENTDEADZONE)
            {
                Vector3 moveVec = new Vector3(0, 0, speed);
                moveVec = player.transform.TransformDirection(moveVec);
                player.GetComponent<CharacterController>().Move(moveVec);
            }
            else if (Input.GetAxis("Vertical") < -MOVEMENTDEADZONE)
            {
                Vector3 moveVec = new Vector3(0, 0, -speed);
                moveVec = player.transform.TransformDirection(moveVec);
                player.GetComponent<CharacterController>().Move(moveVec);
            }

            if (Input.GetAxis("RightHorizontal") < -MOVEMENTDEADZONE || Input.GetAxis("Horizontal") < -MOVEMENTDEADZONE)
            {
                player.transform.Rotate(new Vector3(0, -player.turnSpeed, 0));
            }
            else if (Input.GetAxis("RightHorizontal") > MOVEMENTDEADZONE || Input.GetAxis("Horizontal") > MOVEMENTDEADZONE)
            {
                player.transform.Rotate(new Vector3(0, player.turnSpeed, 0));
            }
        }

        private void doMelee(PlayerCharacter player)
        {
            if (Input.GetButtonDown("Attack"))
            {
                var anim = player.GetComponentInChildren<Animator>();
                anim.SetTrigger("SwingSword");
            }
        }
    }

}
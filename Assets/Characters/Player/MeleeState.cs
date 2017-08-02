using UnityEngine;

namespace Player
{
    public class MeleeState : ScriptableObject, PlayerState
    {
        private bool hit = false;

        public PlayerState HandleTransition(PlayerCharacter player)
        {
            if (hit)
                return ScriptableObject.CreateInstance<HitState>();
            if (Input.GetKey("left shift"))
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
        }

        public void HandleHit(PlayerCharacter player)
        {
            --player.health;
            hit = true;
            if (player.health <= 0)
                player.Die();
        }

        private void doMovement(PlayerCharacter player)
        {
            float speed = player.moveSpeed * Time.deltaTime;

            if (Input.GetKey("up") || Input.GetKey("w"))
            {
                Vector3 moveVec = new Vector3(0, 0, speed);
                moveVec = player.transform.TransformDirection(moveVec);
                player.GetComponent<CharacterController>().Move(moveVec);
            }
            else if (Input.GetKey("down") || Input.GetKey("s"))
            {
                Vector3 moveVec = new Vector3(0, 0, -speed);
                moveVec = player.transform.TransformDirection(moveVec);
                player.GetComponent<CharacterController>().Move(moveVec);
            }

            if (Input.GetKey("left") || Input.GetKey("a"))
            {
                player.transform.Rotate(new Vector3(0, -player.turnSpeed, 0));

            }
            else if (Input.GetKey("right") || Input.GetKey("d"))
            {
                player.transform.Rotate(new Vector3(0, player.turnSpeed, 0));
            }
        }
    }

}
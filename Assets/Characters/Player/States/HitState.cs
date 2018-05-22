using UnityEngine;

namespace Player
{
    public class HitState : IPlayerState
    {
        private const int FLASHINTERVAL = 8;
        private int timer;
        Material normalMat;
        Material hurtMat;

        public IPlayerState HandleTransition(PlayerCharacter player)
        {
            if (timer <= 0)
            {
                if (Input.GetButton("Aim") && Input.GetAxis("AimAxis") == 0)
                    return new ShooterState();
                else
                    return new MeleeState();
            }
            return null;
        }

        public PlayerActions OnEnter(PlayerCharacter player)
        {
            timer = player.hitTime;
            normalMat = Resources.Load("PlayerMat", typeof(Material)) as Material;
            hurtMat = Resources.Load("PlayerHurtMat", typeof(Material)) as Material;

            return PlayerActions.GetInstance();
        }

        public void OnExit(PlayerCharacter player)
        {
            player.GetComponent<Renderer>().material = normalMat;
        }

        public void HandleUpdate(PlayerCharacter player)
        {
            Flash(player);
            player.GetComponent<CharacterController>().Move(player.transform.TransformDirection(Vector3.back) * Time.deltaTime * player.bounceback);
            --timer;
        }

        public void HandleHit(PlayerCharacter player)
        {
            // nothing
        }

        private void Flash(PlayerCharacter player)
        {
            var flashRate = FLASHINTERVAL;
            if (timer % flashRate < flashRate/2)
            {
                player.GetComponent<Renderer>().material = normalMat;
            }
            else
            {
                player.GetComponent<Renderer>().material = hurtMat;
            }
        }
    }
}
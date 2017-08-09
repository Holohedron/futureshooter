using UnityEngine;

namespace Player
{
    public class ShooterState : ScriptableObject, PlayerState
    {
        private const float FIRETIMERMAX = 60.0f;
        private const int BOUNDUPPERX = 25; // percent of the width of the screen
        private const int BOUNDLOWERX = 25; // percent of the width of the screen
        private const int FASTBOUNDUPPERX = 15;
        private const int FASTBOUNDLOWERX = 15;
        private float fireTimer;
        private bool hit = false;

        public PlayerState HandleTransition(PlayerCharacter player)
        {
            if (hit)
                return ScriptableObject.CreateInstance<HitState>();
            if (Input.GetKeyUp("left shift"))
                return ScriptableObject.CreateInstance<MeleeState>();
            return null;
        }

        public void OnEnter(PlayerCharacter player)
        {
            // zoom camera in
            fireTimer = 0;
            player.aiming = true;
        }

        public void OnExit(PlayerCharacter player)
        {
            // zoom camera out
            player.transform.GetChild(0).rotation = new Quaternion(0, 0, 0, 0);
            player.aiming = false;
        }

        public void HandleUpdate(PlayerCharacter player)
        {
            doMovement(player);
            doRotate(player);
            doFire(player);
            doAim(player);
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

            if (Input.GetKey("up") || Input.GetKey("w"))
            {
                player.transform.Translate(new Vector3(0, 0, speed));
            }
            else if (Input.GetKey("down") || Input.GetKey("s"))
            {
                player.transform.Translate(new Vector3(0, 0, -speed));
            }

            if (Input.GetKey("left") || Input.GetKey("a"))
            {
                player.transform.Translate(new Vector3(-speed, 0, 0));
            }
            else if (Input.GetKey("right") || Input.GetKey("d"))
            {
                player.transform.Translate(new Vector3(speed, 0, 0));
            }
        }

        private void doRotate(PlayerCharacter player)
        {
            // rotate at set speed when mouse is outside of bounding box
            int fastUpperBoundX = Screen.width - (Screen.width * FASTBOUNDLOWERX / 100);
            int fastLowerBoundX = Screen.width * FASTBOUNDLOWERX / 100;
            int upperBoundX = Screen.width - (Screen.width * BOUNDLOWERX / 100);
            int lowerBoundX = Screen.width * BOUNDUPPERX / 100;

            var mousePos = Input.mousePosition;
            if (mousePos.x > fastUpperBoundX)
                player.transform.Rotate(new Vector3(0, player.aimingTurnSpeed * Time.deltaTime * 2, 0));
            else if (mousePos.x > upperBoundX)
                player.transform.Rotate(new Vector3(0, player.aimingTurnSpeed * Time.deltaTime, 0));
            else if (mousePos.x < fastLowerBoundX)
                player.transform.Rotate(new Vector3(0, -player.aimingTurnSpeed * Time.deltaTime * 2, 0));
            else if (mousePos.x < lowerBoundX)
                player.transform.Rotate(new Vector3(0, -player.aimingTurnSpeed * Time.deltaTime, 0));
        }

        private void doFire(PlayerCharacter player)
        {
            if (fireTimer > 0)
            {
                fireTimer -= player.fireRate;
                return;
            }

            if ((Input.GetKey("space") || Input.GetMouseButton(0)))
            {
                Transform gun = player.transform.GetChild(0).GetChild(0);
                var projectilePos = gun.position + gun.up * gun.localScale.y * 2;
                var projectileRot = gun.rotation * Quaternion.Euler(-90, 0, 0);
                Projectile p = Instantiate<Projectile>(player.projectileFab, projectilePos, projectileRot);

                fireTimer = FIRETIMERMAX;

                player.gameObject.GetComponent<AudioSource>().PlayOneShot(player.projectileSFX);
            }
        }

        private void doAim(PlayerCharacter player)
        {
            Transform gun = player.transform.GetChild(0);

            RaycastHit hitInfo;
            Vector3 targetPos;
            var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hitInfo, 1000))
            {
                targetPos = hitInfo.point;
                gun.LookAt(targetPos);
            }
            else
            {
                // Roughly aim in the right direction if ray hits nothing
                targetPos = Input.mousePosition;
                targetPos.x -= Screen.width / 2;
                targetPos.y -= Screen.height / 2;
                var mousePosX = targetPos.x;

                // Look at a spot around an imaginary sphere around the character
                var targetRadius = player.radius;
                targetPos.z = getZOnSphere(targetPos.x, targetPos.y, targetRadius);
                targetPos = player.transform.TransformPoint(targetPos);
                gun.LookAt(targetPos);

                // Account for the camera's rotated view
                var camera = GameObject.FindGameObjectWithTag("MainCamera").transform;
                gun.Rotate(new Vector3(camera.rotation.eulerAngles.x * (1 - (Mathf.Abs(mousePosX) / (Screen.width))), 0, 0));
            }
        }

        private float getZOnSphere(float x, float y, float r)
        {
            return Mathf.Sqrt((r * r) - (x * x) - (y * y)) / 2;
        }
    }

}
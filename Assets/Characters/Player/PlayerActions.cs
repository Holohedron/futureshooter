using UnityEngine;

namespace Player
{
    public class PlayerActions
    {
        private const float MOVEMENTDEADZONE = 0.01f;
        private const float FIRETIMERMAX = 60.0f;
        private const int AIMSENSITIVITY = 30;
        private const int BOUNDX = 40; // percent of the width of the screen
        private const int FASTBOUNDX = 20;
        private const int RETICLEBOUNDX = 35;
        private const int RETICLEBOUNDY = 20;

        private Vector3 movementVec = Vector3.zero;
        private float fireTimer;
        private bool usingMouse = false;
        
        private static PlayerActions instance;

        private PlayerActions()
        {
        }

        public static PlayerActions GetInstance()
        {
            if (instance == null)
                instance = new PlayerActions();
            return instance;
        }

        public void Move(PlayerCharacter player, bool strafe = false)
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

            if (strafe)
            {
                if (Input.GetAxis("Horizontal") < -MOVEMENTDEADZONE)
                {
                    Vector3 moveVec = new Vector3(-speed, 0, 0);
                    moveVec = player.transform.TransformDirection(moveVec);
                    player.GetComponent<CharacterController>().Move(moveVec);
                }
                else if (Input.GetAxis("Horizontal") > MOVEMENTDEADZONE)
                {
                    Vector3 moveVec = new Vector3(speed, 0, 0);
                    moveVec = player.transform.TransformDirection(moveVec);
                    player.GetComponent<CharacterController>().Move(moveVec);
                }
            }
            else
            {
                if (Input.GetAxis("RightHorizontal") < -MOVEMENTDEADZONE || Input.GetAxis("Horizontal") < -MOVEMENTDEADZONE)
                {
                    player.transform.Rotate(new Vector3(0, -player.turnSpeed, 0));
                }
                else if (Input.GetAxis("RightHorizontal") > MOVEMENTDEADZONE || Input.GetAxis("Horizontal") > MOVEMENTDEADZONE)
                {
                    player.transform.Rotate(new Vector3(0, player.turnSpeed, 0));
                }
            }
        }

        public void Melee(PlayerCharacter player)
        {
            if (Input.GetButtonDown("Attack"))
            {
                var anim = player.GetComponentInChildren<Animator>();
                anim.SetTrigger("SwingSword");
            }
        }

        public void Fire(PlayerCharacter player)
        {
            if (fireTimer > 0)
            {
                fireTimer -= player.fireRate;
                return;
            }

            if (Input.GetButtonDown("Fire") || Input.GetAxis("FireAxis") > 0)
            {
                Transform gun = player.transform.GetChild(0).GetChild(0);
                var projectilePos = gun.position + gun.up * gun.localScale.y * 2;
                var projectileRot = gun.rotation * Quaternion.Euler(-90, 0, 0);
                Projectile p = Object.Instantiate<Projectile>(player.projectileFab, projectilePos, projectileRot);

                fireTimer = FIRETIMERMAX;

                player.gameObject.GetComponent<AudioSource>().PlayOneShot(player.projectileSFX);
            }
        }

        public void Aim(PlayerCharacter player)
        {
            Transform gun = player.transform.GetChild(0);

            RaycastHit hitInfo;
            Vector3 targetPos;
            var ray = Camera.main.ScreenPointToRay(player.reticlePos);
            if (Physics.Raycast(ray, out hitInfo, 1000))
            {
                targetPos = hitInfo.point;
                gun.LookAt(targetPos);
            }
            else
            {
                // Roughly aim in the right direction if ray hits nothing
                targetPos = player.reticlePos;
                targetPos.x -= Screen.width / 2;
                targetPos.y -= Screen.height / 2;
                var mousePosX = targetPos.x;

                // Look at a spot around an imaginary sphere around the character
                var targetRadius = player.radius;
                targetPos.z = Utility.getZOnSphere(targetPos.x, targetPos.y, targetRadius);
                targetPos = player.transform.TransformPoint(targetPos);
                gun.LookAt(targetPos);

                // Account for the camera's rotated view
                var camera = GameObject.FindGameObjectWithTag("MainCamera").transform;
                gun.Rotate(new Vector3(camera.rotation.eulerAngles.x * (1 - (Mathf.Abs(mousePosX) / (Screen.width))), 0, 0));
            }
        }

        public void Rotate(PlayerCharacter player)
        {
            // rotate at set speed when mouse is outside of bounding box
            var fastUpperBoundX = Screen.width * (1.0f - (FASTBOUNDX / 100.0f));
            var fastLowerBoundX = Screen.width * FASTBOUNDX / 100;
            var upperBoundX = Screen.width * (1.0f - (BOUNDX / 100.0f));
            var lowerBoundX = Screen.width * BOUNDX / 100;

            if (player.reticlePos.x > fastUpperBoundX)
                player.transform.Rotate(new Vector3(0, player.aimingTurnSpeed * Time.deltaTime * 2, 0));
            else if (player.reticlePos.x > upperBoundX)
                player.transform.Rotate(new Vector3(0, player.aimingTurnSpeed * Time.deltaTime, 0));
            else if (player.reticlePos.x < fastLowerBoundX)
                player.transform.Rotate(new Vector3(0, -player.aimingTurnSpeed * Time.deltaTime * 2, 0));
            else if (player.reticlePos.x < lowerBoundX)
                player.transform.Rotate(new Vector3(0, -player.aimingTurnSpeed * Time.deltaTime, 0));
        }

        public void SetReticlePos(PlayerCharacter player)
        {
            if (Utility.mouseChanged())
                usingMouse = true;
            if (Utility.rightStickChanged())
                usingMouse = false;

            if (usingMouse)
                player.reticlePos = Input.mousePosition;
            else
                setReticlePosController(player);
        }

        private void setReticlePosController(PlayerCharacter player)
        {
            var reticleUpperBoundX = Screen.width * (1.0f - (RETICLEBOUNDX / 100.0f));
            var reticleLowerBoundX = Screen.width * RETICLEBOUNDX / 100;
            var reticleUpperBoundY = Screen.height * (1.0f - (RETICLEBOUNDY / 100.0f));
            var reticleLowerBoundY = Screen.height * RETICLEBOUNDY / 100;

            var upperBoundX = Screen.width * (1.0f - (BOUNDX / 100.0f));
            var lowerBoundX = Screen.width * BOUNDX / 100;

            if (player.reticlePos.x <= reticleUpperBoundX && player.reticlePos.x >= reticleLowerBoundX)
            {
                var newPos = player.reticlePos.x + Input.GetAxis("RightHorizontal") * AIMSENSITIVITY;
                player.reticlePos.x = Mathf.Clamp(newPos, reticleLowerBoundX, reticleUpperBoundX);
            }
            if (player.reticlePos.y <= reticleUpperBoundY && player.reticlePos.y >= reticleLowerBoundY)
            {
                var newPos = player.reticlePos.y + Input.GetAxis("RightVertical") * AIMSENSITIVITY;
                player.reticlePos.y = Mathf.Clamp(newPos, reticleLowerBoundY, reticleUpperBoundY);
            }

            // When stick released, gravitate towards center of screen
            if ((Input.GetAxis("RightHorizontal") == 0 && Input.GetAxis("RightVertical") == 0)
                && (player.reticlePos.x > upperBoundX || player.reticlePos.x < lowerBoundX))
            {
                var targetPos = new Vector2(Screen.width / 2, Screen.height / 2);
                player.reticlePos.x = Mathf.Lerp(player.reticlePos.x, targetPos.x, 0.1f);
            }
        }

        public void Jump(PlayerCharacter player)
        {
            player.LeaveGround();
            movementVec.y = player.jumpSpeed;
            player.GetComponent<CharacterController>().Move(movementVec);
        }

        public void Fall(PlayerCharacter player)
        {
            if (!player.grounded)
            {
                movementVec.y -= player.gravity * Time.deltaTime;
                player.GetComponent<CharacterController>().Move(movementVec);
            }
            else
            {
                movementVec.y = -1;
            }
        }

        public void GetHit(PlayerCharacter player)
        {
            --player.health;
            player.GetComponent<AudioSource>().PlayOneShot(player.hurtSFX);
            if (player.health <= 0)
                Die(player);
        }

        public void Die(PlayerCharacter player)
        {
            player.GetComponent<CharacterController>().enabled = false;
            player.gameObject.AddComponent<CapsuleCollider>();
            player.gameObject.AddComponent<Rigidbody>();
            player.transform.Rotate(new Vector3(1, 0, 1)); // to make sure we fall over dramatically

            player.dead = true;
            player.aiming = false;
        }
    }
}
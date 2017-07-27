using UnityEngine;

public class MeleeState : ScriptableObject, PlayerState
{
    public PlayerState HandleTransition(PlayerCharacter player)
    {
        // hold aim key to go into Shooter State
        if (Input.GetKey("left shift"))
        {
            return ScriptableObject.CreateInstance<ShooterState>();
        }
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
            player.transform.Rotate(new Vector3(0, -player.turnSpeed, 0));
        }
        else if (Input.GetKey("right") || Input.GetKey("d"))
        {
            player.transform.Rotate(new Vector3(0, player.turnSpeed, 0));
        }
    }
}
